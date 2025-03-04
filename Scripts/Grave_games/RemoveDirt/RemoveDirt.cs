using TMPro;
using UnityEngine;

/*  Code taken and modified from Code Monkey 
    and can be found at https://unitycodemonkey.com/video.php?v=Xss4__kgYiY */

/* Stone must have own E text for some reason */

public class RemoveDirtGrave : MonoBehaviour
{
    [Header("Textures")]
    [SerializeField] private Texture2D dirtMaskTextureBase;
    [SerializeField] private Texture2D dirtBrush;
    [SerializeField] private Material material;
    [Space]
    [Header("Ui Text")]
    [SerializeField] public TMP_Text eText;
    [SerializeField] public TMP_Text qText;
    [SerializeField] public TMP_Text totalDirtLeft;
    [Space]
    [Header("Input parameters")]
    [SerializeField] public KeyCode interactionKeyEnter = KeyCode.E;
    [SerializeField] public KeyCode interactionKeyLeave = KeyCode.Q;
    [SerializeField] public Camera staticCamera;

    // ---
    private Texture2D dirtMaskTexture;
    private float dirtAmountTotal;
    private float dirtAmount;
    private Vector2Int lastPaintPixelPosition;

    private CameraManager cameraManager;
    private Transform playerCamera;
    private Transform playerStatic;
    private SubtaskHandler subtaskHandler;
    private MeshCollider noInteraction;

    private void Awake()
    {
        // Mini game stuff
        dirtMaskTexture = new Texture2D(dirtMaskTextureBase.width, dirtMaskTextureBase.height);
        dirtMaskTexture.SetPixels(dirtMaskTextureBase.GetPixels());
        dirtMaskTexture.Apply();
        material.SetTexture("_DirtMask", dirtMaskTexture);

        dirtAmountTotal = 0f;
        for (int x = 0; x < dirtMaskTextureBase.width; x++)
        {
            for (int y = 0; y < dirtMaskTextureBase.height; y++)
            {
                dirtAmountTotal += dirtMaskTextureBase.GetPixel(x, y).g;
            }
        }
        dirtAmount = dirtAmountTotal;

        subtaskHandler = gameObject.GetComponent<SubtaskHandler>();
        
        // Camera stuff
        playerCamera = Camera.main.transform;
        playerStatic = staticCamera.transform;
        cameraManager = FindObjectOfType<CameraManager>();

        if (cameraManager == null)
        {
            Debug.LogError("CameraManager not found in the scene! (RemoveDirt)");
        }
    }

    private void Update()
    {
        // Här kollar du hur mycket smuts som är kvar och uppdaterar UI-texten
        float dirtPercentageLeft = GetDirtAmount() * 100f;
        totalDirtLeft.text = "Cleaning done: " + Mathf.CeilToInt(100f - dirtPercentageLeft) + "%";

        if(GetDirtAmount() <= 0.01f){
            subtaskHandler.taskComp = true;

            // Not able to interact no more with the stone when done
            noInteraction = gameObject.GetComponent<MeshCollider>();
            noInteraction.enabled = false;

            if (Input.GetKeyDown(interactionKeyLeave))
            {
                ResetCamera();
                this.enabled = false;
            }
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                eText.gameObject.SetActive(true);
                Debug.Log("Hit object: " + hit.collider.gameObject.name);

                if (Input.GetKeyDown(interactionKeyEnter))
                {
                    Activate();
                }
                if (Input.GetKeyDown(interactionKeyLeave))
                {
                    ResetCamera();
                }
            }
            else
            {
                if (eText)
                {
                    eText.gameObject.SetActive(false);
                }
            }
        }

        /* UV COORD: GAME PART */
        if (staticCamera.enabled)
        {
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(staticCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
                {
                    Vector2 textureCoord = raycastHit.textureCoord;

                    int pixelX = (int)(textureCoord.x * dirtMaskTexture.width);
                    int pixelY = (int)(textureCoord.y * dirtMaskTexture.height);

                    Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);
                    //Debug.Log("UV: " + textureCoord + "; Pixels: " + paintPixelPosition);

                    int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) + Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
                    int maxPaintDistance = 2;
                    if (paintPixelDistance < maxPaintDistance)
                    {
                        // Painting too close to last position
                        return;
                    }
                    lastPaintPixelPosition = paintPixelPosition;

                    int pixelXOffset = pixelX - (dirtBrush.width / 2);
                    int pixelYOffset = pixelY - (dirtBrush.height / 2);

                    for (int x = 0; x < dirtBrush.width; x++)
                    {
                        for (int y = 0; y < dirtBrush.height; y++)
                        {
                            Color pixelDirt = dirtBrush.GetPixel(x, y);
                            Color pixelDirtMask = dirtMaskTexture.GetPixel(pixelXOffset + x, pixelYOffset + y);

                            float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                            dirtAmount -= removedAmount;

                            dirtMaskTexture.SetPixel(
                                pixelXOffset + x,
                                pixelYOffset + y,
                                new Color(0, pixelDirtMask.g * pixelDirt.g, 0)
                            );
                        }
                    }

                    dirtMaskTexture.Apply();
                }
            }
        }
    }
    // For % of dirt left
    private float GetDirtAmount()
    {
        return this.dirtAmount / dirtAmountTotal;
    }

    // Camera switching
    private void Activate()
    {
        cameraManager.SetActiveCamera(staticCamera);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        totalDirtLeft.gameObject.SetActive(true);
        qText.gameObject.SetActive(true);
        eText.gameObject.SetActive(false);
    }

    private void ResetCamera()
    {
        cameraManager.deactivateSubCamera();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        qText.gameObject.SetActive(false);
        totalDirtLeft.gameObject.SetActive(false);
    }

}

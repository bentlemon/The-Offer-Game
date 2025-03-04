using TMPro;
using UnityEngine;

public class HideBush : MonoBehaviour
{
    [SerializeField]
    [Header("Inputs")]
    public float interactionDistance = 5f;
    public KeyCode interactionKeyEnter = KeyCode.E;
    public KeyCode interactionKeyLeave = KeyCode.Q;

    [Header("Objects")]
    public Camera bushCamera;
    public Material bushMaterial;
    public Transform player;

    [SerializeField]
    [Header("Ui Text")]
    public TMP_Text eText;
    public TMP_Text qText;
    public TMP_Text hideText;
    public TMP_Text instuctionsToClean;
    public TMP_Text getoutText;

    [SerializeField]
    [Header("Events")]
    public GameEvent onPlayerHides;

    // ---
    private Animator animator;
    private Transform playerCamera;
    private MeshRenderer meshRenderer;
    private CameraManager cameraManager;

    private bool isLookingAtBush = false; // Flag to track if player is looking at bush

    private void Awake()
    {
        playerCamera = Camera.main.transform;
        animator = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();

        cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager == null)
        {
            Debug.LogError("CameraManager not found in the scene! (HideBush)");
        }
    }

    private void Update()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!isLookingAtBush)
                {
                    isLookingAtBush = true; // Player is looking at bush
                    eText.gameObject.SetActive(true);
                    hideText.gameObject.SetActive(true);
                }

                if (Input.GetKeyDown(interactionKeyEnter))
                {
                    Activate();
                    player.gameObject.SetActive(false);
                }

                if (Input.GetKeyDown(interactionKeyLeave))
                {
                    DeactivateUI();
                    ResetCamera();
                    player.gameObject.SetActive(true);
                }
            }
        }
        else if (isLookingAtBush)
        {
            // Player no longer looking at bush, deactivate UI
            isLookingAtBush = false;
            DeactivateUI();
        }
    }

    private void Activate()
    {
        cameraManager.SetActiveCamera(bushCamera);

        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }

        eText.gameObject.SetActive(false);
        hideText.gameObject.SetActive(false);
        qText.gameObject.SetActive(true);
        getoutText.gameObject.SetActive(true);

        onPlayerHides.Raise(this, true);
    }

    private void ResetCamera()
    {
        cameraManager.deactivateSubCamera();

        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
        }

        qText.gameObject.SetActive(false);
        getoutText.gameObject.SetActive(false);
    }

    private void DeactivateUI()
    {
        eText.gameObject.SetActive(false);
        hideText.gameObject.SetActive(false);
    }
}

using TMPro;
using UnityEngine;
using System.Collections;

public class PickUpItem : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] public Camera mainCamera;
    [SerializeField] public LayerMask ignoredLayerMask;
    [SerializeField] public KeyCode interactionKey = KeyCode.E;

    [Header("UI Text")]
    [SerializeField] public TMP_Text eText;  // Text showing "Press E to interact"
    [SerializeField] public TMP_Text uiText; // Text showing the pickup message

    private GameObject[] pickUpObjects; // Array to store multiple PickUp objects
    private GameObject currentPickUpObject; // The object currently being looked at
    private SubtaskHandler subtaskHandler;
    private SimpleTextUi textDialoge; // To get the message attached to the obj
    private bool isDisplayingText = false;

    void Start()
    {
        uiText.gameObject.SetActive(false);

        // Find all objects with the tag "PickUp"
        pickUpObjects = GameObject.FindGameObjectsWithTag("PickUp");

        if (pickUpObjects.Length == 0)
        {
            //Debug.Log("No PickUp objects found!");
            return;
        }
    }

    void Update()
    {
        Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(mouseRay, out hit, 50f);

        if (hitSomething)
        {
            // Loop through all pickUpObjects to check if we are looking at any of them
            foreach (GameObject pickUpObject in pickUpObjects)
            {
                if (hit.collider.gameObject == pickUpObject)
                {
                    currentPickUpObject = pickUpObject; // Store the object we're looking at
                    eText.gameObject.SetActive(true);

                    if (pickUpObject.TryGetComponent<Outline>(out var outline))
                    {
                        outline.enabled = true;
                    }

                    if (Input.GetKeyDown(interactionKey))
                    {
                        HandlePickUp();
                    }

                    return; // Exit the loop once we've found a pick-up object
                }
            }
        }

        // If we're not looking at any PickUp object, disable the interaction
        DisablePickUpInteraction();
    }

    void HandlePickUp()
    {
        var meshRenderer = currentPickUpObject.GetComponent<MeshRenderer>();
        var collider = currentPickUpObject.GetComponent<Collider>();
        subtaskHandler = currentPickUpObject.GetComponent<SubtaskHandler>();
        textDialoge = currentPickUpObject.GetComponent<SimpleTextUi>();

        string textToShow = textDialoge.interactionText;

        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }

        if (collider != null)
        {
            collider.enabled = false;
        }

        subtaskHandler.taskComp = true;
        eText.gameObject.SetActive(false);

        StartCoroutine(ShowText(textToShow));
    }

    IEnumerator ShowText(string message)
    {
        isDisplayingText = true;
        eText.gameObject.SetActive(false); // Hide interaction text
        uiText.gameObject.SetActive(true); // Show the message

        string currentText = "";
        for (int i = 0; i < message.Length; i++)
        {
            currentText = message.Substring(0, i + 1);
            uiText.text = currentText;
            yield return new WaitForSeconds(0.05f); // Control typing speed
        }

        yield return new WaitForSeconds(2f); // Hold the message for 2 seconds
        uiText.gameObject.SetActive(false);  // Hide the message after a delay
        isDisplayingText = false;
    }

    void DisablePickUpInteraction()
    {
        if (currentPickUpObject != null && currentPickUpObject.TryGetComponent<Outline>(out var outline))
        {
            outline.enabled = false; // Disable outline when not looking at the object
        }
        eText.gameObject.SetActive(false);  // Hide interaction text
        currentPickUpObject = null; // Reset the current pick-up object reference
    }
}

using TMPro;
using UnityEngine;

public class NoteInteraction : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] public LayerMask ignoredLayerMask;
    [SerializeField] public KeyCode interactionKeyShow = KeyCode.E;
    [SerializeField] public KeyCode interactionKeyHide = KeyCode.Q;

    [Space]
    [Header("UI Text")]
    [SerializeField] public TMP_Text eText;  // Text showing "Press E to interact"
    [SerializeField] public TMP_Text qText;  // Text showing the pickup message
    [SerializeField] public TMP_Text spaceText;

    [SerializeField] private Canvas noteCanvas; // Referens till canvasen som innehåller notisen
    [SerializeField] private BoxCollider interactionArea;

    private GameObject[] noteObjects;       // Array to store multiple PickUp objects
    private bool noteVisible = false;       // Track if the note is currently visible
    private Outline currentOutline;         // To keep track of the current outline

    void Start()
    {
        noteObjects = GameObject.FindGameObjectsWithTag("Note");

        if (noteObjects.Length == 0)
        {
            Debug.LogError("No note objects found!");
            return;
        }

        eText.gameObject.SetActive(false); // Start with eText hidden
    }

    void Update()
    {
        if (Camera.main == null)
        {
            return;
        }

        bool isInInteractionArea = interactionArea != null && interactionArea.bounds.Contains(Camera.main.transform.position);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits something
        if (Physics.Raycast(mouseRay, out hit, 50f))
        {
            bool hitNote = false; // Flag to check if we hit a note
            foreach (GameObject note in noteObjects)
            {
                if (hit.collider.gameObject == note)
                {
                    if (!noteVisible)
                    {
                        eText.gameObject.SetActive(true); // Show interaction text
                    }
                    hitNote = true; // Mark that we hit a note
                    Debug.Log("Hit object: " + hit.collider.gameObject.name);

                    if (Input.GetKeyDown(interactionKeyShow) && !noteVisible)
                    {
                        eText.gameObject.SetActive(false); // Hide interaction text
                        ShowNote();
                    }

                    // Activate outline if note is hit
                    if (hit.collider.TryGetComponent<Outline>(out var outline))
                    {
                        if (currentOutline != null && currentOutline != outline)
                        {
                            currentOutline.enabled = false; // Disable previous outline
                        }

                        currentOutline = outline;
                        currentOutline.enabled = true; // Enable current outline
                    }

                    break; // Break loop since we found a note
                }
            }

            // Hide interaction text and disable outline if not hitting any note
            if (!hitNote)
            {
                eText.gameObject.SetActive(false);

                if (currentOutline != null)
                {
                    currentOutline.enabled = false; // Disable the outline
                    currentOutline = null;
                }
            }
        }
        else
        {
            eText.gameObject.SetActive(false); // Hide the interaction text if not hitting a note

            if (currentOutline != null)
            {
                currentOutline.enabled = false; // Disable the outline
                currentOutline = null;
            }
        }

        // If the note is visible and the player is outside the interaction area, hide the note
        if (noteVisible && !isInInteractionArea)
        {
            HideNote();
        }

        // Allow the player to hide the note with Q key
        if (noteVisible && Input.GetKeyDown(interactionKeyHide))
        {
            HideNote();
        }
    }

    void ShowNote()
    {
        spaceText.gameObject.SetActive(true);
        qText.gameObject.SetActive(true);
        noteCanvas.gameObject.SetActive(true); // Activate note canvas
        noteVisible = true;

        // Disable outline once the note is shown
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }
    }

    void HideNote()
    {
        spaceText.gameObject.SetActive(false);
        qText.gameObject.SetActive(false);
        noteCanvas.gameObject.SetActive(false); // Deactivate note canvas
        noteVisible = false;
        eText.gameObject.SetActive(false); // Ensure eText is hidden when hiding the note

        // Ensure outline is disabled when the note is hidden
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }
    }
}

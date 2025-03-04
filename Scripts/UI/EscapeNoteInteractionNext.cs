using System.Collections;
using TMPro;
using UnityEngine;

public class EscapeNoteInteractionNext : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] public LayerMask ignoredLayerMask;
    [SerializeField] public KeyCode interactionKeyShow = KeyCode.E;
    [SerializeField] public KeyCode interactionKeyHide = KeyCode.Q;

    [Space]
    [Header("UI Text")]
    [SerializeField] public TMP_Text eText;  
    [SerializeField] public TMP_Text qText;  
    [SerializeField] public TMP_Text spaceText;

    [SerializeField] private Canvas noteCanvas; 
    [SerializeField] private BoxCollider interactionArea;

    private GameObject[] noteObjects;      
    private bool noteVisible = false;       
    private Outline currentOutline;         

    void Start()
    {
        noteObjects = GameObject.FindGameObjectsWithTag("Note");

        if (noteObjects.Length == 0)
        {
            Debug.LogError("Inga notobjekt hittades!");
            return;
        }

        eText.gameObject.SetActive(false); 
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

        if (Physics.Raycast(mouseRay, out hit, 50f))
        {
            bool hitNote = false;
            foreach (GameObject note in noteObjects)
            {
                if (hit.collider.gameObject == note)
                {
                    if (!noteVisible)
                    {
                        eText.gameObject.SetActive(true); // Visa interaktionstext
                    }
                    hitNote = true; // Markera att vi träffade en not
                    Debug.Log("Träffade objekt: " + hit.collider.gameObject.name);

                    if (Input.GetKeyDown(interactionKeyShow) && !noteVisible)
                    {
                        eText.gameObject.SetActive(false); // Dölja interaktionstext
                        ShowNote();
                    }

                    // Aktivera outline om noten träffas
                    if (hit.collider.TryGetComponent<Outline>(out var outline))
                    {
                        if (currentOutline != null && currentOutline != outline)
                        {
                            currentOutline.enabled = false; // Inaktivera föregående outline
                        }

                        currentOutline = outline;
                        currentOutline.enabled = true; // Aktivera nuvarande outline
                    }

                    break; // Avbryt loop eftersom vi hittade en not
                }
            }

            // Dölja interaktionstext och inaktivera outline om vi inte träffar någon not
            if (!hitNote)
            {
                eText.gameObject.SetActive(false);

                if (currentOutline != null)
                {
                    currentOutline.enabled = false; // Inaktivera outline
                    currentOutline = null;
                }
            }
        }
        else
        {
            eText.gameObject.SetActive(false); // Dölja interaktionstext om vi inte träffar någon not

            if (currentOutline != null)
            {
                currentOutline.enabled = false; // Inaktivera outline
                currentOutline = null;
            }
        }

        // Om noten är synlig och spelaren är utanför interaktionsområdet, dölja noten
        if (noteVisible && !isInInteractionArea)
        {
            HideNote();
        }

        // Tillåt spelaren att dölja noten med Q-tangenten
        if (noteVisible && Input.GetKeyDown(interactionKeyHide))
        {
            HideNote();
        }
    }

    void ShowNote()
    {
        spaceText.gameObject.SetActive(true);
        qText.gameObject.SetActive(true);
        noteCanvas.gameObject.SetActive(true); // Aktivera notcanvas
        noteVisible = true;

        // Inaktivera outline när noten visas
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }
    }

    void HideNote()
    {
        spaceText.gameObject.SetActive(false);
        qText.gameObject.SetActive(false);
        noteCanvas.gameObject.SetActive(false); // Inaktivera notcanvas
        noteVisible = false;
        eText.gameObject.SetActive(false); // Se till att eText är gömd när noten döljs

        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }
    }
}

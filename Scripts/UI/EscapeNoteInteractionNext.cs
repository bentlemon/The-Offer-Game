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
                    hitNote = true; // Markera att vi tr�ffade en not
                    Debug.Log("Tr�ffade objekt: " + hit.collider.gameObject.name);

                    if (Input.GetKeyDown(interactionKeyShow) && !noteVisible)
                    {
                        eText.gameObject.SetActive(false); // D�lja interaktionstext
                        ShowNote();
                    }

                    // Aktivera outline om noten tr�ffas
                    if (hit.collider.TryGetComponent<Outline>(out var outline))
                    {
                        if (currentOutline != null && currentOutline != outline)
                        {
                            currentOutline.enabled = false; // Inaktivera f�reg�ende outline
                        }

                        currentOutline = outline;
                        currentOutline.enabled = true; // Aktivera nuvarande outline
                    }

                    break; // Avbryt loop eftersom vi hittade en not
                }
            }

            // D�lja interaktionstext och inaktivera outline om vi inte tr�ffar n�gon not
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
            eText.gameObject.SetActive(false); // D�lja interaktionstext om vi inte tr�ffar n�gon not

            if (currentOutline != null)
            {
                currentOutline.enabled = false; // Inaktivera outline
                currentOutline = null;
            }
        }

        // Om noten �r synlig och spelaren �r utanf�r interaktionsomr�det, d�lja noten
        if (noteVisible && !isInInteractionArea)
        {
            HideNote();
        }

        // Till�t spelaren att d�lja noten med Q-tangenten
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

        // Inaktivera outline n�r noten visas
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
        eText.gameObject.SetActive(false); // Se till att eText �r g�md n�r noten d�ljs

        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }
    }
}

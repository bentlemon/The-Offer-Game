using System.Collections;
using TMPro;
using UnityEngine;

public class AngelInteractionKey : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] public LayerMask ignoredLayerMask;
    [SerializeField] public KeyCode interactionKeyShow = KeyCode.E;

    [Space]
    [Header("UI Text")]
    [SerializeField] private TMP_Text eText;
    [SerializeField] private TMP_Text infoText; // Anv�nd infoText som en st�ng

    private Outline currentOutline;
    private SubtaskHandler subtaskHandler;
    private bool hasInteracted = false;
    private bool isDisplayingText = false; // F�r att hantera om texten visas eller inte

    void Start()
    {
        subtaskHandler = gameObject.GetComponent<SubtaskHandler>();
        eText.gameObject.SetActive(false);
        infoText.gameObject.SetActive(false); // D�lja infotexten i b�rjan
    }

    void Update()
    {
        if (Camera.main == null)
        {
            return;
        }

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit, 50f))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Angel") && hit.collider.gameObject.name == gameObject.name)
            {
                Debug.Log("Tr�ffade objekt: " + hit.collider.gameObject.name);

                // Visa interaktionstext om spelaren inte har interagerat
                if (!hasInteracted)
                {
                    eText.gameObject.SetActive(true);
                }

                // Kontrollera om spelaren trycker p� interaktionsknappen
                if (Input.GetKeyDown(interactionKeyShow) && !hasInteracted)
                {
                    eText.gameObject.SetActive(false);
                    hasInteracted = true;
                    subtaskHandler.taskComp = true;
                    Debug.Log("Spelaren har interagerat med �ngeln!");

                    string[] paragraphs = {
                        infoText.text, 
                        "I can finally get out!" 
                    };
                    StartCoroutine(ShowText(paragraphs));
                }

                // Hantera outline
                if (hit.collider.TryGetComponent<Outline>(out var outline))
                {
                    if (currentOutline != null && currentOutline != outline)
                    {
                        currentOutline.enabled = false;
                    }

                    currentOutline = outline;
                    currentOutline.enabled = true;
                }
            }
            else
            {
                eText.gameObject.SetActive(false);

                if (currentOutline != null)
                {
                    currentOutline.enabled = false;
                    currentOutline = null;
                }
            }
        }
        else
        {
            eText.gameObject.SetActive(false);

            if (currentOutline != null)
            {
                currentOutline.enabled = false;
                currentOutline = null;
            }
        }
    }

    private IEnumerator ShowText(string[] paragraphs)
    {
        isDisplayingText = true;
        infoText.gameObject.SetActive(true);
        for (int i = 0; i < paragraphs.Length; i++)
        {
            yield return StartCoroutine(ShowParagraph(paragraphs[i])); // Visa varje paragraf
            yield return new WaitForSeconds(1f); // V�nta i 1 sekund mellan stycken

            // Om det inte �r den sista paragrafen, v�nta i 3 sekunder innan n�sta paragraf
            if (i < paragraphs.Length - 1)
            {
                yield return new WaitForSeconds(3f); // V�nta i 3 sekunder efter sista stycket
            }
        }
        infoText.gameObject.SetActive(false); // D�lja infotexten
        isDisplayingText = false;
    }

    private IEnumerator ShowParagraph(string text)
    {
        string currentText = "";
        for (int i = 0; i < text.Length; i++)
        {
            currentText += text[i]; // Bygg texten tecken f�r tecken
            infoText.text = currentText; // Uppdatera texten p� infoText
            yield return new WaitForSeconds(0.1f); // Typwriter-effekt
        }
    }
}

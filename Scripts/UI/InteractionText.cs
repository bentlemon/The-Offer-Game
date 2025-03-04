using System.Collections;
using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField]
    public float interactionDistance = 5f;
    public KeyCode interactionKeyEnter = KeyCode.E;
    public TMP_Text eText;
    public TMP_Text uiText;

    private GameObject currentInteractable;
    private bool isDisplayingText = false;

    void Start()
    {
        uiText.gameObject.SetActive(false);
    }

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                currentInteractable = hit.collider.gameObject;
                eText.gameObject.SetActive(true);

                if (Input.GetKeyDown(interactionKeyEnter) && !isDisplayingText)
                { 
                    var interactable = currentInteractable.GetComponent<InteractiveObject>();
                    if (interactable != null)
                    {
                        StartCoroutine(ShowText(interactable.paragraphs));
                    }
                }
            }
        }
        else
        {
            eText.gameObject.SetActive(false); 
            currentInteractable = null; 
        }
    }

    IEnumerator ShowText(string[] paragraphs)
    {
        isDisplayingText = true;
        uiText.gameObject.SetActive(true); 
        for (int i = 0; i < paragraphs.Length; i++) 
        {
            uiText.text = "";
            yield return StartCoroutine(ShowParagraph(paragraphs[i])); 
            yield return new WaitForSeconds(1f); 

            if (i < paragraphs.Length - 1)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }
            else
            {
                yield return new WaitForSeconds(3f);
            }
        }
        uiText.gameObject.SetActive(false); 
        isDisplayingText = false; 
    }

    IEnumerator ShowParagraph(string text)
    {
        string currentText = "";
        for (int i = 0; i < text.Length; i++)
        {
            currentText = text.Substring(0, i + 1);
            uiText.text = currentText;
            yield return new WaitForSeconds(0.1f); 
        }
    }
}

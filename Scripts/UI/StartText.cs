using System.Collections;
using UnityEngine;
using TMPro;


/* For TRANSITIONSSCENES with just text:
    Use this script and add paragraphs 
    in the inspector */

/* For interactible obj: Use the
    Interaction Manager script on 
    main canvas + Interactobjects on
    the object */

public class ActiveTextStart : MonoBehaviour
{
    [SerializeField]
    public float textSpeed = 0.1f;
    public string[] paragraphs;
    public TMP_Text eText;

    private string currentText = "";
    private TMP_Text uiText;

    void Start()
    {
        uiText = this.GetComponent<TMP_Text>();
        eText.gameObject.SetActive(false);
        StartCoroutine(ShowParagraphs());
    }

    IEnumerator ShowParagraphs()
    {
        foreach (string paragraph in paragraphs) // Loopa genom varje stycke
        {
            yield return StartCoroutine(ShowText(paragraph)); // Visa text
            yield return new WaitForSeconds(textSpeed);
            eText.gameObject.SetActive(true); // Visa [E]-texten

            // Vänta tills spelaren trycker på "E" för att fortsätta
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            eText.gameObject.SetActive(false);
        }
        //eText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    IEnumerator ShowText(string text)
    {
        currentText = "";
        for (int i = 0; i < text.Length; i++)
        {
            currentText = text.Substring(0, i + 1);
            uiText.text = currentText;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}

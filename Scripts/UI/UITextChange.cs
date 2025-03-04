using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


/* For TRANSITIONSSCENES with just text:
    Use this script and add paragraphs 
    in the inspector */

/* For interactible obj: Use the
    Interaction Manager script on 
    main canvas + Interactobjects on
    the object */

public class UITextChange : MonoBehaviour
{
    [SerializeField]
    public float textSpeed = 0.1f;
    public string[] paragraphs; 
    public TMP_Text eText; 
    public bool changeLevelTrigger = false;
    public bool backToMenuLevelTrigger = false;
    public bool changeToChosenScene = false;
    public string chosenSceneName = "";

    // ---
    private string currentText = "";
    private TMP_Text uiText;
    private LevelLoader levelLoader;

    void Start()
    {
        uiText = this.GetComponent<TMP_Text>(); 
        eText.gameObject.SetActive(false); 
        StartCoroutine(ShowParagraphs());

        levelLoader = FindObjectOfType<LevelLoader>();
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
        if (changeLevelTrigger)
        {
            changeLevel();
        }
        if (backToMenuLevelTrigger)
        {
            RestartGame();
        }
        if (changeToChosenScene) { 
            levelLoader.LoadNextChosenLevel(chosenSceneName);
        }
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

    void changeLevel()
    {
        levelLoader.LoadNextLevel();
    }

    void RestartGame()
    {
        // Back to main menu
        SceneManager.LoadScene(0);
    }
}

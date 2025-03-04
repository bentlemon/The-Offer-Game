using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI startArrow;
    public TextMeshProUGUI creditsArrow;
    public TextMeshProUGUI quitArrow;
    public TextMeshProUGUI backArrow;

    public GameObject mainMenuCanvas;  
    public GameObject creditsCanvas;   

    private LevelLoader levelLoader;

    private void Start()
    {
        ShowMainMenu();
        HideAllArrows();
        levelLoader = FindObjectOfType<LevelLoader>(); 
    }

    private void HideAllArrows()
    {
        startArrow.text = "";
        creditsArrow.text = "";
        quitArrow.text = "";
        backArrow.text = "";
    }

    // Click funktioner -----------------------
    public void StartGame()
    {
        levelLoader.LoadNextLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
    }

    public void ShowCredits()
    {
        creditsCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }

    public void BackToMainMenu()
    {
        ShowMainMenu();
    }

    // Hover-funktioner för "<" -------------------
    public void OnStartHover()
    {
        HideAllArrows();
        startArrow.text = ">";
    }

    public void OnCreditsHover()
    {
        HideAllArrows();
        creditsArrow.text = ">";
    }

    public void OnQuitHover()
    {
        HideAllArrows();
        quitArrow.text = ">";
    }

    public void OnBackHover()
    {
        HideAllArrows();
        backArrow.text = "<";
    }

    public void OnExitHover()
    {
        HideAllArrows(); 
    }
}

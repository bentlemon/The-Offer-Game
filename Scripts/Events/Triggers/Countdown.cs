using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public float timeRemaining = 10f; // Total tid i sekunder
    public bool timerIsRunning = false;
    public TMP_Text timerText;

    private float activationTime; // Tiden då ljudet ska aktiveras
    private bool soundPlayed = false; // Flagga för att kontrollera om ljudet har spelats
    private PlaySoundOnActivate soundScript; // Referens till ljudskriptet

    public LevelLoader levelLoader;

    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        soundScript = GetComponent<PlaySoundOnActivate>(); // Hämta ljudskriptet
        activationTime = timeRemaining * 0.1f; // 10% av total tid (10 sekunder blir 1 sekund)
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

                // Kontrollera om det är dags att spela ljudet
                if (!soundPlayed && timeRemaining <= activationTime)
                {
                    soundScript.ActivateSound(); // Spela ljudet
                    soundPlayed = true; // Sätt flaggan för att förhindra att ljudet spelas igen
                }
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

 
}

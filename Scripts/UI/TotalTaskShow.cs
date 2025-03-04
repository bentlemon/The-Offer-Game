using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TaskCounterUI : MonoBehaviour
{
    public List<GameObject> tasks; // Listan med uppgifter
    public TMP_Text taskCounterText; // UI-element för att visa antalet uppgifter
    private int completedTasks; // Antal slutförda deluppgifter
    private int previousCompletedTasks; // Förra antalet slutförda uppgifter
    private int totalTasks; // Totalt antal uppgifter

    private void Start()
    {
        completedTasks = 0;
        previousCompletedTasks = 0; // Initiera med 0
        totalTasks = tasks.Count; // Sätt det totala antalet uppgifter
        UpdateTaskCounter(); // Uppdatera UI i början
    }

    private void Update()
    {
        completedTasks = 0; // Reset varje frame

        foreach (GameObject task in tasks)
        {
            SubtaskTracker tracker = task.GetComponent<SubtaskTracker>(); // Hämta SubtaskTracker från varje task

            if (tracker != null && tracker.allTasksComplete) // Kontrollera om alla uppgifter är slutförda
            {
                completedTasks++; // Öka antalet slutförda uppgifter
            }
        }

        // Kontrollera om antalet slutförda uppgifter har förändrats
        if (completedTasks != previousCompletedTasks)
        {
            UpdateTaskCounter(); // Uppdatera UI om det har förändrats
            previousCompletedTasks = completedTasks; // Spara det nya antalet
        }
    }

    private void UpdateTaskCounter()
    {
        // Här uppdaterar du UI med det aktuella antalet slutförda uppgifter
        taskCounterText.text = $"{completedTasks}/{totalTasks} Done"; // Formaterar texten
    }
}

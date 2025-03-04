using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TaskCounterUI : MonoBehaviour
{
    public List<GameObject> tasks; // Listan med uppgifter
    public TMP_Text taskCounterText; // UI-element f�r att visa antalet uppgifter
    private int completedTasks; // Antal slutf�rda deluppgifter
    private int previousCompletedTasks; // F�rra antalet slutf�rda uppgifter
    private int totalTasks; // Totalt antal uppgifter

    private void Start()
    {
        completedTasks = 0;
        previousCompletedTasks = 0; // Initiera med 0
        totalTasks = tasks.Count; // S�tt det totala antalet uppgifter
        UpdateTaskCounter(); // Uppdatera UI i b�rjan
    }

    private void Update()
    {
        completedTasks = 0; // Reset varje frame

        foreach (GameObject task in tasks)
        {
            SubtaskTracker tracker = task.GetComponent<SubtaskTracker>(); // H�mta SubtaskTracker fr�n varje task

            if (tracker != null && tracker.allTasksComplete) // Kontrollera om alla uppgifter �r slutf�rda
            {
                completedTasks++; // �ka antalet slutf�rda uppgifter
            }
        }

        // Kontrollera om antalet slutf�rda uppgifter har f�r�ndrats
        if (completedTasks != previousCompletedTasks)
        {
            UpdateTaskCounter(); // Uppdatera UI om det har f�r�ndrats
            previousCompletedTasks = completedTasks; // Spara det nya antalet
        }
    }

    private void UpdateTaskCounter()
    {
        // H�r uppdaterar du UI med det aktuella antalet slutf�rda uppgifter
        taskCounterText.text = $"{completedTasks}/{totalTasks} Done"; // Formaterar texten
    }
}

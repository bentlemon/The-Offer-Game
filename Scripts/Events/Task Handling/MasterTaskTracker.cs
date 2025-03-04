using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MasterTaskTrackerDayTime : MonoBehaviour
{
    [Space(5)]
    [Header("Chronological Order of Tasks")]
    [SerializeField] public List<GameObject> sceneTasks;

    [Space(5)]
    [Header("Other stuff")]
    [SerializeField] public TMP_Text gettingDarkText; // Referens till TextMeshPro-objektet
    [SerializeField] public TMP_Text shiftToRun; // Referens till TextMeshPro-objektet
    [SerializeField] public int afterWhichTaskElement; // Index för tasken efter vilken texten ska aktiveras
    private bool graveTasksActive = false;

    // Referens till TotalTaskUI
    [SerializeField] private GameObject totalTaskUI; // UI-elementet som ska aktiveras

    // --- 
    private TaskTracker taskTracker;
    private int onTask = 1;
    private LevelLoader levelLoader;
    bool shiftShownFlag = false;

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        if (levelLoader == null)
        {
            Debug.LogError("LevelLoader not found in the scene!");
        }

        // Dölj TotalTaskUI initialt
        if (totalTaskUI != null)
        {
            totalTaskUI.SetActive(false);
        }

        // Dölj gettingDarkText initialt
        if (gettingDarkText != null)
        {
            gettingDarkText.gameObject.SetActive(false); // Dölja texten i början
        }

    }

    void Update()
    {
        // Bevara logiken för sceneTask[0]
        if (sceneTasks[0].GetComponent<TaskTracker>().allTasksComplete && !graveTasksActive)
        {
            onTask = 2;
            graveTasksActive = true;

            // Aktivera TotalTaskUI
            if (totalTaskUI != null)
            {
                totalTaskUI.SetActive(true); // Aktivera UI när alla uppgifter är klara
            }

            // Deaktivera opendoor_task paritvlezone
            var effectActiveZone = sceneTasks[0].transform.Find("OpenDoor_task/EffectActiveZone");
            if (effectActiveZone != null)
            {
                effectActiveZone.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("EffectActiveZone not found under first main task!");
            }

            // Aktivera ActiveZones för sceneTasks[1]
            ActivateAllActiveZones(sceneTasks[1].transform, "EffectActiveZone");

            // Aktivera skript för RemoveDirtGrave i sceneTasks[1]
            RemoveDirtGrave[] cleanStones = sceneTasks[1].GetComponentsInChildren<RemoveDirtGrave>();
            foreach (RemoveDirtGrave stones in cleanStones)
            {
                if (stones != null)
                {
                    stones.enabled = true;
                }
            }

            // Aktivera RemoveWeed på kameran
            var cameraObject = Camera.main;
            if (cameraObject.TryGetComponent<RemoveWeed>(out var weedRemovalScript))
            {
                weedRemovalScript.enabled = true;
            }
            else
            {
                Debug.LogError("RemoveWeed script not found on the main camera!");
            }
        }

        // Sekventiell aktivering för resten av sceneTasks (från sceneTask[1] och uppåt)
        if (onTask > 1 && onTask < sceneTasks.Count)
        {
            if (sceneTasks[onTask - 1].GetComponent<TaskTracker>().allTasksComplete)
            {
                // Deaktivera föregående task's EffectActiveZone
                var previousEffectActiveZone = sceneTasks[onTask - 1].transform.Find("EffectActiveZone");
                if (previousEffectActiveZone != null)
                {
                    previousEffectActiveZone.gameObject.SetActive(false);
                }

                ActivateNextTask(onTask);
                onTask++;  // Öka onTask till nästa

                if (onTask == afterWhichTaskElement + 1)
                {
                    ActivateGettingDarkText();
                }
            }
            if (onTask == 4 && !shiftShownFlag)
            {
                shiftShownFlag = !shiftShownFlag;
                ActivateShiftInstruction();
            }
        }

        // Kontrollera om alla tasks är klara (sista sceneTask)
        if (onTask == sceneTasks.Count && sceneTasks[onTask - 1].GetComponent<TaskTracker>().allTasksComplete)
        {
            OnAllTasksComplete();  // Anropa funktion när alla tasks är klara
        }
    }

    // Metod för att aktivera gettingDarkText
    private void ActivateGettingDarkText()
    {
        if (gettingDarkText != null)
        {
            gettingDarkText.gameObject.SetActive(true); // Aktivera texten

            // Om du vill dölja texten efter en viss tid
            StartCoroutine(HideGettingDarkTextAfterDelay(5f)); // Dölja efter 5 sekunder
        }
    }

    private void ActivateShiftInstruction()
    {
        Debug.Log("Activating shiftToRun text.");
        if (shiftToRun != null)
        {
            shiftToRun.gameObject.SetActive(true); // Aktivera texten

            // Om du vill dölja texten efter en viss tid
            StartCoroutine(HideShiftTorunAfterDelay(5f)); // Dölja efter 5 sekunder
        }
    }

    private IEnumerator HideShiftTorunAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (shiftToRun != null)
        {
            shiftToRun.gameObject.SetActive(false); // Dölja texten
        }
    }

    // Korutin för att dölja gettingDarkText efter en viss tid
    private IEnumerator HideGettingDarkTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (gettingDarkText != null)
        {
            gettingDarkText.gameObject.SetActive(false); // Dölja texten
        }
    }

    // Metod för att aktivera nästa uppgift
    private void ActivateNextTask(int taskIndex)
    {
        if (taskIndex < sceneTasks.Count)
        {
            // Aktivera ActiveZone för nästa task
            ActivateAllActiveZones(sceneTasks[taskIndex].transform, "EffectActiveZone");

            // Försök att hitta och aktivera "RemoveDirtGrave" skript om de finns
            RemoveDirtGrave[] cleanStones = sceneTasks[taskIndex].GetComponentsInChildren<RemoveDirtGrave>();
            if (cleanStones.Length > 0)  // Kontrollera om cleanStones hittades
            {
                foreach (RemoveDirtGrave stones in cleanStones)
                {
                    stones.enabled = true;
                }
            }
            else
            {
                Debug.Log("No RemoveDirtGrave components found in sceneTask[" + taskIndex + "]");
            }

            // Aktivera "RemoveWeed" skript på kameran om det finns
            var cameraObject = Camera.main;
            if (cameraObject.TryGetComponent<RemoveWeed>(out var weedRemovalScript))
            {
                weedRemovalScript.enabled = true;
            }
            else
            {
                Debug.LogError("RemoveWeed script not found on the main camera!");
            }
        }
    }

    // Ny metod för att aktivera alla ActiveZone
    private void ActivateAllActiveZones(Transform parent, string objectName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == objectName)
            {
                child.gameObject.SetActive(true);
                Debug.Log("Activated: " + child.name);
            }

            // Rekursivt aktivera barnobjekt
            ActivateAllActiveZones(child, objectName);
        }
    }

    // Funktion som körs när alla tasks är klara
    private void OnAllTasksComplete()
    {
        Debug.Log("All tasks are complete!");

        // Starta koroutinen för att vänta innan scenen byts
        StartCoroutine(WaitBeforeSceneChange());
    }

    // Korutin för att vänta innan scenen byts
    private IEnumerator WaitBeforeSceneChange()
    {
        yield return new WaitForSeconds(3); // Vänta den angivna tiden

        levelLoader.LoadNextLevel();  // Byt scen
    }
}

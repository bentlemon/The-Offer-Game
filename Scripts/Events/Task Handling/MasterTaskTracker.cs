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
    [SerializeField] public int afterWhichTaskElement; // Index f�r tasken efter vilken texten ska aktiveras
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

        // D�lj TotalTaskUI initialt
        if (totalTaskUI != null)
        {
            totalTaskUI.SetActive(false);
        }

        // D�lj gettingDarkText initialt
        if (gettingDarkText != null)
        {
            gettingDarkText.gameObject.SetActive(false); // D�lja texten i b�rjan
        }

    }

    void Update()
    {
        // Bevara logiken f�r sceneTask[0]
        if (sceneTasks[0].GetComponent<TaskTracker>().allTasksComplete && !graveTasksActive)
        {
            onTask = 2;
            graveTasksActive = true;

            // Aktivera TotalTaskUI
            if (totalTaskUI != null)
            {
                totalTaskUI.SetActive(true); // Aktivera UI n�r alla uppgifter �r klara
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

            // Aktivera ActiveZones f�r sceneTasks[1]
            ActivateAllActiveZones(sceneTasks[1].transform, "EffectActiveZone");

            // Aktivera skript f�r RemoveDirtGrave i sceneTasks[1]
            RemoveDirtGrave[] cleanStones = sceneTasks[1].GetComponentsInChildren<RemoveDirtGrave>();
            foreach (RemoveDirtGrave stones in cleanStones)
            {
                if (stones != null)
                {
                    stones.enabled = true;
                }
            }

            // Aktivera RemoveWeed p� kameran
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

        // Sekventiell aktivering f�r resten av sceneTasks (fr�n sceneTask[1] och upp�t)
        if (onTask > 1 && onTask < sceneTasks.Count)
        {
            if (sceneTasks[onTask - 1].GetComponent<TaskTracker>().allTasksComplete)
            {
                // Deaktivera f�reg�ende task's EffectActiveZone
                var previousEffectActiveZone = sceneTasks[onTask - 1].transform.Find("EffectActiveZone");
                if (previousEffectActiveZone != null)
                {
                    previousEffectActiveZone.gameObject.SetActive(false);
                }

                ActivateNextTask(onTask);
                onTask++;  // �ka onTask till n�sta

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

        // Kontrollera om alla tasks �r klara (sista sceneTask)
        if (onTask == sceneTasks.Count && sceneTasks[onTask - 1].GetComponent<TaskTracker>().allTasksComplete)
        {
            OnAllTasksComplete();  // Anropa funktion n�r alla tasks �r klara
        }
    }

    // Metod f�r att aktivera gettingDarkText
    private void ActivateGettingDarkText()
    {
        if (gettingDarkText != null)
        {
            gettingDarkText.gameObject.SetActive(true); // Aktivera texten

            // Om du vill d�lja texten efter en viss tid
            StartCoroutine(HideGettingDarkTextAfterDelay(5f)); // D�lja efter 5 sekunder
        }
    }

    private void ActivateShiftInstruction()
    {
        Debug.Log("Activating shiftToRun text.");
        if (shiftToRun != null)
        {
            shiftToRun.gameObject.SetActive(true); // Aktivera texten

            // Om du vill d�lja texten efter en viss tid
            StartCoroutine(HideShiftTorunAfterDelay(5f)); // D�lja efter 5 sekunder
        }
    }

    private IEnumerator HideShiftTorunAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (shiftToRun != null)
        {
            shiftToRun.gameObject.SetActive(false); // D�lja texten
        }
    }

    // Korutin f�r att d�lja gettingDarkText efter en viss tid
    private IEnumerator HideGettingDarkTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (gettingDarkText != null)
        {
            gettingDarkText.gameObject.SetActive(false); // D�lja texten
        }
    }

    // Metod f�r att aktivera n�sta uppgift
    private void ActivateNextTask(int taskIndex)
    {
        if (taskIndex < sceneTasks.Count)
        {
            // Aktivera ActiveZone f�r n�sta task
            ActivateAllActiveZones(sceneTasks[taskIndex].transform, "EffectActiveZone");

            // F�rs�k att hitta och aktivera "RemoveDirtGrave" skript om de finns
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

            // Aktivera "RemoveWeed" skript p� kameran om det finns
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

    // Ny metod f�r att aktivera alla ActiveZone
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

    // Funktion som k�rs n�r alla tasks �r klara
    private void OnAllTasksComplete()
    {
        Debug.Log("All tasks are complete!");

        // Starta koroutinen f�r att v�nta innan scenen byts
        StartCoroutine(WaitBeforeSceneChange());
    }

    // Korutin f�r att v�nta innan scenen byts
    private IEnumerator WaitBeforeSceneChange()
    {
        yield return new WaitForSeconds(3); // V�nta den angivna tiden

        levelLoader.LoadNextLevel();  // Byt scen
    }
}

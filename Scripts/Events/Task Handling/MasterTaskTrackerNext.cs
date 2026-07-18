using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MasterTaskTrackerNight : MonoBehaviour
{
    [Header("Chronological Order of Tasks")]
    [SerializeField] public List<GameObject> sceneTasks;

    [Header("Other stuff")]
    [SerializeField] public TMP_Text informationText;
    [SerializeField] private GameObject totalTaskUI;
    public TMP_Text getOutText;

    [Header("Effect zone for secound task")]
    [SerializeField] private GameObject effectZoneTaskOne;   // Referens for EffectActiveZone
    [SerializeField] public GameObject ExitZone;             // Referens for EffectActiveZone
    [SerializeField] public GameObject ExitZoneOut;          // Referens for EffectActiveZone

    private GameObject firstAngel;
    private GameObject secoundAngel;
    private TaskTracker taskTracker;
    private int onTask = 0; // Debug

    void Start()
    {
        // Hide TotalTaskUI initial
        if (totalTaskUI != null)
        {
            totalTaskUI.SetActive(false);
        }

        // Hide  gettingDarkText Init
        if (informationText != null)
        {
            informationText.gameObject.SetActive(false);
        }

        // Get reference and store it in inactive obj
        Transform childTransform = sceneTasks[1].transform.Find("GetKeyTask/Kneeling_angel_gotkey");
        if (childTransform != null)
        {
            secoundAngel = childTransform.gameObject;
        }
        else
        {
            Debug.LogWarning("GetKeyTask obj returned null");
        }

        firstAngel = GameObject.Find("Kneeling_angel");
        if (firstAngel == null)
        {
            Debug.LogWarning("firstAngel not found in the scene!");
        }
    }

    void Update()
    {
        //Debug.Log("Aktuellt onTask: " + onTask);
        //Debug.Log("All Tasks Complete: " + sceneTasks[onTask].GetComponent<TaskTracker>().allTasksComplete);

        // Activate TotalTaskUI
        if (totalTaskUI != null && !sceneTasks[1].GetComponent<TaskTracker>().allTasksComplete)
        {
            totalTaskUI.SetActive(true);
        }

        // Check that every subtask is done
        if (sceneTasks[onTask].GetComponent<TaskTracker>().allTasksComplete)
        {
            // Deactivate last task's EffectActiveZone
            var previousEffectActiveZone = sceneTasks[onTask].transform.Find("EffectActiveZone");
            if (previousEffectActiveZone != null)
            {
                previousEffectActiveZone.gameObject.SetActive(false);
            }

            if(onTask == 0)
            {
                onTask++;
            }
        }

        if (onTask == 1 && !sceneTasks[1].GetComponent<TaskTracker>().allTasksComplete)
        {
            // Activate the specifik EffectActiveZone for the next task
            if (effectZoneTaskOne != null)
            {
                effectZoneTaskOne.SetActive(true); // Activate zone
                Debug.Log("EffectActiveZone har aktiverats.");
            }

            totalTaskUI.SetActive(false);
            firstAngel.SetActive(false);

            // Activate secondAngel if it's inactive
            if (secoundAngel != null && !secoundAngel.activeSelf)
            {
                secoundAngel.SetActive(true);
            }
        }

        if (sceneTasks[1].GetComponent<TaskTracker>().allTasksComplete)
        {
            ExitZone.SetActive(false); 
            ExitZoneOut.SetActive(true);
        }
    }

    private void ActivateInformationText()
    {
        if (informationText != null)
        {
            Debug.Log("Im trying to display the text");
            informationText.gameObject.SetActive(true); // Activate text
            StartCoroutine(HideInformationTextAfterDelay(6f)); // Hide text after 5 sec
        }
    }

    private IEnumerator HideInformationTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (informationText != null)
        {
            informationText.gameObject.SetActive(false); // Hide text
        }
    }
}

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
    [SerializeField] private GameObject totalTaskUI; // UI-elementet som ska aktiveras
    public TMP_Text getOutText;

    [Header("Effect zone for secound task")]
    [SerializeField] private GameObject effectZoneTaskOne; // Referens till EffectActiveZone
    [SerializeField] public GameObject ExitZone; // Referens till EffectActiveZone
    [SerializeField] public GameObject ExitZoneOut; // Referens till EffectActiveZone

    private GameObject firstAngel;
    private GameObject secoundAngel;
    private TaskTracker taskTracker;
    private int onTask = 0; // Debug

    void Start()
    {
        // D�lj TotalTaskUI initialt
        if (totalTaskUI != null)
        {
            totalTaskUI.SetActive(false);
        }

        // D�lj gettingDarkText initialt
        if (informationText != null)
        {
            informationText.gameObject.SetActive(false); // D�lja texten i b�rjan
        }

        // H�mta och lagra referens till det inaktiva objektet
        Transform childTransform = sceneTasks[1].transform.Find("GetKeyTask/Kneeling_angel_gotkey");
        if (childTransform != null)
        {
            secoundAngel = childTransform.gameObject;
        }
        else
        {
            Debug.LogWarning("Barnbarnsobjekt med namnet 'GetKeyTask/Kneeling_angel_gotkey' hittades inte i sceneTask[1].");
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

        // Aktivera TotalTaskUI
        if (totalTaskUI != null && !sceneTasks[1].GetComponent<TaskTracker>().allTasksComplete)
        {
            totalTaskUI.SetActive(true);
        }

        // Kontrollera om alla deluppgifter i den aktuella task �r klara
        if (sceneTasks[onTask].GetComponent<TaskTracker>().allTasksComplete)
        {
            // Deaktivera f�reg�ende task's EffectActiveZone
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
            // Aktivera den specifika EffectActiveZone f�r den andra uppgiften
            if (effectZoneTaskOne != null)
            {
                effectZoneTaskOne.SetActive(true); // Aktivera zonen
                Debug.Log("EffectActiveZone har aktiverats.");
            }

            // G�m UI-element och aktivera specifika objekt
            totalTaskUI.SetActive(false);
            firstAngel.SetActive(false);

            // Aktivera secoundAngel om den �r inaktiv
            if (secoundAngel != null && !secoundAngel.activeSelf)
            {
                secoundAngel.SetActive(true);
            }
        }

        if (sceneTasks[1].GetComponent<TaskTracker>().allTasksComplete)
        {
            ExitZone.SetActive(false); 
            ExitZoneOut.SetActive(true);
            // Trigga text om att ta sig ut
            //ActivateInformationText();
        }
    }

    private void ActivateInformationText()
    {
        if (informationText != null)
        {
            Debug.Log("Im trying to display the text");
            informationText.gameObject.SetActive(true); // Aktivera texten
            StartCoroutine(HideInformationTextAfterDelay(6f)); // D�lja efter 5 sekunder
        }
    }

    private IEnumerator HideInformationTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (informationText != null)
        {
            informationText.gameObject.SetActive(false); // D�lja texten
        }
    }
}

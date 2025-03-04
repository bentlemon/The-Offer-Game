using UnityEngine;

public class SubtaskTracker : MonoBehaviour
{
    public bool allTasksComplete = false;
    public AudioClip completionClip;
    [Range(0f, 1f)] public float volume = 1f; // L�gger till volymkontroll
    private AudioSource audioSource;
    private bool hasPlayedSound = false;

    private void Start()
    {
        // Skapa en AudioSource-komponent om den inte redan finns
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = completionClip;
        audioSource.playOnAwake = false;
        audioSource.volume = volume; // S�tt initial volym
    }

    private void Update()
    {
        allTasksComplete = true;

        foreach (Transform child in transform)
        {
            SubtaskHandler subtaskHandler = child.GetComponent<SubtaskHandler>();
            if (subtaskHandler == null)
            {
                continue;
            }
            else
            {
                if (!subtaskHandler.taskComp)
                {
                    allTasksComplete = false;
                    break;
                }
            }
        }

        if (allTasksComplete && !hasPlayedSound)
        {
            Transform taskHighlight = transform.Find("EffectActiveZone");
            taskHighlight.gameObject.SetActive(false);

            // Spela upp ljudklippet och s�tt flaggan s� att det inte spelas igen
            if (completionClip != null)
            {
                audioSource.volume = volume; // S�tt volymen innan ljudet spelas
                audioSource.Play();
                hasPlayedSound = true;
            }
        }
    }
}

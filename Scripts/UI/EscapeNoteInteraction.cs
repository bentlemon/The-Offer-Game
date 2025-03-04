using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeNoteInteraction : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] public LayerMask ignoredLayerMask;
    [SerializeField] public KeyCode interactionKeyShow = KeyCode.E;
    [SerializeField] public KeyCode interactionKeyHide = KeyCode.Q;

    [Space]
    [Header("UI Text")]
    [SerializeField] public TMP_Text eText;  // Text som visar "Tryck E f�r att interagera"
    [SerializeField] public TMP_Text qText;  // Text f�r att visa meddelandet
    [SerializeField] public TMP_Text spaceText;
    [Space]

    [SerializeField] private Canvas noteCanvas; // Referens till canvasen som inneh�ller notisen
    [SerializeField] private BoxCollider interactionArea;
    [Space]

    [Header("Audio Settings")]
    [SerializeField] private AudioSource soundSource; // Referens till AudioSource

    private GameObject[] noteObjects;       // Array f�r att lagra flera PickUp-objekt
    private bool noteVisible = false;       // H�lla koll p� om notisen �r synlig
    private Outline currentOutline;         // F�r att h�lla koll p� nuvarande outline
    private LevelLoader levelLoader;

    [SerializeField] private float fadeDuration = 100f; // Tiden det tar att fadear ut ljudet

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        noteObjects = GameObject.FindGameObjectsWithTag("Note");

        if (noteObjects.Length == 0)
        {
            Debug.LogError("Inga notobjekt hittades!");
            return;
        }

        // Kontrollera om soundSource �r null
        if (soundSource == null)
        {
            Debug.LogError("AudioSource saknas! Kontrollera att ljudobjektet har en AudioSource-komponent.");
        }

        eText.gameObject.SetActive(false); // Starta med eText g�md
    }

    void Update()
    {
        bool isInInteractionArea = interactionArea != null && interactionArea.bounds.Contains(Camera.main.transform.position);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Kontrollera om rayen tr�ffar n�got
        if (Physics.Raycast(mouseRay, out hit, 50f))
        {
            bool hitNote = false; // Flagga f�r att kontrollera om vi tr�ffade en not
            foreach (GameObject note in noteObjects)
            {
                if (hit.collider.gameObject == note)
                {
                    if (!noteVisible)
                    {
                        eText.gameObject.SetActive(true); // Visa interaktionstext
                    }
                    hitNote = true; // Markera att vi tr�ffade en not
                    Debug.Log("Tr�ffade objekt: " + hit.collider.gameObject.name);

                    if (Input.GetKeyDown(interactionKeyShow) && !noteVisible)
                    {
                        eText.gameObject.SetActive(false); // D�lja interaktionstext
                        ShowNote();
                    }

                    // Aktivera outline om noten tr�ffas
                    if (hit.collider.TryGetComponent<Outline>(out var outline))
                    {
                        if (currentOutline != null && currentOutline != outline)
                        {
                            currentOutline.enabled = false; // Inaktivera f�reg�ende outline
                        }

                        currentOutline = outline;
                        currentOutline.enabled = true; // Aktivera nuvarande outline
                    }

                    break; // Avbryt loop eftersom vi hittade en not
                }
            }

            // D�lja interaktionstext och inaktivera outline om vi inte tr�ffar n�gon not
            if (!hitNote)
            {
                eText.gameObject.SetActive(false);

                if (currentOutline != null)
                {
                    currentOutline.enabled = false; // Inaktivera outline
                    currentOutline = null;
                }
            }
        }
        else
        {
            eText.gameObject.SetActive(false); // D�lja interaktionstext om vi inte tr�ffar n�gon not

            if (currentOutline != null)
            {
                currentOutline.enabled = false; // Inaktivera outline
                currentOutline = null;
            }
        }

        // Om noten �r synlig och spelaren �r utanf�r interaktionsomr�det, d�lja noten
        if (noteVisible && !isInInteractionArea)
        {
            HideNote();
        }

        // Till�t spelaren att d�lja noten med Q-tangenten
        if (noteVisible && Input.GetKeyDown(interactionKeyHide))
        {
            HideNote();
            StartCoroutine(PlaySoundAndLoadNextScene());
        }
    }

    void ShowNote()
    {
        spaceText.gameObject.SetActive(true);
        qText.gameObject.SetActive(true);
        noteCanvas.gameObject.SetActive(true); // Aktivera notcanvas
        noteVisible = true;

        // Inaktivera outline n�r noten visas
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }
    }

    void HideNote()
    {
        spaceText.gameObject.SetActive(false);
        qText.gameObject.SetActive(false);
        noteCanvas.gameObject.SetActive(false); // Inaktivera notcanvas
        noteVisible = false;
        eText.gameObject.SetActive(false); // Se till att eText �r g�md n�r noten d�ljs

        // Se till att outline �r inaktiverad n�r noten d�ljs
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }
    }

    private IEnumerator PlaySoundAndLoadNextScene()
    {
        // Spela ljudet och v�nta en stund
        if (soundSource != null)
        {
            yield return new WaitForSeconds(3f);
            soundSource.Play(); // Spela ljudet
            yield return FadeOutSound(fadeDuration); // Fada ut ljudet
        }
    }

    private IEnumerator FadeOutSound(float fadeDuration)
    {
        if (soundSource == null)
        {
            Debug.LogError("AudioSource saknas!");
            yield break; // Avbryt om AudioSource inte finns
        }

        float startVolume = soundSource.volume; // Spara startvolym
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime; // �ka tiden
            soundSource.volume = Mathf.Lerp(startVolume, 0, time / fadeDuration); // Lerp till volym 0
            yield return null; // V�nta en frame
        }

        soundSource.volume = 0; // S�kerst�ll att volymen �r 0 efter fade
        soundSource.Stop(); // Stoppa ljudet
        levelLoader.LoadNextLevel();
    }
}

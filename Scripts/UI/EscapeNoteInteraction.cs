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
    [SerializeField] public TMP_Text eText;  // Text som visar "Tryck E för att interagera"
    [SerializeField] public TMP_Text qText;  // Text för att visa meddelandet
    [SerializeField] public TMP_Text spaceText;
    [Space]

    [SerializeField] private Canvas noteCanvas; // Referens till canvasen som innehåller notisen
    [SerializeField] private BoxCollider interactionArea;
    [Space]

    [Header("Audio Settings")]
    [SerializeField] private AudioSource soundSource; // Referens till AudioSource

    private GameObject[] noteObjects;       // Array för att lagra flera PickUp-objekt
    private bool noteVisible = false;       // Hålla koll på om notisen är synlig
    private Outline currentOutline;         // För att hålla koll på nuvarande outline
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

        // Kontrollera om soundSource är null
        if (soundSource == null)
        {
            Debug.LogError("AudioSource saknas! Kontrollera att ljudobjektet har en AudioSource-komponent.");
        }

        eText.gameObject.SetActive(false); // Starta med eText gömd
    }

    void Update()
    {
        bool isInInteractionArea = interactionArea != null && interactionArea.bounds.Contains(Camera.main.transform.position);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Kontrollera om rayen träffar något
        if (Physics.Raycast(mouseRay, out hit, 50f))
        {
            bool hitNote = false; // Flagga för att kontrollera om vi träffade en not
            foreach (GameObject note in noteObjects)
            {
                if (hit.collider.gameObject == note)
                {
                    if (!noteVisible)
                    {
                        eText.gameObject.SetActive(true); // Visa interaktionstext
                    }
                    hitNote = true; // Markera att vi träffade en not
                    Debug.Log("Träffade objekt: " + hit.collider.gameObject.name);

                    if (Input.GetKeyDown(interactionKeyShow) && !noteVisible)
                    {
                        eText.gameObject.SetActive(false); // Dölja interaktionstext
                        ShowNote();
                    }

                    // Aktivera outline om noten träffas
                    if (hit.collider.TryGetComponent<Outline>(out var outline))
                    {
                        if (currentOutline != null && currentOutline != outline)
                        {
                            currentOutline.enabled = false; // Inaktivera föregående outline
                        }

                        currentOutline = outline;
                        currentOutline.enabled = true; // Aktivera nuvarande outline
                    }

                    break; // Avbryt loop eftersom vi hittade en not
                }
            }

            // Dölja interaktionstext och inaktivera outline om vi inte träffar någon not
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
            eText.gameObject.SetActive(false); // Dölja interaktionstext om vi inte träffar någon not

            if (currentOutline != null)
            {
                currentOutline.enabled = false; // Inaktivera outline
                currentOutline = null;
            }
        }

        // Om noten är synlig och spelaren är utanför interaktionsområdet, dölja noten
        if (noteVisible && !isInInteractionArea)
        {
            HideNote();
        }

        // Tillåt spelaren att dölja noten med Q-tangenten
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

        // Inaktivera outline när noten visas
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
        eText.gameObject.SetActive(false); // Se till att eText är gömd när noten döljs

        // Se till att outline är inaktiverad när noten döljs
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }
    }

    private IEnumerator PlaySoundAndLoadNextScene()
    {
        // Spela ljudet och vänta en stund
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
            time += Time.deltaTime; // Öka tiden
            soundSource.volume = Mathf.Lerp(startVolume, 0, time / fadeDuration); // Lerp till volym 0
            yield return null; // Vänta en frame
        }

        soundSource.volume = 0; // Säkerställ att volymen är 0 efter fade
        soundSource.Stop(); // Stoppa ljudet
        levelLoader.LoadNextLevel();
    }
}

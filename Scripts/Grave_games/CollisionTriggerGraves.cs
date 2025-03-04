using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTriggerGraves : MonoBehaviour
{
    public float interactionDistance = 5f; // Avståndet inom vilket spelaren kan interagera
    public KeyCode interactionKeyEnter = KeyCode.E; // Knappen för att aktivera zonen
    public KeyCode interactionKeyLeave = KeyCode.Q; // Knappen för att aktivera zonen
    public LayerMask ignoredLayerMask;

    private Animator animator;

    private Transform playerCamera; // Håller referens till spelarens kamera
    public Camera staticCamera; // Referens till den statiska kameran
    public Camera mainCamera; // Referens till huvudkameran

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerCamera = Camera.main.transform; // Hämtar spelarens huvudkamera
        mainCamera = Camera.main; // Spara referens till huvudkameran
        staticCamera.enabled = false; // Se till att den statiska kameran är avstängd från början
    }

    private void Update()
    {
        Ray ray = new(playerCamera.position, playerCamera.forward); // Skapar en raycast från spelarens kamera
        RaycastHit hit;

        // Om raycast träffar zonens Collider och är inom ett specifikt avstånd
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject) // Kontrollera om vi träffar denna zon
            {
                // Lägg till kod här som visar vilken knapp man ska trycka på 
                // Om spelaren trycker på interaktionsknappen
                if (Input.GetKeyDown(interactionKeyEnter))
                {
                    Activate();
                }
                if (Input.GetKeyDown(interactionKeyLeave))
                {
                    ResetCamera();
                }
            }
        }

        // Draw ray
        if (staticCamera.enabled)
        {
            // Skapa ray från kameran baserat på muspositionen
            Ray mouseRay = staticCamera.ScreenPointToRay(Input.mousePosition);

            // Använd Physics.RaycastAll för att få alla träffar
            RaycastHit[] hits = Physics.RaycastAll(mouseRay, 100f); // 100f är rayens räckvidd
            Debug.DrawRay(mouseRay.origin, mouseRay.direction * 100f, Color.cyan);


            // Logga alla träffar
            foreach (RaycastHit h in hits)
            {
                // Filtrera bort triggerzonen
                if (h.collider.gameObject.layer == LayerMask.NameToLayer(ignoredLayerMask.ToString()))
                {
                    continue; // Hoppa över objekt på ignoreLayer
                }

                Debug.Log("Träffade: " + h.collider.gameObject.name + " på lager: " + LayerMask.LayerToName(h.collider.gameObject.layer));
                // Här kan du lägga till din logik för att hantera träffar
            }

        }

        }

    // Funktion som aktiverar zonen
    private void Activate()
    {
        Debug.Log("Zonen aktiverades!");

        // Byt kamera
        mainCamera.enabled = false;  // Stäng av huvudkameran
        staticCamera.enabled = true; // Aktivera den statiska kameran
        Cursor.lockState = CursorLockMode.None;

    }

    // Funktion som återställer till huvudkameran
    private void ResetCamera()
    {
        staticCamera.enabled = false;  // Stäng av den statiska kameran
        mainCamera.enabled = true;     // Aktivera huvudkameran igen
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void RemoveWeeds()
    {

    }

    public void CleanStone()
    {

    }
}
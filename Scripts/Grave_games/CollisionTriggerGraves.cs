using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTriggerGraves : MonoBehaviour
{
    public float interactionDistance = 5f; // Avst�ndet inom vilket spelaren kan interagera
    public KeyCode interactionKeyEnter = KeyCode.E; // Knappen f�r att aktivera zonen
    public KeyCode interactionKeyLeave = KeyCode.Q; // Knappen f�r att aktivera zonen
    public LayerMask ignoredLayerMask;

    private Animator animator;

    private Transform playerCamera; // H�ller referens till spelarens kamera
    public Camera staticCamera; // Referens till den statiska kameran
    public Camera mainCamera; // Referens till huvudkameran

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerCamera = Camera.main.transform; // H�mtar spelarens huvudkamera
        mainCamera = Camera.main; // Spara referens till huvudkameran
        staticCamera.enabled = false; // Se till att den statiska kameran �r avst�ngd fr�n b�rjan
    }

    private void Update()
    {
        Ray ray = new(playerCamera.position, playerCamera.forward); // Skapar en raycast fr�n spelarens kamera
        RaycastHit hit;

        // Om raycast tr�ffar zonens Collider och �r inom ett specifikt avst�nd
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject) // Kontrollera om vi tr�ffar denna zon
            {
                // L�gg till kod h�r som visar vilken knapp man ska trycka p� 
                // Om spelaren trycker p� interaktionsknappen
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
            // Skapa ray fr�n kameran baserat p� muspositionen
            Ray mouseRay = staticCamera.ScreenPointToRay(Input.mousePosition);

            // Anv�nd Physics.RaycastAll f�r att f� alla tr�ffar
            RaycastHit[] hits = Physics.RaycastAll(mouseRay, 100f); // 100f �r rayens r�ckvidd
            Debug.DrawRay(mouseRay.origin, mouseRay.direction * 100f, Color.cyan);


            // Logga alla tr�ffar
            foreach (RaycastHit h in hits)
            {
                // Filtrera bort triggerzonen
                if (h.collider.gameObject.layer == LayerMask.NameToLayer(ignoredLayerMask.ToString()))
                {
                    continue; // Hoppa �ver objekt p� ignoreLayer
                }

                Debug.Log("Tr�ffade: " + h.collider.gameObject.name + " p� lager: " + LayerMask.LayerToName(h.collider.gameObject.layer));
                // H�r kan du l�gga till din logik f�r att hantera tr�ffar
            }

        }

        }

    // Funktion som aktiverar zonen
    private void Activate()
    {
        Debug.Log("Zonen aktiverades!");

        // Byt kamera
        mainCamera.enabled = false;  // St�ng av huvudkameran
        staticCamera.enabled = true; // Aktivera den statiska kameran
        Cursor.lockState = CursorLockMode.None;

    }

    // Funktion som �terst�ller till huvudkameran
    private void ResetCamera()
    {
        staticCamera.enabled = false;  // St�ng av den statiska kameran
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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;

public class RemoveWeed : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] public Camera mainCamera;
    [SerializeField] public LayerMask ignoredLayerMask;
    [SerializeField] public KeyCode interactionKey = KeyCode.E;

    [Header("Ui text")]
    [SerializeField] public TMP_Text eText;

    // ---
    private GameObject lastLookedAtObject;
    private SubtaskHandler subtaskHandler;

    private void Awake()
    {
        subtaskHandler = gameObject.GetComponent<SubtaskHandler>();
    }

    void Update()
    {
        Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(mouseRay, out hit, 10f);

        if (hitSomething)
        {
            Debug.DrawRay(mouseRay.origin, mouseRay.direction * 100f, Color.cyan);

            // filter ignoredLayerMask
            if (((1 << hit.collider.gameObject.layer) & ignoredLayerMask) == 0)
            {
                // Looking at a new obj
                if (lastLookedAtObject != hit.collider.gameObject)
                {
                    DisableLastLookedAtObject();
                    lastLookedAtObject = hit.collider.gameObject;

                    if (lastLookedAtObject.TryGetComponent<Outline>(out var newScript))
                    {
                        eText.gameObject.SetActive(true);
                        newScript.enabled = true; 
                        //Debug.Log("Aktiverade skriptet på: " + lastLookedAtObject.name);
                    }

                }
            }
        }
        else
        {
            DisableLastLookedAtObject();
        }

        if (Input.GetKeyDown(interactionKey) && lastLookedAtObject != null && lastLookedAtObject.CompareTag("Weeds"))
        {
            var meshRenderer = lastLookedAtObject.GetComponent<MeshRenderer>();
            var collider = lastLookedAtObject.GetComponent<Collider>();
            subtaskHandler = lastLookedAtObject.GetComponent<SubtaskHandler>();

            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }

            if (collider != null)
            {
                collider.enabled = false;
            }

            subtaskHandler.taskComp = true;  // Setting that the task for this weed is done
            eText.gameObject.SetActive(false);

        }
    }

    void DisableLastLookedAtObject()
    {
        if (lastLookedAtObject != null)
        {
            var oldScript = lastLookedAtObject.GetComponent<Outline>();
            if (oldScript != null)
            {
                oldScript.enabled = false;
                eText.gameObject.SetActive(false);
            }

            lastLookedAtObject = null;
        }
    }

}

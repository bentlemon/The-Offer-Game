using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Object tracking")]
    [SerializeField] public KeyCode interactionKey = KeyCode.E;
    [SerializeField] public GameObject keyUnderBrick;
    [SerializeField] public GameObject triggerDoor;
    [SerializeField] public GameObject doorL;
    [SerializeField] public GameObject doorR;

    [SerializeField]
    [Header("Ui Text")]
    public TMP_Text eText;

    [Header("Animators")]
    private Animator leftDoorAnimator;
    private Animator rightDoorAnimator;

    // ---
    private SubtaskHandler subtaskHandler;
    private bool doorsOpened = false;
    private bool keyObtained;

    private void Awake()
    {
        leftDoorAnimator = doorL.GetComponent<Animator>();
        rightDoorAnimator = doorR.GetComponent<Animator>();
        subtaskHandler = gameObject.GetComponent<SubtaskHandler>();
    }

    void Update()
    {
        keyObtained = keyUnderBrick.GetComponent<SubtaskHandler>().taskComp;

        if (keyObtained)
        {
            // Turn off the blockage zone
            triggerDoor.SetActive(false);
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (!doorsOpened)
                    {
                        if (hit.collider.gameObject == doorL || hit.collider.gameObject == doorR)
                        {
                            eText.gameObject.SetActive(true);

                            if (Input.GetKeyDown(interactionKey))
                            {
                                subtaskHandler.taskComp = true;
                                OpenDoors();
                            }
                        }
                        else
                        {
                            eText.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        eText.gameObject.SetActive(false);
                    }
                }
                else
                {
                    eText.gameObject.SetActive(false);
                }
            }
        }
    }

    void OpenDoors()
    {
        leftDoorAnimator.SetTrigger("Open");
        rightDoorAnimator.SetTrigger("Open");

        doorsOpened = true;
        eText.gameObject.SetActive(false);
    }
}

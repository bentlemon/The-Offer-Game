using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePageSide : MonoBehaviour
{
    [SerializeField] public GameObject frontPage;
    [SerializeField] public GameObject backPage;
    [SerializeField] public KeyCode interactionKeyRotate = KeyCode.Space;

    private bool frontCheck = true;

    void Start()
    {
        backPage.SetActive(false);
        frontPage.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(interactionKeyRotate))
        {
            if (frontCheck)
            {
                backPage.SetActive(true);
                frontPage.SetActive(false);
            }
            else
            {
                frontPage.SetActive(true);
                backPage.SetActive(false);
            }

            frontCheck = !frontCheck; // Toggle frontCheck after each switch
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactedOnce : MonoBehaviour
{
    private SubtaskHandler subtaskHandler;
    private InteractiveObject interactiveObject;
    bool interactedFlag = false;

    void Start()
    {
        subtaskHandler = gameObject.GetComponent<SubtaskHandler>();
    }

    void Update()
    {
        
    }
}

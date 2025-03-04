using System.Collections.Generic;
using UnityEngine;

/* Simple camera manager to handle 
    all cameras in scenes */

public class CameraManager : MonoBehaviour
{
    /* OBS!! Main camera needs to be element 0!!*/
    public GameObject Player;
    public List<Camera> allCameras;

    public void Awake()
    {
        // Debug code
        /*int count = allCameras.Count;
        Debug.Log("We've got " + count + " cameras");

        for (int i = 0; i < allCameras.Count; i++)
        {
            string cameraState = allCameras[i].enabled ? "Active" : "Inactive";
            Debug.Log("Camera " + (i + 1) + ": " + allCameras[i].name + " - " + cameraState);
        }
        */
    }

    public void SetActiveCamera(Camera activeCamera)
    {
        foreach (Camera cam in allCameras)
        {
            cam.enabled = false;
        }

        if (activeCamera != allCameras[0])
        {
            Player.SetActive(false);
        }
        activeCamera.enabled = true;
        //Awake();
    }

    public void deactivateSubCamera()
    {
        foreach (Camera cam in allCameras)
        {
            cam.enabled = false;
        }

        allCameras[0].enabled = true; // main camera back !!!!
        Player.SetActive(true);
    }
}



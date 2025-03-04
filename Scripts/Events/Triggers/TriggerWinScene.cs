using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWinScene : MonoBehaviour
{
    private LevelLoader levelLoader;

    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    private void OnTriggerEnter(Collider other)
    {
            levelLoader.LoadNextChosenLevel("Outro_Scene_07"); // Outro level   
    }
}

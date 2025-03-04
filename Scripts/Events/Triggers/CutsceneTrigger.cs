using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    private LevelLoader levelLoader;
    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "CutSceneTrigger")
        levelLoader.LoadNextLevel();
    }
}

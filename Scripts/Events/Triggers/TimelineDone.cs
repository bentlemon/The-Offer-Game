using UnityEngine.Playables;
using UnityEngine;

public class TimelineDone : MonoBehaviour
{
    PlayableDirector director;
    LevelLoader levelLoader;

    bool playing = false;

    void Awake()
    {
        director = GameObject.Find("TImeline").GetComponent<PlayableDirector>();
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    void Update()
    {
        if (director.state != PlayState.Playing)
        {
            playing = false;
            levelLoader.LoadNextLevel();

        }
        else
        {
            playing = true;
        }
    }
}

using UnityEngine;

public class PlaySoundOnActivate : MonoBehaviour
{
    public GameObject soundObject; // GameObject som har AudioSource
    private AudioSource audioSource;

    void Start()
    {
        if (soundObject != null)
        {
            audioSource = soundObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogError("soundObject is not assigned!");
        }
    }


    // Getter för att hämta ljudkällan
    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    // Metod för att starta ljudet
    public void ActivateSound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play(); // Spela upp ljudet
        }
    }

    // Metod för att stänga av ljudet
    public void DeactivateSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // Stoppa ljudet
        }
    }
}

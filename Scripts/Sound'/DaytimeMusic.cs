using System.Collections;
using UnityEngine;

public class DelayedMusicFader : MonoBehaviour
{
    public AudioSource initialAudioSource;  // AudioSource f�r f�rsta ljudklippet
    public AudioSource secondaryAudioSource; // AudioSource f�r andra ljudklippet
    public float initialFadeInDuration = 2f;  // Fade-in tid f�r f�rsta klippet
    public float initialFadeOutDuration = 2f; // Fade-out tid f�r f�rsta klippet
    public float delayBeforeStart = 180f;     // F�rdr�jning innan andra klippet b�rjar
    public float secondaryFadeInDuration = 2f; // Fade-in tid f�r andra klippet

    private void Start()
    {
        initialAudioSource.volume = 0f;  // S�tt volymen p� f�rsta klippet till 0 fr�n start
        secondaryAudioSource.volume = 0f; // S�tt volymen p� andra klippet till 0 fr�n start

        float initialFadeOutStartTime = initialAudioSource.clip.length * 0.3f; // Ber�kna tidpunkt f�r att b�rja fade-out p� f�rsta klippet (30 % av l�ngden)

        StartCoroutine(PlayInitialClipWithFade(initialFadeOutStartTime)); // Starta f�rsta klippet med fade-in och fade-out
        Invoke("StartSecondaryClipFadeIn", delayBeforeStart);             // Starta andra klippet efter f�rdr�jning
    }

    private IEnumerator PlayInitialClipWithFade(float fadeOutStartTime)
    {
        initialAudioSource.Play();
        yield return StartCoroutine(FadeIn(initialAudioSource, initialFadeInDuration)); // Fade-in f�r f�rsta klippet
        yield return new WaitForSeconds(fadeOutStartTime - initialFadeInDuration); // V�nta tills fade-out-tid

        yield return StartCoroutine(FadeOut(initialAudioSource, initialFadeOutDuration)); // Fade-out f�r f�rsta klippet
    }

    private void StartSecondaryClipFadeIn()
    {
        secondaryAudioSource.Play();
        StartCoroutine(FadeIn(secondaryAudioSource, secondaryFadeInDuration)); // Fade-in f�r andra klippet
    }

    private IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float startVolume = 0f;
        while (audioSource.volume < 1f)
        {
            audioSource.volume += Time.deltaTime / duration;
            yield return null;
        }
        audioSource.volume = 1f;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime / duration;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume; // �terst�ll volymen om det beh�vs
    }
}

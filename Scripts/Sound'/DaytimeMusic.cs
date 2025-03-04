using System.Collections;
using UnityEngine;

public class DelayedMusicFader : MonoBehaviour
{
    public AudioSource initialAudioSource;  // AudioSource för första ljudklippet
    public AudioSource secondaryAudioSource; // AudioSource för andra ljudklippet
    public float initialFadeInDuration = 2f;  // Fade-in tid för första klippet
    public float initialFadeOutDuration = 2f; // Fade-out tid för första klippet
    public float delayBeforeStart = 180f;     // Fördröjning innan andra klippet börjar
    public float secondaryFadeInDuration = 2f; // Fade-in tid för andra klippet

    private void Start()
    {
        initialAudioSource.volume = 0f;  // Sätt volymen på första klippet till 0 från start
        secondaryAudioSource.volume = 0f; // Sätt volymen på andra klippet till 0 från start

        float initialFadeOutStartTime = initialAudioSource.clip.length * 0.3f; // Beräkna tidpunkt för att börja fade-out på första klippet (30 % av längden)

        StartCoroutine(PlayInitialClipWithFade(initialFadeOutStartTime)); // Starta första klippet med fade-in och fade-out
        Invoke("StartSecondaryClipFadeIn", delayBeforeStart);             // Starta andra klippet efter fördröjning
    }

    private IEnumerator PlayInitialClipWithFade(float fadeOutStartTime)
    {
        initialAudioSource.Play();
        yield return StartCoroutine(FadeIn(initialAudioSource, initialFadeInDuration)); // Fade-in för första klippet
        yield return new WaitForSeconds(fadeOutStartTime - initialFadeInDuration); // Vänta tills fade-out-tid

        yield return StartCoroutine(FadeOut(initialAudioSource, initialFadeOutDuration)); // Fade-out för första klippet
    }

    private void StartSecondaryClipFadeIn()
    {
        secondaryAudioSource.Play();
        StartCoroutine(FadeIn(secondaryAudioSource, secondaryFadeInDuration)); // Fade-in för andra klippet
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
        audioSource.volume = startVolume; // Återställ volymen om det behövs
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceManager : MonoBehaviour
{
    public AudioClip ScavnengerAmbience;
    public AudioClip flightDeckAmbience;
    public AudioClip runnerSoundTrack;
    public AudioSource audioSource;
    public float fadeDuration = 5f;
    public float maxVolume = 0.15f;

    private bool isFading = false;

    void Start()
    {
        audioSource.clip = ScavnengerAmbience;
        audioSource.volume = 0;
        audioSource.loop = true;
        audioSource.Play();

        StartCoroutine(FadeIn());
    }

    public void ChangeAmbience()
    {
        if (!isFading) StartCoroutine(ChangeAmbientClip());
    }

    public void Runner()
    {
        if (!isFading) StartCoroutine(TransitionToRunner());
    }

    private IEnumerator ChangeAmbientClip()
    {
        isFading = true;
        yield return StartCoroutine(FadeOut());

        yield return new WaitForSeconds(1f);
        audioSource.clip = flightDeckAmbience;
        audioSource.Play();

        yield return StartCoroutine(FadeIn());
        isFading = false;
    }

    private IEnumerator TransitionToRunner()
    {
        isFading = true;
        yield return StartCoroutine(FadeOut());

        yield return new WaitForSeconds(1f);
        audioSource.clip = runnerSoundTrack;
        audioSource.Play();

        yield return StartCoroutine(FadeIn());

        // End after fading in the runner track without looping again
        isFading = false;
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, maxVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = maxVolume;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(maxVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0f;
    }
}
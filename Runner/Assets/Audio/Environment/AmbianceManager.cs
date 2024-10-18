using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceManager : MonoBehaviour
{
    public AudioClip ScavnengerAmbience;
    public AudioClip flightDeckAmbience;
    public AudioSource audioSource;
    public float fadeDuration = 5f;
    public float maxVolume = 0.15f;

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
        StartCoroutine(ChangeAmbientClip());
    }

    public IEnumerator ChangeAmbientClip() 
    {
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(1f);
        audioSource.clip = flightDeckAmbience;
        audioSource.Play();
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
        yield return new WaitForSeconds(audioSource.clip.length - fadeDuration - 5);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut() 
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration) 
        {
            //Debug.Log("Volume: " + audioSource.volume + " Elapsed Time: " + elapsedTime);
            audioSource.volume = Mathf.Lerp(maxVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //Debug.Log("Volume: " + audioSource.volume + " Elapsed Time: " + elapsedTime);
        audioSource.volume = 0f;
        StartCoroutine(FadeIn());
    }
}

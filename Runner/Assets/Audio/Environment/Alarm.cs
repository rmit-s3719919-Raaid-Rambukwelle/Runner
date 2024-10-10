using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    public AudioSource audioSource;
    public float pauseDuration = 5f;
    public float fadeDuration = .1f;

    void Start()
    {
        StartCoroutine(LoopAlarm());
    }

    private IEnumerator LoopAlarm() 
    {
        while (true) 
        {
            audioSource.volume = 1f;
            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length - fadeDuration);

            float elapsedTime = 0f;
            float startVolume = audioSource.volume;

            while (elapsedTime < fadeDuration) 
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = 0f;

            yield return new WaitForSeconds(pauseDuration);
        }
    }
}

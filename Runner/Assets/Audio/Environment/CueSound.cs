using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueSound : MonoBehaviour
{
    public List<AudioSource> audioSources;
    public PlayerCamera playerCamera;

    public void PlaySoundByIndex(int index) 
    {
        if (index >= 0 && index < audioSources.Count) 
        {
            audioSources[index].Play();
        }
    }

    public void PlayAllSounds() 
    {
        foreach (AudioSource source in audioSources) 
        {
            source.Play();
        }
    }

    public void StopSoundByIndex(int index) 
    {
        if (index >0 && index < audioSources.Count) 
        {
            audioSources[index].Stop();
        }
    }

    public void StopAllSounds() 
    {
        foreach (AudioSource source in audioSources) 
        {
            source.Stop();
        }
    }

    public IEnumerator CameraShake() 
    {
        playerCamera.ActivateScreenShake();

        yield return new WaitForSeconds(2f);

        playerCamera.DeactivateScreenShake();
    }
}

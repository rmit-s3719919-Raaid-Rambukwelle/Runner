using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueSound : MonoBehaviour
{
    public List<AudioSource> audioSources;
    // Update is called once per frame

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
}

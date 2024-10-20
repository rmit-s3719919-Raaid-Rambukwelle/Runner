using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip[] walkSounds;
    public AudioClip[] actionSounds;
    public AudioClip[] miscSounds;
    public AudioClip grappleSoundOne;
    public AudioClip grappleSoundTwo;

    [Header("Audio Sources")]
    public AudioSource audioSource;
    public AudioSource actionSource;
    public AudioSource miscSource;

    [Header("Play Sequential")]
    private int sequentialIndex = 0;
    public List<AudioClip> sequentialSoundList;

    public AudioSource sequentialSource;

    public void PlayWalkSound() 
    {
        AudioClip clip = walkSounds[(int)Random.Range(0, walkSounds.Length)];
        audioSource.PlayOneShot(clip);
    }

    public void PlayActionSound()
    {
        AudioClip clip = actionSounds[(int)Random.Range(0, actionSounds.Length)];
        actionSource.PlayOneShot(clip);
    }

    public void PlayMiscSound()
    {
        AudioClip clip = miscSounds[(int)Random.Range(0, miscSounds.Length)];
        miscSource.PlayOneShot(clip);
    }

    public void PlayGrappleSoundOne()
    {
        audioSource.PlayOneShot(grappleSoundOne);
    }    
    
    public void PlayGrappleSoundTwo()
    {
        audioSource.PlayOneShot(grappleSoundTwo);
    }

    public void StopWalkSound() 
    {
        if (audioSource.isPlaying) 
        {
            audioSource.Stop();    
        }
    }

    public void PlaySequentialSound() 
    {
        if (sequentialSoundList != null && sequentialIndex < sequentialSoundList.Count) 
        {
            AudioClip clip = sequentialSoundList[sequentialIndex];
            sequentialSource.PlayOneShot(clip);

            sequentialIndex++;
        }
    }
}

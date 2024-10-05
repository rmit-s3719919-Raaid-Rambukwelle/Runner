using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Footsteps")]
    public AudioClip[] walkSounds;
    private AudioSource footstepSource;

    private void Awake()
    {
        footstepSource = GetComponent<AudioSource>();
    }

    public void PlayWalkSound() 
    {
        AudioClip clip = walkSounds[(int)Random.Range(0, walkSounds.Length)];
        footstepSource.PlayOneShot(clip);
    }

    public void StopWalkSound() 
    {
        if (footstepSource.isPlaying) 
        {
            footstepSource.Stop();        
        }
    }
}

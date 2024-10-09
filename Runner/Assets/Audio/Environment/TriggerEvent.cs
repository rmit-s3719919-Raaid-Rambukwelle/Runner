using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource audioSource;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}

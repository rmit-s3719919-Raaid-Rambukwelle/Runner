using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource audioSource;

    public PlayerCamera playerCamera;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CameraShake());
            GetComponent<Collider>().enabled = false;
        }
    }

    public IEnumerator CameraShake()
    {
        audioSource.PlayOneShot(audioClip);

        playerCamera.ActivateScreenShake();

        yield return new WaitForSeconds(2f);

        playerCamera.DeactivateScreenShake();
    }
}
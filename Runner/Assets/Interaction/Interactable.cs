using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();

    public bool interactable;
    public bool canGrapple;

    [Header("UI Text")]
    public bool showText;
    public string textToShow;

    [Header("Dialogue Text")]
    public bool updateNPCDialogue;
    public GameObject newTrigger;

    [Header("Additional Interactions")]
    public PlayableDirector playableDirector;


    private void OnTriggerEnter(Collider other)
    {
        PlayerManager.current.currentInteractable = this;
    }

    private void OnTriggerExit(Collider other)
    {
        if (PlayerManager.current.currentInteractable == this)
            PlayerManager.current.currentInteractable = null;
    }

    public void PlayCutscene() 
    {
        if (playableDirector != null) 
        {
            playableDirector.Play();
        }
    }
}

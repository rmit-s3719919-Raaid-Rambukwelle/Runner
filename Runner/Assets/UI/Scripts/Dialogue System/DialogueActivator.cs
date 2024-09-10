using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;


    public void UpdateDialogueObject(DialogueObject dialogueObject) 
    {
        this.dialogueObject = dialogueObject;
    }

    public void DisableDialogue()
    {

        PlayerManager.current.Interactable = null;
        
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerManager playerManager))
        {
            playerManager.Interactable = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerManager playerManager))
        {
            if (playerManager.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                playerManager.Interactable = null;

            }
        }

    }
    public void Interact(PlayerManager playerManager)
    {
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == dialogueObject)
        {
            playerManager.DialogueUI.AddResponseEvents(responseEvents.Events);
        }

        playerManager.DialogueUI.ShowDialogue(dialogueObject);
    }
}
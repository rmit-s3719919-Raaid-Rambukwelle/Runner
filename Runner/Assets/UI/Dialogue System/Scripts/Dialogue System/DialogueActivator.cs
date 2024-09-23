using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : PlayerControlHandler, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private bool autoTriggerDialogue = false;


    public void UpdateDialogueObject(DialogueObject dialogueObject) 
    {
        this.dialogueObject = dialogueObject;
    }

    public void DisableDialogue()
    {
        PlayerManager.current.Interactable = null;
        
        gameObject.SetActive(false);
        EnablePlayerControls();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerManager playerManager))
        {
            if (autoTriggerDialogue) 
            {
                TriggerDialogue(playerManager);
            } else 
                {
                    playerManager.Interactable = this;
                }
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
        TriggerDialogue(playerManager);
    }

    public void TriggerDialogue(PlayerManager playerManager) 
    {
        DisablePlayerControls();
        autoTriggerDialogue = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == dialogueObject)
        {
            playerManager.DialogueUI.AddResponseEvents(responseEvents.Events);
        }

        playerManager.DialogueUI.ShowDialogue(dialogueObject);
    }
}
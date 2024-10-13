using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : PlayerControlHandler, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private bool autoTriggerDialogue = false;
    public bool showText;
    public string popupText;

    public void UpdateDialogueObject(DialogueObject dialogueObject) 
    {
        this.dialogueObject = dialogueObject;
    }

    public void DisableDialogue()
    {
        PlayerManager.current.UpdatePopupText(" ");
        PlayerManager.current.Interactable = null;
        gameObject.SetActive(false);
        EnablePlayerControls();
        playerCameraScript.AdjustNpcCam(10);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CannotInteract() 
    {
        PlayerManager.current.Interactable = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerManager playerManager))
        {
                if (autoTriggerDialogue)
                {
                    TriggerDialogue(playerManager);
                    playerCameraScript.AdjustNpcCam(50);
                }
                else
                {
                    playerManager.Interactable = this;
                    if (showText)
                        playerManager.UpdatePopupText(popupText);
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
                if (showText)
                    playerManager.UpdatePopupText(" ");
            }
        }
    }
    public void Interact(PlayerManager playerManager)
    {
        TriggerDialogue(playerManager);
        playerCameraScript.AdjustNpcCam(50);
        PlayerManager.current.UpdatePopupText(" ");
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

    public void OnDialogueClose() 
    {
        autoTriggerDialogue = false;
    }

    public void DisableText()
    {
        PlayerManager.current.UpdatePopupText(" ");
        showText = false;
    }
}
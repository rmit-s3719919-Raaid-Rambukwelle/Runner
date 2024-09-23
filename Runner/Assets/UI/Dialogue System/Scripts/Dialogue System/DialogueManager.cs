using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueActivator dialogueActivator;
    [SerializeField] private DialogueObject defaultDialogue;
    [SerializeField] private DialogueObject updatedDialogue;

    private bool powerOn = false;

    public void OnPowerSwitchToggled() 
    {
        powerOn = true;
        NewDialogueObject();
    }

    public void NewDialogueObject() 
    {
        if (powerOn) 
        {
            dialogueActivator.UpdateDialogueObject(updatedDialogue);
        } else 
            {
                dialogueActivator.UpdateDialogueObject(defaultDialogue);
            }
    }
}

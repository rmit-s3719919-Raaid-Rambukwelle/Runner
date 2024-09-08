using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out FirstPersonDrifter firstPersonDrifter))
        {
            firstPersonDrifter.Interactable = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out FirstPersonDrifter firstPersonDrifter)) 
        {
            if (firstPersonDrifter.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this) 
            {
                firstPersonDrifter.Interactable = null;
            }
        }

    }
    public void Interact(FirstPersonDrifter firstPersonDrifter) 
    {
        firstPersonDrifter.DialogueUI.ShowDialogue(dialogueObject);
    }
}

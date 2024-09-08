using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E)) 
        {

        }    
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            OnObjectClicked();
        }
    }

    private void OnObjectClicked() 
    {
        if (Interactable != null)
        {
            //Interactable.Interact(firstPersonDrifter: this);
        }
        Debug.Log("You clicked me!");
    }
}

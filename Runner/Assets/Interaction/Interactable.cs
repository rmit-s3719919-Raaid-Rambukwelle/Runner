using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();


    private void OnMouseEnter()
    {
        //Debug.Log("Hovered over " + gameObject.name);
        PlayerController.instance.currentInteractable = this;
    }

    private void OnMouseExit()
    {
        //Debug.Log("Hovered left " + gameObject.name);
        if (PlayerController.instance.currentInteractable == this)
            PlayerController.instance.currentInteractable = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();

    public bool interactable;

    [Header("UI Text")]
    public bool showText;
    public string textToShow;



    private void OnTriggerEnter(Collider other)
    {
        if (!interactable) return;
        PlayerManager.current.currentInteractable = this;
        if (showText)
            PlayerManager.current.UpdatePopupText(textToShow);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!interactable) return;
        if (PlayerManager.current.currentInteractable == this)
        {
            PlayerManager.current.currentInteractable = null;
            if (showText)
                PlayerManager.current.UpdatePopupText(" ");
        }
    }

    public void EnableInteractable()
    {
        interactable = true;
        showText = true;
    }
}

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

    [Header("Player Animation")]
    public string animationString;
    public Animator playerAni;

    [Header("Deactivate Objects")]
    public bool deactivateObjects;
    public float delayBetweenDeactivation;
    public GameObject[] objsToDeactivate;

    [Header("Activate Objects")]
    public bool activateObjects;
    public float delayBetweenActivation;
    public GameObject[] objsToActivate;

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

    protected IEnumerator deactivateObjectsInScript()
    {
        foreach (var obj in objsToDeactivate)
        {
            yield return new WaitForSeconds(delayBetweenDeactivation);
            obj.SetActive(false);
        }
    }

    protected IEnumerator activateObjectsInScript()
    {
        foreach (var obj in objsToActivate)
        {
            yield return new WaitForSeconds(delayBetweenActivation);
            obj.SetActive(true);
        }
    }
}

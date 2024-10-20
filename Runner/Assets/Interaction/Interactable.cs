using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();

    public bool interactable;

    [Header("UI Icon")]
    public GameObject interactIcon;

    [Header("UI Text")]
    public bool showText;
    public string textToShow;

    [Header("Player Animation")]
    public string triggerString;
    public Animator playerAni;

    [Header("Deactivate Objects")]
    public bool deactivateObjects;
    public float delayBetweenDeactivation;
    public GameObject[] objsToDeactivate;

    [Header("Activate Objects")]
    public bool activateObjects;
    public float delayBetweenActivation;
    public GameObject[] objsToActivate1;
    public GameObject[] objsToActivate2;
    public GameObject[] objsToActivate3;

    private void OnTriggerEnter(Collider other)
    {
        if (!interactable) return;
        PlayerManager.current.currentInteractable = this;
        interactIcon.SetActive(true);
        if (showText)
            PlayerManager.current.UpdatePopupText(textToShow);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!interactable) return;
        if (PlayerManager.current.currentInteractable == this)
        {
            PlayerManager.current.currentInteractable = null;
            interactIcon.SetActive(false);
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
            obj.SetActive(false);
            yield return null;
        }
    }

    protected IEnumerator activateObjectsInScript()
    {
        foreach (var obj1 in objsToActivate1)
        {
            obj1.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        foreach (var obj2 in objsToActivate2)
        {
            obj2.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        foreach (var obj3 in objsToActivate3) 
        {
             obj3.SetActive(true);
        }
            
        
    }
}

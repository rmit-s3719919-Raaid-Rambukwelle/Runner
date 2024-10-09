using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Interaction : Interactable
{
    [Header("Interaction")]
    public bool deactivateOnUse;
    public bool locked;
    public string lockedMessage;
    public string requiredObj;




    [Header("Audio")]
    public bool playAudio;
    public PlayerAudio ap;

    public override void Interact()
    {
        if (!interactable) return;

        if (locked && !PlayerManager.current.SearchInventory(requiredObj))
        {
            PlayerManager.current.UpdatePopupText(lockedMessage);
            return;
        }

        if (deactivateOnUse) interactable = false;

        if (useAnimation)
            ani.SetBool(animationString, true);

        if (playAudio)
            ap.PlayActionSound();

        if (deactivateObjects)
            StartCoroutine(deactivateObjectsInScript());

        if (activateObjects)
            StartCoroutine(activateObjectsInScript());

        PlayerManager.current.UpdatePopupText(" ");
    }


}

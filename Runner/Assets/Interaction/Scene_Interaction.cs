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

    [Header("Animation")]
    public bool useAnimation;
    public string animationString;
    public Animator ani;

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
    }
}

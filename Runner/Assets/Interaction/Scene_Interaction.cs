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

    [Header("Object Animation")]
    public bool useAnimation;
    public string objAnimationString;
    public Animator objAni;

    [Header("Audio")]
    public bool playAudio;
    public CueSound ap;

    public override void Interact()
    {
        if (!interactable) return;

        if (locked && !PlayerManager.current.SearchInventory(requiredObj))
        {
            PlayerManager.current.UpdatePopupText(lockedMessage);
            return;
        }

        if (deactivateOnUse) interactable = false;

        playerAni.SetTrigger(triggerString);

        if (useAnimation)
            Invoke(nameof(StartAnimation), 1.5f);

        if (playAudio)
            ap.PlayAllSounds();

        if (deactivateObjects)
            StartCoroutine(deactivateObjectsInScript());

        if (activateObjects)
            StartCoroutine(activateObjectsInScript());

        PlayerManager.current.UpdatePopupText(" ");
    }

    void StartAnimation()
    {
        objAni.SetBool(objAnimationString, true);
    }


}

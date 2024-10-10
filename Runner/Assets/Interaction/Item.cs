using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    [Header("Item variables")]
    public string itemName;
    public string itemDesc;
    public Sprite itemImage;
    public string itemID;

    [Header("Grapple grab")]
    public bool canGrapple;
    public LineRenderer lr;
    public Transform grappleHand;
    public Transform target;


    private void Start()
    {
        if (canGrapple)
            disableGrapple();
    }

    public override void Interact()
    {
        AddToInventory();
    }

    public void AddToInventory()
    {
        if (showText)
            PlayerManager.current.UpdatePopupText(" ");
        PlayerManager.current.inventory.items.Add(this);
        PlayerManager.current.currentInteractable = null;

        if (deactivateObjects)
            StartCoroutine(deactivateObjectsInScript());

        if (activateObjects)
            StartCoroutine(activateObjectsInScript());

        if (canGrapple)
        {
            // disable after time
        }
        else
            gameObject.SetActive(false);
    }

    void disableGrapple()
    {
        lr.startWidth = 0f;
        lr.endWidth = 0f;
    }

    public void itemGrapple()
    {
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, grappleHand.position);
        lr.SetPosition(1, target.position);

        float d = 0f;
        ani.CrossFade("Grapple", 0.2f);
        AnimationClip[] clips = ani.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Grapple")
            {
                d = clip.length;
            }
        }
        Invoke(nameof(disableGrapple), d);
    }

}


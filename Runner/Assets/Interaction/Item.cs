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
    public AnimationClip clip;


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



        if (canGrapple)        
            ItemGrapple();
        

        playerAni.CrossFade(animationString, 0f);
        Invoke(nameof(DisableItem), 1.2f);

        if (deactivateObjects)
            StartCoroutine(deactivateObjectsInScript());

        if (activateObjects)
            StartCoroutine(activateObjectsInScript());
    }


    void DisableItem()
    {
        gameObject.SetActive(false);
    }

    public void ItemGrapple()
    {
        lr.positionCount = 0;
        lr.positionCount = 2;
        lr.SetPosition(0, grappleHand.position);
        lr.SetPosition(1, target.position);          
    }
}


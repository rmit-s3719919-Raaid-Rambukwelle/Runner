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
    public bool canGrapple;

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
        gameObject.SetActive(false);
    }   
}

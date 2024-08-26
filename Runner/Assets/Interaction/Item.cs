using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public string itemName;
    public string itemDesc;
    public Sprite itemImage;

    public float weight;


    public override void Interact()
    {
        AddToInventory();
    }

    void AddToInventory()
    {
        if (weight + PlayerController.instance.currentInventoryWeight > PlayerController.instance.maxInventoryWeight)
        {
            Debug.Log("Item is too heavy");
            return;
        }

        PlayerController.instance.currentInventoryWeight += weight;

        PlayerController.instance.items.Add(this);
        gameObject.SetActive(false);
        PlayerController.instance.currentInteractable = null;
    }

    
}

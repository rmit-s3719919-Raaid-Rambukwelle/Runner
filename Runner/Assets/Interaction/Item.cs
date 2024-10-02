using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public string itemName;
    public string itemDesc;
    public Sprite itemImage;

    public string itemID;

    public float weight;
    public FlightDeckSequenceController flightDeckSequenceController;

    public override void Interact()
    {
        AddToInventory();
        if (newTrigger != null) 
        {
            flightDeckSequenceController.StartTransition();
        }
    }

    void AddToInventory()
    {
        if (weight > 100f)
        {
            Debug.Log("Item is too heavy");
            return;
        }


        PlayerManager.current.inventory.items.Add(this);
        gameObject.SetActive(false);
        PlayerManager.current.currentInteractable = null;
    }   
}

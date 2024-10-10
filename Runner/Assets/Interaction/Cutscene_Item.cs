using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_Item : Item
{
    [Header("Cutscene variables")]
    public GameObject newTrigger;
    public FlightDeckSequenceController flightDeckSequenceController;
    public override void Interact()
    {
        AddToInventory();
        if (newTrigger != null)
        {
            StartCutscene();            
        }
    }

    void StartCutscene()
    {
        flightDeckSequenceController.StartTransition();
    }
}

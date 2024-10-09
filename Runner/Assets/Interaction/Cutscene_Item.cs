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
            flightDeckSequenceController.StartSequence();
            float animationLength = flightDeckSequenceController.GetAnimationClipLength(flightDeckSequenceController.robotAnimator, "PushButton");
            Invoke(nameof(StartCutscene), animationLength + 2f);
            
        }
    }

    void StartCutscene()
    {
        flightDeckSequenceController.StartTransition();
    }


}

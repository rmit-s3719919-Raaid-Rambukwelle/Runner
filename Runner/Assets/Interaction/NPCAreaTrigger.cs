using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAreaTrigger : MonoBehaviour
{
    // Use one of these per NPC
    // e.g. if we have 2 NPCs, use two NPCAreaTrigger objects

    public bool singleActivation;
    public bool activateDialogue;
    [SerializeField] bool readyToTrigger;
    bool canActivate = true;
    public string triggerTag;
    public Transform newTargetPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (!canActivate) return;
        // Activate on NPC with tag entering range
        if (other.CompareTag(triggerTag))
        {
            Debug.Log("Hit correct NPC");

            // Sets next position
            other.GetComponent<NPCMovementTrigger>().targetPosition = newTargetPosition;
            readyToTrigger = true;
        }

        // Activate on player entering range
        // readyToTrigger bool activates only once NPC has entered range
        // activateDialogue bool is used to control the player and dialogue - only one NPC needs to control the player and dialogue at a time
        if (other.CompareTag("Player") && readyToTrigger && activateDialogue)
        {

            // Disables trigger if only used for single activation
            if (singleActivation)
                canActivate = false;

            // Add functions to start dialogue here
            Debug.Log("Start Dialogue");
            
        }
    }
}

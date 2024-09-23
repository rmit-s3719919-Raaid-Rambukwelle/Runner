using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAreaTrigger : MonoBehaviour
{
    public bool singleActivation;
    public bool activateDialogue;

    [SerializeField] private bool readyToTrigger;

    private bool canActivate = true;
    public string triggerTag;

    public Transform newTargetPosition;

    private bool isNpcInZone = false; // Tracks if NPC is in zone
    private bool isPlayerInZone = false; // Tracks if Player is in zone

    private void OnTriggerEnter(Collider other)
    {
        if (!canActivate) return;

        // Check if npc enters trigger zone
        if (other.CompareTag(triggerTag))
        {
            Debug.Log("NPC entered trigger zone");

            // Sets next position
            other.GetComponent<NPCMovementTrigger>().targetPosition = newTargetPosition;
            isNpcInZone = true;
            readyToTrigger = true;
        }

        // Activate on player entering range
        // readyToTrigger bool activates only once NPC has entered range
        // activateDialogue bool is used to control the player and dialogue - only one NPC needs to control the player and dialogue at a time
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone");
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggerTag)) 
        {
            Debug.Log("NPC left trigger zone");
            isNpcInZone = false;
            readyToTrigger = false;
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left trigger zone");
            isPlayerInZone = false;
        }
    }

    public bool isNPCinTriggerZone() 
    {
        return isNpcInZone && isPlayerInZone && readyToTrigger;
    }

    public void Test() 
    {
        Debug.Log("If you're reading this, then the dialouge system can handle multiple dialogues");
    }
}

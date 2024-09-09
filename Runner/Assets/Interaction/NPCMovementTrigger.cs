using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovementTrigger : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform targetPosition;

    // Call this method from other scripts to start movement
    public void StartMovement()
    {
        // Starts movement according to navmesh
        agent.SetDestination(targetPosition.position);

        // Re-enables player movement
        PlayerController.instance.canMove = true;
    }

    // Space bar to activate movement
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartMovement();
            Debug.Log("Moving NPC");
        }
    }
}

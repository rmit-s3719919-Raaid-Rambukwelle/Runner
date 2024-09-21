using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControlHandler : MonoBehaviour
{
    public ThirdPersonMovement playerMovementScript;
    public ThirdPersonCamera playerCameraScript;

    protected virtual void Start()
    {
        playerMovementScript = FindObjectOfType<ThirdPersonMovement>();
        playerCameraScript = FindObjectOfType<ThirdPersonCamera>();

        if (playerMovementScript == null || playerCameraScript == null)
        {
            Debug.LogError("Failed to find player movement or camera scripts!");
        }
    }

    protected virtual void DisablePlayerControls() 
    {
        playerMovementScript.enabled = false;
        playerCameraScript.enabled = false;
    }

    protected virtual void EnablePlayerControls() 
    {
        playerMovementScript.enabled = true;
        playerCameraScript.enabled = true;
    }
}

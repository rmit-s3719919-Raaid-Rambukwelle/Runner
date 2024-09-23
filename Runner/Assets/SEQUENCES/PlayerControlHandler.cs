using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControlHandler : MonoBehaviour
{
    public ThirdPersonMovement playerMovementScript;
    public ThirdPersonCamera playerCameraScript;
    public PlayerManager playerManagerScript;

    protected virtual void Start()
    {
        playerMovementScript = FindObjectOfType<ThirdPersonMovement>();
        playerCameraScript = FindObjectOfType<ThirdPersonCamera>();
        playerManagerScript = FindObjectOfType<PlayerManager>();

        if (playerMovementScript == null || playerCameraScript == null)
        {
            Debug.LogError("Failed to find player movement or camera scripts!");
        }
    }

    protected virtual void DisablePlayerControls() 
    {
        playerMovementScript.enabled = false;
        playerCameraScript.enabled = false;
        playerManagerScript.enabled = false;
    }

    protected virtual void EnablePlayerControls() 
    {
        playerMovementScript.enabled = true;
        playerCameraScript.enabled = true;
        playerManagerScript.enabled = true;
    }
}

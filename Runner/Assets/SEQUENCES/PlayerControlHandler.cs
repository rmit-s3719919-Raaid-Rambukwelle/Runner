using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControlHandler : MonoBehaviour
{
    public ThirdPersonMovement playerMovementScript;
    public ThirdPersonCamera playerCameraScript;
    public PlayerManager playerManagerScript;

    protected virtual void Awake() 
    {
        playerMovementScript = FindObjectOfType<ThirdPersonMovement>();
        playerCameraScript = FindObjectOfType<ThirdPersonCamera>();
        playerManagerScript = FindObjectOfType<PlayerManager>();

        if (playerMovementScript == null || playerCameraScript == null)
        {
            Debug.LogError("Failed to find player movement or camera scripts!");
        }
    }
    protected virtual void Start()
    {

    }

    protected virtual void DisablePlayerControls() 
    {
        //Debug.Log("DisablePlayerControls called from: " + this.GetType().Name);
        playerMovementScript.enabled = false;
        playerCameraScript.enabled = false;
        playerManagerScript.enabled = false;
    }

    protected virtual void EnablePlayerControls() 
    {
        //Debug.Log("EnablePlayerControls called from: " + this.GetType().Name);
        playerMovementScript.enabled = true;
        playerCameraScript.enabled = true;
        playerManagerScript.enabled = true;
    }
}

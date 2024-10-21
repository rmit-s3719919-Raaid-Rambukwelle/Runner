using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControlHandler : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
    public PlayerCamera playerCameraScript;
    public PlayerManager playerManagerScript;

    protected virtual void Awake() 
    {
        playerMovementScript = FindObjectOfType<PlayerMovement>();
        playerCameraScript = FindObjectOfType<PlayerCamera>();
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
        playerMovementScript.ani.SetFloat("Velocity", 0f);
        playerManagerScript.canMove = false;
    }

    protected virtual void EnablePlayerControls() 
    {
        //Debug.Log("EnablePlayerControls called from: " + this.GetType().Name);
        playerManagerScript.canMove = true;
    }
}

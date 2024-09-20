using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;

    int isWalkingBackwardsHash;

    int isWalkingRightHash;
    int isWalkingLeftHash;

    int isLookingLeftHash;
    int isLookingRightHash;

    public void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isWalkingBackwardsHash = Animator.StringToHash("isWalkingBackwards");

        isWalkingRightHash = Animator.StringToHash("isWalkingRight");
        isWalkingLeftHash = Animator.StringToHash("isWalkingLeft");

        isLookingLeftHash = Animator.StringToHash("isLookingLeft");
        isLookingRightHash = Animator.StringToHash("isLookingRight");
    }

    public void Update()
    {
        HandleWalking();
        HandleStrafeWalking();
        HandleBackwardsWalking();
        HandleLook();
    }

    private void HandleWalking() 
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");

        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (isWalking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }
    }

    private void HandleBackwardsWalking() 
    {
        bool isWalkingBackwards = animator.GetBool(isWalkingBackwardsHash);
        bool backwardsPressed = Input.GetKey("s");

        if (!isWalkingBackwards && backwardsPressed) 
        {
            animator.SetBool(isWalkingBackwardsHash, true);
        }

        if (isWalkingBackwards && !backwardsPressed) 
        {
            animator.SetBool(isWalkingBackwardsHash, false);
        }
    }

    private void HandleStrafeWalking()
    {
        bool isWalking = animator.GetBool(isWalkingHash);  // Check if moving forward
        bool rightPressed = Input.GetKey("d");
        bool leftPressed = Input.GetKey("a");

        // Only trigger strafe walk if NOT moving forward
        if (!isWalking)
        {
            // Handle strafing to the right
            if (rightPressed)
            {
                animator.SetBool(isWalkingRightHash, true);
            }
            else if (!rightPressed)
            {
                animator.SetBool(isWalkingRightHash, false);  // Reset right strafe if 'd' is released
            }

            // Handle strafing to the left
            if (leftPressed)
            {
                animator.SetBool(isWalkingLeftHash, true);
            }
            else if (!leftPressed)
            {
                animator.SetBool(isWalkingLeftHash, false);  // Reset left strafe if 'a' is released
            }
        }
        else
        {
            // Ensure both strafe animations are reset when moving forward
            animator.SetBool(isWalkingRightHash, false);
            animator.SetBool(isWalkingLeftHash, false);
        }
    }

    private void HandleLook() 
    {
        float mouseX = Input.GetAxis("Mouse X");

        // Set a sensitivity threshold
        float sensitivityThreshold = 0.1f; // Adjust this value as needed

        bool isMoving = Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d");

        if (isMoving)
        {
            animator.SetBool(isLookingLeftHash, false);
            animator.SetBool(isLookingRightHash, false);
            return;
        }

        if (mouseX < -sensitivityThreshold) // Looking left
        {
            animator.SetBool(isLookingLeftHash, true);
            animator.SetBool(isLookingRightHash, false);
        }
        else if (mouseX > sensitivityThreshold) // Looking right
        {
            animator.SetBool(isLookingRightHash, true);
            animator.SetBool(isLookingLeftHash, false);
        }
        else // No significant horizontal movement, reset both animations
        {
            animator.SetBool(isLookingLeftHash, false);
            animator.SetBool(isLookingRightHash, false);
        }
    }
}

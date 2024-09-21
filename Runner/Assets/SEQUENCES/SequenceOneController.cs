using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceOneController : PlayerControlHandler
{

    public Animator playerAnimator;
    public string animationName;
    public GameObject player;

    //private bool sequenceFinished = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(PlayOpeningSequence());
    }

    IEnumerator PlayOpeningSequence() 
    {
        DisablePlayerControls();

        playerAnimator.Play("StandingUp");

        float animationSpeed = playerAnimator.GetCurrentAnimatorStateInfo(0).speed;

        // Get the length of the animation and divide by the speed to account for the faster playback
        float animationLength = GetAnimationClipLength(playerAnimator, "StandingUp") / animationSpeed;

        // Wait for the adjusted length of the animation
        yield return new WaitForSeconds(animationLength);

        playerAnimator.SetTrigger("AllowMovement");

        EnablePlayerControls();
    }

    float GetAnimationClipLength(Animator animator, string clipName) 
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips) 
        {
            if (clip.name == clipName) 
            {
                return clip.length;
            }
        }
        return 0;
    }
}

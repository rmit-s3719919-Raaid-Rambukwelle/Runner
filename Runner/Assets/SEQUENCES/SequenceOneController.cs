using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceOneController : PlayerControlHandler
{
    public Animator playerAnimator;
    public Animator robotAnimator;

    private bool isSequenceActive;

    public GameObject player;
    public GameObject dialogue;

    //private bool sequenceFinished = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(PlayOpeningSequence());
    }

    IEnumerator PlayOpeningSequence()
    {
        isSequenceActive = true;
        DisablePlayerControls();
        StartCoroutine(PlayerAnimation());
        StartCoroutine(RobotAnimation());
        isSequenceActive = false;

        StartCoroutine(StartDialogueSequence(7f));

        yield return null;
    }

    IEnumerator PlayerAnimation() 
    {
        playerAnimator.Play("StandingUp");
        float animationLength = GetAnimationClipLength(playerAnimator, "StandingUp");
        yield return new WaitForSeconds(animationLength);

        playerAnimator.SetTrigger("AllowMovement");
        Debug.Log("StandingUp animation finished, transitioning to idle.");

    }

    IEnumerator RobotAnimation()
    {
        robotAnimator.Play("Standing");
        float animationLength = GetAnimationClipLength(robotAnimator, "Standing");
        yield return new WaitForSeconds(animationLength);

        robotAnimator.SetTrigger("AllowMovement");
    }

    IEnumerator StartDialogueSequence(float delay) 
    {
        yield return new WaitForSeconds(delay);
        dialogue.SetActive(true);
        Debug.Log("Dialogue started");
    }

    public bool isSequenceOneActive()
    {
        return isSequenceActive;
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
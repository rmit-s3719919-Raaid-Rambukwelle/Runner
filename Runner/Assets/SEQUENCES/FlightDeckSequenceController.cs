using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FlightDeckSequenceController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator robotAnimator;

    [SerializeField] private PlayableDirector playableDirector;

    public void StartSequence() 
    {
        StartCoroutine(PlayFlightDeckSequence());
    }

    IEnumerator PlayFlightDeckSequence() 
    {
        Debug.Log("Beginning Animation");
        StartCoroutine(RobotAnimation());
        yield return null;
    }

    IEnumerator PlayerAnimation() 
    {
        playerAnimator.Play("FallingDown");
        float animationLength = GetAnimationClipLength(playerAnimator, "StandingUp");
        yield return new WaitForSeconds(animationLength);

        playerAnimator.SetTrigger("AllowMovement");
        Debug.Log("StandingUp animation finished, transitioning to idle.");
    }

    IEnumerator RobotAnimation()
    {
        robotAnimator.SetTrigger("ButtonTrigger");
        //robotAnimator.Play("PushButton");
        float animationLength = GetAnimationClipLength(robotAnimator, "PushButton");
        yield return new WaitForSeconds(animationLength);

        
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

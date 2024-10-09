using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FlightDeckSequenceController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] public Animator robotAnimator;

    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Item theCore;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject runnerSpawn;


    public void StartSequence() 
    {
        StartCoroutine(PlayFlightDeckSequence());
    }

    public void StartTransition() 
    {
        StartCoroutine(PlayTransitionSequence());
    }

    IEnumerator PlayTransitionSequence() 
    {
        //StartCoroutine(PlayerAnimation());
        PlayCutscene();

        yield return null;
    }

    IEnumerator PlayFlightDeckSequence() 
    {
        Debug.Log("Beginning Animation");
        StartCoroutine(RobotAnimation());

        if (theCore != null) 
        {
            Debug.Log("The core is active");
            theCore.interactable = true;
        }
        yield return null;
    }

    IEnumerator PlayerAnimation() 
    {
        playerAnimator.SetTrigger("takenCore");
        float animationLength = GetAnimationClipLength(playerAnimator, "Fall");
        yield return new WaitForSeconds(animationLength);
        //Debug.Log("StandingUp animation finished, transitioning to idle.");
    }

    IEnumerator RobotAnimation()
    {
        robotAnimator.SetTrigger("ButtonTrigger");
        //robotAnimator.Play("PushButton");
        float animationLength = GetAnimationClipLength(robotAnimator, "PushButton");
        yield return new WaitForSeconds(animationLength);    
    }

    public float GetAnimationClipLength(Animator animator, string clipName)
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

    public void PlayCutscene()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
        }
    }

    public void MovePlayer() 
    {
        player.transform.position = runnerSpawn.transform.position;
    }
}

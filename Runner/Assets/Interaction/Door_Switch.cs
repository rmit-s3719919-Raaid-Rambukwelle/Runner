using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Switch : MonoBehaviour
{
    [Header("Switch variables")]
    public GameObject DoorObj;
    public GameObject robot;
    public GameObject nextTrigger;
    public Animator doorAnimator;

    [Header("Dialogue variables")]
    public NPCMovementTrigger npcMovementTrigger;
    public DialogueActivator dialogueActivator;

    private Quaternion originalRotation;


    public void Interact()
    {
        StartCoroutine(PlayAnimationSequence());
    }

    private IEnumerator PlayAnimationSequence() 
    {
        originalRotation = robot.transform.rotation;

        //robot face door
        Vector3 targetDirection = DoorObj.transform.position - robot.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // smooth rotation
        float rotationSpeed = 5f;
        while(Quaternion.Angle(robot.transform.rotation, targetRotation) > 0.1f) 
        {
            robot.transform.rotation = Quaternion.Slerp(robot.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        Animator robotAnimator = robot.GetComponent<Animator>();
        robotAnimator.SetTrigger("EnterCodeTrigger");

        float enterCodeLength = GetAnimationClipLength(robotAnimator, "EnterCode");
        Debug.Log("Robot animation length: " + enterCodeLength);
        yield return new WaitForSeconds(enterCodeLength);

        Debug.Log("Robot animation finished. Now opening the door.");

        doorAnimator.SetTrigger("OpenTrigger");

        yield return new WaitForSeconds(1f);

        nextTrigger.SetActive(true);

        robot.transform.rotation = originalRotation;

        dialogueActivator.DisableDialogue();
    }

    private float GetAnimationClipLength(Animator animator, string clipName) 
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

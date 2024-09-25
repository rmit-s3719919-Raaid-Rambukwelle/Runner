using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Switch : Interactable
{
    [Header("Switch variables")]
    public GameObject DoorObj;
    public GameObject robot;

    public NPCMovementTrigger npcMovementTrigger;
    public DialogueActivator dialogueActivator;

    public bool locked = false;
    public string lockedMessage;
    public string requiredObj;
    public bool reactivateOnUse;
    public Animator doorAnimator;

    private Quaternion originalRotation;

    bool active = true;


    public override void Interact()
    {
        if (!active) return;

        if (locked && !PlayerManager.current.SearchInventory(requiredObj))
        {
            PlayerManager.current.interactText.text = lockedMessage;
            return;
        }

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

        yield return new WaitForSeconds(0.5f);

        npcMovementTrigger.StartMovement();

        robot.transform.rotation = originalRotation;

        if (!reactivateOnUse) active = false;

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

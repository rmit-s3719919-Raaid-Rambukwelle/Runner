using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAnimationController : MonoBehaviour
{
    public Animator animator;

    private NavMeshAgent agent;
    public Transform player;

    public float detectionRadius = 5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (agent.velocity.magnitude > 0.1f) 
        {
            animator.SetBool("isWalking", true);
        } else 
            {
            animator.SetBool("isWalking", false);

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (distanceToPlayer <= detectionRadius) 
                {
                    //FacePlayer();
                }

            }
    }

    private void FacePlayer() 
    {
        //get direction to player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        //calculate rotation  
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

        //smoothly rotate to face player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}

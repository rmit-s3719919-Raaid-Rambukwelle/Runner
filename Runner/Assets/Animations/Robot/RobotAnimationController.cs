using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAnimationController : MonoBehaviour
{
    public Animator animator;
    private NavMeshAgent agent;

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
            }
    }
}

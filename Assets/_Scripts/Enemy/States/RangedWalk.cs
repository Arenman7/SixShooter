using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedWalk : StateMachineBehaviour
{

    public Transform playerTransform;
    NavMeshAgent navMeshAgent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTransform = FindObjectOfType<PlayerHealth>().transform;
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent.SetDestination(playerTransform.position);
        CheckForRange(animator);
        animator.transform.LookAt(playerTransform);
    }

    void CheckForRange(Animator animator)
    {
        if(Vector3.Distance(playerTransform.position, navMeshAgent.transform.position) <= 25)
        {
            navMeshAgent.SetDestination(navMeshAgent.transform.position);
            animator.SetTrigger("Attack");
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

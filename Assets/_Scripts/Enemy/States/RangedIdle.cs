using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedIdle : StateMachineBehaviour
{
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    Transform player;
    RaycastHit hit;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindObjectOfType<PlayerHealth>().transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
       if(Vector3.Distance(animator.transform.position, player.transform.position ) < 20f )
       {
            animator.SetTrigger("Alert");
       }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

}

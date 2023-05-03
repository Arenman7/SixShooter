using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : StateMachineBehaviour
{
    public GameObject bullet;
    public bool dualWield;
    Transform player;
    public float forceX;
    public float forceY;

    Vector3 muzzle1, muzzle2;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        muzzle1 = new Vector3(-0.855000019f,0.390022278f,1.5411396f);
        muzzle2 = new Vector3(0.788999975f,0.389999866f,1.54100037f);

        player = animator.GetComponent<Transform>();
        if(dualWield==false)
        {
        animator.transform.LookAt(player);
       Rigidbody rb = Instantiate(bullet, animator.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
       rb.AddForce(animator.transform.forward * forceX, ForceMode.Impulse);
       rb.AddForce(animator.transform.up * forceY, ForceMode.Impulse);
        }
        else
        {
            
            animator.transform.LookAt(player);
       Rigidbody rb = Instantiate(bullet, animator.transform.position + muzzle1, Quaternion.identity).GetComponent<Rigidbody>();
       rb.AddForce(animator.transform.forward * forceX, ForceMode.Impulse);
       rb.AddForce(animator.transform.up * forceY, ForceMode.Impulse);
       rb = Instantiate(bullet, animator.transform.position + muzzle2, Quaternion.identity).GetComponent<Rigidbody>();
       rb.AddForce(animator.transform.forward * forceX, ForceMode.Impulse);
       rb.AddForce(animator.transform.up * forceY, ForceMode.Impulse);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.transform.LookAt(player);
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}

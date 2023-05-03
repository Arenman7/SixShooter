using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public IdleState idleState; 

    Transform enemyTransform;
    Transform spriteTransform;
    Transform playerTransform;

    Animator spriteAnim;

    AngleToPlayer angleToPlayer;
    EnemyAIStats aiStats;

    Vector3 attackRange;
    
    public void Start()
    {

        playerTransform = FindObjectOfType<PlayerMove>().transform;
        enemyTransform = GetComponentInParent<Enemy>().transform;
        spriteTransform = GetComponentInParent<SpriteRenderer>().transform;
        spriteAnim = GetComponentInParent<Animator>();
        angleToPlayer = GetComponentInParent<AngleToPlayer>();
        aiStats = GetComponentInParent<EnemyAIStats>();
    }


    public override State RunCurrentState()
    {
        HandleWalkAnimsAndLook();
        aiStats.StartCoroutine(aiStats.MoveTowardsPlayer());

        if(aiStats.idle)
        {
            aiStats.chasePlayer = false;
            aiStats.attackPlayer = false;
            return idleState;
        }
        else if(aiStats.attackPlayer)
        {
            aiStats.idle = false;
            aiStats.chasePlayer = false;
            return attackState;
        }
        else
        {
            return this;
        }
    }

    void HandleWalkAnimsAndLook()
    {
        spriteAnim.Play("Cowboy1Walk");
        spriteAnim.SetFloat("spriteRotation", angleToPlayer.lastIndex);
        
        if(spriteTransform)
        {
            spriteTransform.LookAt(playerTransform);
        }
    }
}

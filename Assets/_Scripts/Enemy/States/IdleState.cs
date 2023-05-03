using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chaseState;
    public AttackState attackState;

    Transform enemyTransform;
    Transform spriteTransform;
    Transform playerTransform;

    Animator spriteAnim;
    AngleToPlayer angleToPlayer;
    EnemyAIStats aiStats;
    
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
        HandleAnimsAndLook();

        if(aiStats.chasePlayer)
        {
            aiStats.attackPlayer = false;
            aiStats.idle = false;
            return chaseState;
        }
        else if(aiStats.attackPlayer)
        {
            aiStats.idle = false;
            aiStats.chasePlayer = false;
            return attackState;
        }
        else
        {
            aiStats.StopMoving();
            return this;
        }
    }

    void HandleAnimsAndLook()
    {
        spriteAnim.Play("Cowboy1Idle");
        spriteAnim.SetFloat("spriteRotation", angleToPlayer.lastIndex);
        
        if(spriteTransform)
        {
            spriteTransform.LookAt(playerTransform);
        }
    }
}

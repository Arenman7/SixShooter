using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public ChaseState chaseState;
    public IdleState idleState;
    
    EnemyAIStats aiStats;

    private void Start()
    {
        aiStats = GetComponentInParent<EnemyAIStats>();
    }

    public override State RunCurrentState()
    {
        if(!aiStats.wasShot)
        aiStats.StartCoroutine(aiStats.BeginShootAtPlayer());

        if(aiStats.idle)
        {
            aiStats.chasePlayer = false;
            aiStats.attackPlayer = false;
            return idleState;
        }
        else if(aiStats.chasePlayer)
        {
            aiStats.idle = false;
            aiStats.attackPlayer = false;
            return chaseState;
        }
        else
            return this;
    }


}

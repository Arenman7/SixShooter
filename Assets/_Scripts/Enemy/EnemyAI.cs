using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Material aggroMat;
    
    //Return to origin
    public Vector3 startingPos;
    

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //range
    public float sightRange, attackRange;
    private float ogSightRange;

    //states
    public bool playerInSightRange, playerInAttackRange;
    public bool isAggro, atStartPos, walkingBack;
    
    //agro
    public bool wasShot, wasRecentlyShot;
    public bool shotNear;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();


    }

    private void Start()
    {
        startingPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        ogSightRange = sightRange;

    }

    private void Update()
    {
        //Check for sight and attack range
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) 
        {
            if(!walkingBack)
            {
                StopMoving();
                Invoke(nameof(WaitToReturn), 3f);            
            }
        }
        if(GetComponent<EnemyAwareness>().wasShot) 
        wasRecentlyShot = true;


        if (playerInSightRange && !playerInAttackRange || wasRecentlyShot || isAggro) 
        {
            ChasePlayer();
            StartCoroutine(ResetAggro(5f));
        }

        if (playerInAttackRange && playerInSightRange) AttackPlayer(); 

        IncreaseViewRange();       

    }

    private IEnumerator ResetAggro(float t)
    {
        yield return new WaitForSeconds(t);
        isAggro = false;
    }

    private void StopMoving()
    {
        agent.SetDestination(transform.position);
    }

    private void IncreaseViewRange()
    {
        if(isAggro)
        sightRange = ogSightRange + 5f;
        else
        {
            sightRange = ogSightRange;
        }
    }

    private void ChasePlayer()
    {
        
        isAggro = true;
        walkingBack = false;
        wasRecentlyShot = false;
        agent.SetDestination(player.position);
        GetComponent<EnemyAwareness>().wasShot = false;
    }

    private void AttackPlayer()
    {

        isAggro = true;
        walkingBack = false;
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here

            
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        
    }

    private void HandleAggro()
    {
        if(isAggro)
        {
            GetComponentInChildren<MeshRenderer>().material = aggroMat;
        }
    }

    private void WaitToReturn()
    {
        walkingBack=true;
        isAggro = false;
        agent.SetDestination(startingPos);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Vector3 test = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10);
        Gizmos.DrawWireSphere(test, sightRange);
    }
}


/////////////////////// 
/*
    private EnemyAwareness enemyAwareness;
    private Transform playerTransform;
    private NavMeshAgent enemyNavMeshAgent;

    private void Start()
    {
        enemyAwareness = GetComponent<EnemyAwareness>();
        playerTransform = FindObjectOfType<PlayerMove>().transform;
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        if(enemyAwareness.isAggro)
        {
            enemyNavMeshAgent.SetDestination(playerTransform.position);
        }
        else
        {
            enemyNavMeshAgent.SetDestination(transform.position);
        }
    }
}
*/
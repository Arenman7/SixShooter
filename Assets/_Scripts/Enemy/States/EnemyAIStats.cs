using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIStats : MonoBehaviour
{
    [Header("References")]
    Transform enemyTransform;
    Transform spriteTransform;
    Transform playerTransform;
    SpriteRenderer spriteRender;
    Animator animator;
    AngleToPlayer angleToPlayer;
    EnemyAwareness enemyAwareness;
    PlayerHealth playerHealth;
    
    [Header("Misc")]
    public LayerMask whatIsPlayer;
    [HideInInspector] public NavMeshAgent navMeshAgent;

    [Header("Attack Params")]
    public int damage;
    public float sightRange;
    public float attackRange;
    float ogRange;

    [Header("Bools")]
    public bool chasePlayer;
    public bool attackPlayer;
    public bool idle;
    public bool wasShot;
    public bool isAggro;
    public bool hitPlayer;

    [Header("Rotation Speeds")]
    [SerializeField] private float rotSpeed=50f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip gunShotClip;
    public AudioClip enemyHurt;

    private void Start()
    {   
        ogRange = attackRange;
        enemyAwareness = GetComponent<EnemyAwareness>();
        angleToPlayer = GetComponent<AngleToPlayer>();
        animator = GetComponentInChildren<Animator>();
        enemyTransform = GetComponent<Enemy>().transform;
        spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        spriteRender = GetComponentInChildren<SpriteRenderer>();

        playerTransform = FindObjectOfType<PlayerMove>().transform;
        playerHealth = FindObjectOfType<PlayerHealth>();

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        wasShot = enemyAwareness.wasShot;
        isAggro = enemyAwareness.isAggro;

        if(wasShot || enemyAwareness.isAggro)
        {
            if(idle)
                chasePlayer = true;
        }
    }

    public IEnumerator MoveTowardsPlayer()
    {
        if(!InAttackRange())
        {
            navMeshAgent.SetDestination(playerTransform.position);
            yield return new WaitForSeconds(5f);
        }
        else
        {//is in attack range
            StopMoving();
            attackPlayer = true;
        }
    }

    public void StopMoving()
    {
        navMeshAgent.SetDestination(enemyTransform.position);
    }

    public IEnumerator BeginShootAtPlayer()
    {
        if(InAttackRange())
        {

            attackRange = ogRange + 10; //increases attack range 

            StopMoving();

            RetrackTarget();
            
            animator.Play("Cowboy1Fire");
            
            animator.SetFloat("spriteRotation", angleToPlayer.lastIndex);
            spriteTransform.LookAt(playerTransform);

            if(spriteRender.sprite.name == "cowboy1fire1_4")
            { //youch!
                
            }
            
            yield return new WaitForSeconds(3f);

        }
        else
        {
            attackRange = ogRange;
            StartCoroutine(MoveTowardsPlayer());
        }
    }


    private void RetrackTarget()
    {
        Quaternion rotTarget = Quaternion.LookRotation(playerTransform.position - enemyTransform.transform.position);
        
        enemyTransform.transform.rotation = Quaternion.RotateTowards(enemyTransform.transform.rotation, rotTarget, rotSpeed * Time.deltaTime);
    }

    private bool InAttackRange()
    {

            if(Physics.CheckSphere(enemyTransform.position, attackRange, whatIsPlayer))
            {
                attackPlayer = true;
                return true;
            }
            else
            {
                chasePlayer = true;
                return false;
            }
     
    }



}

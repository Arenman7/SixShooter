using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class OldManEnemy : MonoBehaviour
{
    [Header("Components")]
    Animator spriteAnimator;
    AngleToPlayer angleToPlayer;
    Transform spriteTransform;
    Transform playerTransform;
    Transform enemyTransform;
    Enemy enemy;
    NavMeshAgent navMeshAgent;
    EnemyAISensor enemyAISensor;
    AudioSource audioSource;
    Transform attackPoint;

    [Header("Bools")]
    public bool idling=true;
    public bool walking;
    public bool attacking;
    public bool isHit;
    public bool isDead;
    public bool seenPlayer;
    public bool canAttack=true;

    [Header("Enemy Settings")]
    public float speed=8;
    public float rotSpeed=120;
    public float attackRange=2f;
    public float stoppingDistance=5f;
    public LayerMask playerLayer;

    [Header("Misc")]
    public AudioClip enemyAttackClip;
    public AudioClip enemyHurtClip1;
    public AudioClip enemyHurtClip2;
    public AudioClip enemyHurtClip3;
    public AudioClip enemyDeathClip;
    public UnityEvent damagePlayer;

    
    void Start()
    {
        enemyTransform = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        enemyAISensor = GetComponent<EnemyAISensor>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        spriteAnimator = GetComponentInChildren<Animator>();
        angleToPlayer = GetComponentInChildren<AngleToPlayer>();
        spriteTransform = GetComponentInChildren<Transform>();
        playerTransform = FindObjectOfType<PlayerHealth>().transform;
        enemy = GetComponent<Enemy>();
        SetNavMeshProps();

        attackPoint = transform.Find("Attack Point");
    }
    
    void Update()
    {
        HandleAttackDist();
        CheckForSightLine();
        CheckForState();

        if(idling) Idle();
        if(walking) Walk(); 
    }

    void SetNavMeshProps()
    {
        navMeshAgent.stoppingDistance = stoppingDistance;
        navMeshAgent.speed = speed;
        navMeshAgent.angularSpeed = rotSpeed;
    }

    void HandleAttackDist()
    {
        if (Vector3.Distance (enemy.transform.position, playerTransform.position) <= navMeshAgent.stoppingDistance && !attacking && canAttack) {
            transform.LookAt(playerTransform);
            StartCoroutine(BeginAttack());
        } 
    }

   

    void CheckForState()
    {
        if(isHit)
        {
            idling = false;
            walking = false;

            attacking = false;
        } 
        else if(seenPlayer && !attacking)
        {
            idling = false;
            walking = true;
        }
        if(attacking)
        {
            walking = false;
        }        
    }

    private void CheckForSightLine()
    {
        if(enemyAISensor.Objects.Count > 0)
        {
            seenPlayer = true;
        }
    }

    private void Idle()
    {
        spriteAnimator.Play("Idle");
        spriteAnimator.SetFloat("spriteRotation", angleToPlayer.lastIndex);
    }

    private void Walk()
    {
        navMeshAgent.SetDestination(playerTransform.position);
        spriteAnimator.Play("Walk");
        spriteAnimator.SetFloat("spriteRotation", angleToPlayer.lastIndex);
    }

    public void HitReaction()
    {
        seenPlayer = true;
        isHit = true;
        navMeshAgent.SetDestination(transform.position);

        int rand = Random.Range(0, 3);
        if(rand == 0)
        audioSource.PlayOneShot(enemyHurtClip1, Random.Range(0.5f, 1f));
        else if(rand == 1)
        audioSource.PlayOneShot(enemyHurtClip2, Random.Range(0.5f, 1f));
        else
        audioSource.PlayOneShot(enemyHurtClip3, Random.Range(0.5f, 1f));
        spriteAnimator.Play("Hit");
        spriteAnimator.SetFloat("spriteRotation", angleToPlayer.lastIndex);
        Invoke(nameof(DelayHitReset), .5f);
    }
    void DelayHitReset()
    {
        isHit = false;
    }

    private IEnumerator BeginAttack()
    {
        canAttack = false;
        attacking = true;
        spriteAnimator.Play("Attack");
        spriteTransform.LookAt(playerTransform);
        yield return new WaitForSeconds(0.7f);
        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
        foreach(Collider player in hitPlayer)
        {
            if(player.CompareTag("Player"))
            {
                audioSource.PlayOneShot(enemyAttackClip, Random.Range(0.5f, 1f));
                damagePlayer.Invoke();
            }
        }
        attacking = false;
        yield return new WaitForSeconds(2f);
        canAttack = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(attackPoint)
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    


    public void Dead()
    {
        isDead = true;
        audioSource.PlayOneShot(enemyDeathClip, Random.Range(0.5f, 1f));
        spriteAnimator.Play("Death");
        if(spriteTransform)
        {
            spriteTransform.LookAt(playerTransform);
        }
        enabled = false;
    }

    

}

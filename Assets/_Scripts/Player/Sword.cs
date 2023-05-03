using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public LayerMask raycastLayerMask;
    public LayerMask enemyLayerMask;

    public BoxCollider swordTrigger;

    Enemy enemy;

    public float range = 3f;
    public float vertRange = 3f;
    public float timeDamageOccursFor = .2f;

    bool parryAttack;
    public int damage = 1;
    public int parryDamage = 2;

    GameObject pistolObj;
    Pistol pistolScript;

    private float nextTimeToSwing;
    [SerializeField] private float swingRate = 1f;


    bool hasRecentlyShot;
    public bool hasRecentlySwung;
    public float delayFireandMelee = .3f;

    public int attackCount;
    float comboWindowTime;
    public float maxWindowTime=1.5f;

    AudioSource audioSource;
    public AudioClip swingSoundClip1,swingSoundClip2,swingSoundClip3,parryClip,hitClip;

    bool beginTimer;
    public bool holstered;
    public bool quickDrawed;
    Animator anim;

    int originalDamage;

    void Start()
    {
        anim = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        pistolObj = GameObject.FindGameObjectWithTag("Pistol");
        if(pistolObj!=null) 
        {
            pistolScript = pistolObj.GetComponent<Pistol>();
        }

        swordTrigger = GetComponent<BoxCollider>();    
        swordTrigger.size = new Vector3(range, vertRange, range);
        swordTrigger.center = new Vector3(0, 0, range * 0.5f);
        swordTrigger.enabled = false;

        comboWindowTime = maxWindowTime;

        originalDamage = damage;
    }

    
    void Update()
    {
        HandleSwinging();
        HandleCombo();
        HandleHolster();

        //FIX LATER
        
        
    }
    

    private void Swing()
    {
        comboWindowTime = maxWindowTime; //combo
        hasRecentlySwung=true;
        attackCount++;
        if(attackCount > 3) attackCount = 1;
        

        swordTrigger.enabled = true;


        if(attackCount == 1)
        {
            audioSource.PlayOneShot(swingSoundClip1, .5f);
            anim.CrossFade("Swing1", .1f, 0);

        }
        else if(attackCount == 2)
        {
            audioSource.PlayOneShot(swingSoundClip2, .5f);
            anim.CrossFade("Swing2", .5f, 0);
        }
        else if(attackCount == 3)
        {
            audioSource.PlayOneShot(swingSoundClip3, .5f);
            anim.CrossFade("Swing3", .5f, 0);

        }

        nextTimeToSwing = Time.time + swingRate;
        

        Invoke(nameof(HasRecentlySwung), delayFireandMelee);
        Invoke(nameof(DelayDisableTrigger), timeDamageOccursFor); 
    }

    private void HandleCombo()
    {
        if(hasRecentlySwung)
        {   
            beginTimer = true;   
            
        }
        if(beginTimer)
        {
            if(comboWindowTime > 0)  
            {
                comboWindowTime -= Time.deltaTime;

                if(Input.GetMouseButtonDown(1) && comboWindowTime < maxWindowTime - swingRate)
                {
                    attackCount++;
                }
            }
            if(comboWindowTime < 0)
            {
                beginTimer = false;
                attackCount = 0;
            }
        }
    }

    void HandleHolster()
    {
        if(attackCount == 0 && !holstered)
        {
            anim.CrossFade("SwordIdle", 0.2f, 0);
        }

        if(attackCount == 0 && Input.GetKeyDown(KeyCode.H))
        {
            holstered = !holstered;
            
        }

        if(attackCount == 0 && holstered && !quickDrawed)
        {
            anim.CrossFade("holsterSword",0,0);
        }

        if(attackCount == 0 && !holstered)
        {
            anim.CrossFade("SwordIdle", 0.2f, 0);
        }

        if(holstered && Input.GetMouseButtonDown(1) && !hasRecentlyShot && !quickDrawed)
        { //quickDraw feature, make collider bigger
            quickDrawed = true;
            hasRecentlySwung = true;
            Debug.Log("Quick Draw!");

            damage = damage * 3;
            swordTrigger.enabled = true;
            audioSource.PlayOneShot(parryClip, .5f);

            anim.CrossFade("SwordQuickDraw",0,0);
            
            Invoke(nameof(ResetQuickDraw), 1.5f);

            nextTimeToSwing = Time.time + swingRate;
        

            Invoke(nameof(HasRecentlySwung), delayFireandMelee);
            Invoke(nameof(DelayDisableTrigger), timeDamageOccursFor); 
        }
    }

    private void HandleSwinging()
    {  //change getmosuebuttondown, to getmousebutton to be able to hold swing.
        if(Input.GetMouseButtonDown(1) && Time.time > nextTimeToSwing && !hasRecentlyShot && !holstered)
        {
            Swing();
            
        }        


        
    }

    private void ResetQuickDraw()
    {
        quickDrawed = false;
        holstered = false;
    }

    private void HasRecentlySwung()
    {
        hasRecentlySwung = false;
    }

    private void DelayDisableTrigger()
    {
        swordTrigger.enabled = false;
        damage = originalDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        enemy = other.GetComponent<Enemy>();
        if(other.CompareTag("Enemy"))
        {
            if(parryAttack)
            {
                enemy.TakeDamage(parryDamage);
            }
            else
            enemy.TakeDamage(damage);
        
        }

    }
    
    
    
}
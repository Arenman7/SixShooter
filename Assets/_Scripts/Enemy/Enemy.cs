using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Enemy : MonoBehaviour
{ 
    public int enemyHealth = 3;
    
    public GameObject gunHitEffect;
    public GameObject deathEffect;

    //EnemyAwareness enemyAwareness;
    public UnityEvent isShot;
    public UnityEvent isDead;
    public static event Action OnShot;
    public static event Action OnDeath;

    void Start()
    {  
        //enemyAwareness = GetComponent<EnemyAwareness>();
    }

    
    void Update()
    {
        if(enemyHealth <= 0)
        {
            enabled = false;
            IsDead();
        }
    }

    public void TakeDamage(int damage)
    {
        Instantiate(gunHitEffect, transform.position, Quaternion.identity);
        enemyHealth -= damage;
        isShot.Invoke();
        OnShot?.Invoke();
        //StartCoroutine(enemyAwareness.WasShot());
    }

    private void IsDead()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        isDead.Invoke();
        OnDeath?.Invoke();
        
    }
}

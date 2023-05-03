using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int health;

    [SerializeField] private int maxArmor = 50;
    public int armor;

    public AudioSource audioSource;
    public AudioClip[] healthPickupClip;
    public AudioClip[] armorPickupClip;

    public bool isDead;
    public bool tookDamage;
    
    public static event Action PlayerHit;
    

    [Header("Hurt Screen")]
    public Image hurtImage;
    public Image pickupBlipImage;

    [Header("Pickup Blip")]
    public float timeToReturnBlip=0.1f;
    public float blipAlpha=0.1f;

    void Start()
    {
        isDead = false;
        health = maxHealth;
        armor = 0;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    private void OnEnable()
    {
        KeyPickup.OnPickupKey += StartPickupBlip;
        Pistol.OnPickupAmmo += StartPickupBlip;

    }

    private void OnDisable()
    {
        KeyPickup.OnPickupKey -= StartPickupBlip;
        Pistol.OnPickupAmmo -= StartPickupBlip;
    }

    void HandleHurtScreenCheck()
    {
        if(health > 80 && health <= 100)
        {
            Color color = hurtImage.color;
            color.a = .2f;
            hurtImage.color = color;
            StartCoroutine(FadeOutHurtScreen());
        }
        else if(health > 60 && health <= 80)
        {
            Color color = hurtImage.color;
            color.a = .4f;
            hurtImage.color = color;
            StartCoroutine(FadeOutHurtScreen());
        }
        else if(health > 40 && health <= 60)
        {
            Color color = hurtImage.color;
            color.a = .6f;
            hurtImage.color = color;
            StartCoroutine(FadeOutHurtScreen());
        }
        else if(health > 20 && health <= 40)
        {
            Color color = hurtImage.color;
            color.a = .8f;
            hurtImage.color = color;
            StartCoroutine(FadeOutHurtScreen());
        }
        else if(health <= 20)
        {
            Color color = hurtImage.color;
            color.a = 1f;
            hurtImage.color = color;
            StartCoroutine(FadeOutHurtScreen());
        }
        
    }

    private IEnumerator FadeOutHurtScreen()
    {
        Color from = hurtImage.color;
        Color to = new Color(hurtImage.color.r, hurtImage.color.g, hurtImage.color.b, 0f);
        float t = 0f;
        while(t<3f)
        {
            hurtImage.color = Color.Lerp(from, to, t / 3f);
            t += Time.deltaTime;
            yield return null;
        }
    }
    void StartPickupBlip()
    {
        StartCoroutine(PickupBlip());
    }

    private IEnumerator PickupBlip()
    {
        Color from = pickupBlipImage.color;
        Color to = new Color(pickupBlipImage.color.r, pickupBlipImage.color.g, pickupBlipImage.color.b, blipAlpha);
        float t = 0f;
        while(t<timeToReturnBlip)
        {
            pickupBlipImage.color = Color.Lerp(from, to, t / timeToReturnBlip);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        from = pickupBlipImage.color;
        to = new Color(pickupBlipImage.color.r, pickupBlipImage.color.g, pickupBlipImage.color.b, 0f);
        while(t<timeToReturnBlip)
        {
            pickupBlipImage.color = Color.Lerp(from, to, t / timeToReturnBlip);
            t += Time.deltaTime;
            yield return null;
        }

        Color color = pickupBlipImage.color;
        color.a = 0f;
        pickupBlipImage.color = color;

    }

    public void DamagePlayer(int damage)
    {   
        PlayerHit?.Invoke();
        tookDamage = true;
        
        if(armor > 0)
        {
            if(armor >= damage)
            {
                armor -= damage;
            }
            else if(armor < damage)
            {
                int remainingDamage;
                remainingDamage = damage - armor;
                
                armor = 0;
                
                health -= remainingDamage;
            }
        }
        else
        {
            health -= damage;
        }
        if(health <= 0)
        {
            PlayerDied();
        }

        HandleHurtScreenCheck();
    }

    private void PlayerDied()
    {
        isDead = true;
    }

    public void GiveHealth(int amount, GameObject pickup)
    {
        if(health < maxHealth)
        {
            StartCoroutine(PickupBlip());
            if(amount == 2)
            audioSource.PlayOneShot(healthPickupClip[0], UnityEngine.Random.Range(.8f, 1f));
            else if(amount == 10)
            audioSource.PlayOneShot(healthPickupClip[1], UnityEngine.Random.Range(.1f, .2f));
            else
            audioSource.PlayOneShot(healthPickupClip[2], UnityEngine.Random.Range(.2f, .4f));
            health += amount;
            Destroy(pickup);
        }

        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void GiveArmor(int amount, GameObject pickup)
    {
        if(armor < maxArmor)
        {
            StartCoroutine(PickupBlip());
            if(amount == 2)
            audioSource.PlayOneShot(armorPickupClip[0], UnityEngine.Random.Range(.2f, .4f));
            else if(amount == 10)
            audioSource.PlayOneShot(armorPickupClip[1], UnityEngine.Random.Range(.05f, .1f));
            else
            audioSource.PlayOneShot(armorPickupClip[2], UnityEngine.Random.Range(.05f, .1f));
            armor += amount;
            Destroy(pickup);
        }

        if(armor > maxArmor)
        {
            armor = maxArmor;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KeyPickup : MonoBehaviour
{
    public bool isGreenKey, isBlueKey, isRedKey;
    AudioSource keyAudioSource;
    public AudioClip pickupKeyClip;
    PlayerHealth playerHealth;

    public static event Action OnPickupKey;

    void OnTriggerEnter(Collider other)
    {
        OnPickupKey?.Invoke();
        if(isGreenKey)
        {
            other.GetComponent<PlayerInventory>().hasGreenKey = true;
        }
        
        if(isBlueKey)
        {
            other.GetComponent<PlayerInventory>().hasBlueKey = true;
        }
        
        if(isRedKey)
        {
            other.GetComponent<PlayerInventory>().hasRedKey = true;
        }
        
        other.GetComponent<PlayerInventory>().audioSource.PlayOneShot(pickupKeyClip, UnityEngine.Random.Range(0.2f,0.4f));

        Destroy(this.gameObject);
    }

}

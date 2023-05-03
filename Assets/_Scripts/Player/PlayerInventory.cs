using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public bool hasGreenKey, hasBlueKey, hasRedKey;
    
    public AudioSource audioSource;

    public Image greenKey, blueKey, redKey;


    void Update()
    {
        if(hasGreenKey) greenKey.gameObject.SetActive(true);
        if(hasBlueKey) blueKey.gameObject.SetActive(true);
        if(hasRedKey) redKey.gameObject.SetActive(true);


    }

}

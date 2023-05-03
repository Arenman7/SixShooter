using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI health, armor, ammo; 
    public PlayerHealth playerHealth;
    PlayerInventory playerInventory;
    public Pistol pistol;

    public bool greenKeyOnLevel, blueKeyOnLevel, redKeyOnLevel;
    public Image greenContainer, blueContainer, redContainer;

    void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();

    }

    // Update is called once per frame
    void Update()
    {
        health.text = playerHealth.health.ToString();
        armor.text = playerHealth.armor.ToString();
        ammo.text = pistol.currentMag.ToString() + "/" + pistol.currentAmmo.ToString();
    
        HandleDisplayKey();
    }

    void HandleDisplayKey()
    {
        if(greenKeyOnLevel) greenContainer.gameObject.SetActive(true);
        if(blueKeyOnLevel) blueContainer.gameObject.SetActive(true);
        if(redKeyOnLevel) redContainer.gameObject.SetActive(true);

    }


}

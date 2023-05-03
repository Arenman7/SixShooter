using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator doorAnim;
    public GameObject areaToSpawn;

    public bool shouldSpawn;
    public bool requiresKey;
    public bool reqGreenKey, reqBlueKey, reqRedKey;

    public bool enemyCanOpen;
    bool greenOpened, blueOpened, redOpened;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(requiresKey)
            {
                if(reqGreenKey && other.GetComponent<PlayerInventory>().hasGreenKey)
                {
                    greenOpened=true;
                    BeginOpenDoor();
                }
                if(reqBlueKey && other.GetComponent<PlayerInventory>().hasBlueKey)
                {
                    blueOpened=true;
                    BeginOpenDoor();
                }
                if(reqRedKey && other.GetComponent<PlayerInventory>().hasRedKey)
                {
                    redOpened=true;
                    BeginOpenDoor();
                }
            }
            else
            {
                BeginOpenDoor();
            }
        }
        if(other.CompareTag("Enemy"))
        {
            if(enemyCanOpen && reqGreenKey && greenOpened)
            {
                BeginOpenDoor();
            }
            if(enemyCanOpen && reqBlueKey && blueOpened)
            {
                BeginOpenDoor();
            }
            if(enemyCanOpen && reqRedKey && redOpened)
            {
                BeginOpenDoor();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {   
        BeginCloseDoor();
    }
    
    void BeginOpenDoor()
    {
        doorAnim.SetBool("CloseDoor", false);
        doorAnim.SetBool("OpenDoor", true);

        if(shouldSpawn)
        {
            areaToSpawn.SetActive(true);
        }
    }

    void BeginCloseDoor()
    {
        doorAnim.SetBool("OpenDoor", false);
        doorAnim.SetBool("CloseDoor", true);
    }
}

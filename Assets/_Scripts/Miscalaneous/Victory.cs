using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public GameObject victoryText;

    void OnTriggerEnter()
    {
        victoryText.SetActive(true);
        FindObjectOfType<AudioManager>().PlayClip("VictorySound");
    }
}

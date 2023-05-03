using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform target;

    void Start()
    {
        target = FindObjectOfType<PlayerMove>().transform;
    }

    void Update()
    {
        transform.rotation = target.rotation;
    }
}

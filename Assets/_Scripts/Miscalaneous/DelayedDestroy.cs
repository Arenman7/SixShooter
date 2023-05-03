using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
    public float timeToDestroy=2f;

    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public bool isAggro;
    public bool wasShot;

    private Transform playerTransform;

    public float awarenessRadius = 8f;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        HandleAwareness();

    }

    private void HandleAwareness()
    {
        
    }

    public IEnumerator WasShot()
    {
        wasShot = true;
        yield return new WaitForSeconds(0.2f);
        wasShot = false;
    }

    public IEnumerator WasAggroed()
    {
        isAggro = true;
        yield return new WaitForSeconds(0.2f);
        isAggro = false;
    }

    
    

}

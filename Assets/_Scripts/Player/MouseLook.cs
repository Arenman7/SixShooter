using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{   
    [SerializeField] private float sensitivity = 1.5f;
    [SerializeField] private float smoothingAmt = 1.5f;

    private float xMousePos;
    private float smoothedMousePos;
    private float currentLookPos;

    [HideInInspector] public bool isPaused;
    void Start()
    {
        HandleCursor();
    }
    
    void Update()
    {
        if(!isPaused)
        {
            GetInput();
            ModifyInput();
            MovePlayer();
        }
    }

    void GetInput()
    {
        xMousePos = Input.GetAxisRaw("Mouse X");
    }

    void ModifyInput()
    {
        xMousePos *= sensitivity * smoothingAmt;
        smoothedMousePos = Mathf.Lerp(smoothedMousePos, xMousePos, 1f/smoothingAmt); 
    }

    void MovePlayer()
    {
        currentLookPos += smoothedMousePos;
        transform.localRotation = Quaternion.AngleAxis(currentLookPos, transform.up);
    }

    void HandleCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}

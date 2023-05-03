using UnityEngine;
using System;

public class CameraLean : MonoBehaviour
{
    public PlayerMove playerMove;
    public Transform pivot;
    public float leanAngle = 3f;
    public float leanSpeed = 4f;

    private Quaternion centerRotation;
    public Animator animator;
    
    public static event Action OnStep;

    void Start()
    {
        centerRotation = pivot.localRotation;
    }
    
    void Update()
    {
        HandleLeans();
        HandleWalkBob();
    }
    void SendStep()
    {
        OnStep?.Invoke();
    }

    void HandleWalkBob()
    {
        if(playerMove.isWalkingBackwards || playerMove.isWalkingLeft || playerMove.isWalkingRight || playerMove.isWalkingForward)
        {
            animator.CrossFade("WalkingHeadBob", 0f);
        }
        else
        {
            animator.CrossFade("IdleHeadBob", 0f);
        }
    }
    
    void HandleLeans()
    {
        if (playerMove.isWalkingForward && playerMove.isWalkingLeft)
        {
            Quaternion targetRotation = Quaternion.Euler(leanAngle, centerRotation.eulerAngles.y, leanAngle);
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, targetRotation, leanSpeed * Time.deltaTime);
        }
        else if (playerMove.isWalkingForward && playerMove.isWalkingRight)
        {
            Quaternion targetRotation = Quaternion.Euler(leanAngle, centerRotation.eulerAngles.y, -leanAngle);
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, targetRotation, leanSpeed * Time.deltaTime);
        }
        else if (playerMove.isWalkingBackwards && playerMove.isWalkingLeft)
        {
            Quaternion targetRotation = Quaternion.Euler(-leanAngle, centerRotation.eulerAngles.y, leanAngle);
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, targetRotation, leanSpeed * Time.deltaTime);
        }
        else if (playerMove.isWalkingBackwards && playerMove.isWalkingRight)
        {
            Quaternion targetRotation = Quaternion.Euler(-leanAngle, centerRotation.eulerAngles.y, -leanAngle);
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, targetRotation, leanSpeed * Time.deltaTime);
        }
        else if (playerMove.isWalkingForward)
        {
            Quaternion targetRotation = Quaternion.Euler(leanAngle, centerRotation.eulerAngles.y, centerRotation.eulerAngles.z);
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, targetRotation, leanSpeed * Time.deltaTime);
        }
        else if (playerMove.isWalkingBackwards)
        {
            Quaternion targetRotation = Quaternion.Euler(-leanAngle, centerRotation.eulerAngles.y, centerRotation.eulerAngles.z);
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, targetRotation, leanSpeed * Time.deltaTime);
        }
        else if (playerMove.isWalkingLeft)
        {
            Quaternion targetRotation = Quaternion.Euler(centerRotation.eulerAngles.x, centerRotation.eulerAngles.y, leanAngle);
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, targetRotation, leanSpeed * Time.deltaTime);
        }
        else if (playerMove.isWalkingRight)
        {
            Quaternion targetRotation = Quaternion.Euler(centerRotation.eulerAngles.x, centerRotation.eulerAngles.y, -leanAngle);
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, targetRotation, leanSpeed * Time.deltaTime);
        }
        else
        {
            Quaternion targetRotation = centerRotation;
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, targetRotation, leanSpeed * Time.deltaTime);
        }
    }

}

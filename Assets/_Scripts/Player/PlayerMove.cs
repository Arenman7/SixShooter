using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;

    [HideInInspector] public bool isPaused; 
    
    public bool isWalkingForward;
    public bool isWalkingBackwards;
    public bool isWalkingLeft;
    public bool isWalkingRight;

    private Vector3 inputVector;
    private Vector3 directionVector;
    
    private Vector3 movementVector;
    
    private float gravity = -10f;

    [SerializeField] private float playerSpeed = 10f;

    void Start()
    {
        characterController = GetComponent<CharacterController>(); //gets character controller
    }

    void Update()
    {
        if(!isPaused)
        {
            GetInput();
            MovePlayer();
            CheckForInputDirection();
        }    
    }

    void GetInput()
    {
        directionVector = inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")); //stores input using unity's old system, may update later.

        inputVector.Normalize(); //so player doesn't move 2x in diag
        inputVector = transform.TransformDirection(inputVector); //moves in dir facing
        
        movementVector = (inputVector * playerSpeed) + (Vector3.up * gravity); 
    }

    void MovePlayer()
    {
        characterController.Move(movementVector * Time.deltaTime);
    }

    void CheckForInputDirection()
    {
        if(directionVector.x >= 0.1f){
            isWalkingRight = true;
            }
            
        else{
            isWalkingRight = false;}
        if(directionVector.x <= -0.1f){
            isWalkingLeft = true;
            }
        else{
            isWalkingLeft = false;}
        if(directionVector.z >= 0.1f){
            isWalkingForward = true;
            }
        else{
            isWalkingForward = false;}
        if(directionVector.z <= -0.1f){
            isWalkingBackwards = true;
            }
        else{
            isWalkingBackwards = false;}
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float crouchSpeed = 2f;
    [SerializeField] private float jumpPower = 5f;
    private float speed;

    // Input vectors
    private Vector2 moveVector, lookVector;

    // Movement states
    private bool sprinting, crouching, jumping;

    // Collision flags
    private bool grounded, ableToStand;

    [Header("Camera")]
    [SerializeField] private Transform cameraHolder;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    #region Update Functions

    private void Update()
    {
        GetMovementSpeed();
    }

    private void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        
    }

    #endregion


    #region Input Callbacks

    public void OnMove(InputAction.CallbackContext cxt)
    {
        moveVector = cxt.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext cxt)
    {
        lookVector = cxt.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext cxt)
    {
        // Add toggle functionality <-- Need reference to game settings

        sprinting = cxt.ReadValue<float>() == 1f;
    }

    public void OnCrouch(InputAction.CallbackContext cxt)
    {
        // Add toggle functionality <-- Need reference to game settings 

        crouching = cxt.ReadValue<float>() == 1f;

        if (crouching && !sprinting && grounded) Crouch();
    }

    public void OnJump(InputAction.CallbackContext cxt)
    {
        jumping = cxt.ReadValue<float>() == 1f;

        if (jumping && grounded && ableToStand) Jump();
    }

    #endregion


    #region Movement Functions

    private void Move()
    {
        
    }

    private void GetMovementSpeed()
    {
        // Function evaluates what type of movement the player is trying to perform and updates 
        // the movement speed accordingly

        if (sprinting && grounded && ableToStand) speed = sprintSpeed;
        else if (crouching && !sprinting && grounded) speed = crouchSpeed;
        else speed = walkSpeed;
    }

    private void Crouch()
    {

    }

    private void Jump()
    {

    }

    #endregion


    #region Camera Functions

    private void Look()
    {

    }

    private void TweenFOV()
    {

    }

    #endregion

}

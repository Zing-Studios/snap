using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float airSpeed;
    [SerializeField] private float jumpMagnitude;
    [SerializeField] private float maxForce;
    private float speed;
    private bool sprinting, crouching, grounded, inCrouch;
    private Vector2 moveVector, lookVector;
    private RaycastHit slopeHit;

    [Header("Colliders")]
    [SerializeField] private CapsuleCollider standingCollider;
    [SerializeField] private CapsuleCollider crouchingCollider;

    [Header("Camera Settings")]
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float sensitivity;
    [SerializeField] private float xRotationLimit;
    private float lookRotation;
    private float sprintFov;

    [Header("Head Bobbing")]
    [SerializeField] private Transform itemHolder;
    [SerializeField, Range(0f, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0f, 30f)] private float frequency = 10f;
    private Vector3 startPos;

    // Miscellaneous
    private float playerHeight = 2f;
    private Rigidbody rb;

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
        // Add ability to toggle sprint

        sprinting = cxt.ReadValue<float>() == 1f && canStandUp();

        if (inCrouch && sprinting) ToggleCrouch();
    }

    public void OnCrouch(InputAction.CallbackContext cxt)
    {
        crouching = cxt.ReadValue<float>() == 1f;

        if (crouching && cxt.started) ToggleCrouch();
    }

    public void OnJump(InputAction.CallbackContext cxt)
    {
        Jump();
    } 

    #endregion

    #region Update Functions

    private void Update()
    {
        // Movement functions
        GetMovementSpeed();
        TweenFOV();

        // Head bobbing
        ResetPosition();
        if (rb.velocity.magnitude > 0.1f) PlayMotion(FootStepMotion());

        // Miscellaneous
        playerHeight = crouching ? 2f : 1f;
        rb.drag = grounded ? 10f : 0.5f;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        Look();
    }

    #endregion

    #region Start Functions

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;

        sprintFov = playerCamera.m_Lens.FieldOfView + 5f;
        startPos = itemHolder.transform.localPosition;
    }

    #endregion

    #region Movement Functions

    private void GetMovementSpeed()
    {
        if (sprinting && grounded) speed = sprintSpeed;
        else if (inCrouch) speed = crouchSpeed;
        else if (grounded) speed = walkSpeed;
        else speed = airSpeed;
    }
    private void Move()
    {
        Vector3 moveDirection = (transform.forward * moveVector.y) + (transform.right * moveVector.x);

        if (OnSlope()) moveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        rb.AddForce(moveDirection * speed, ForceMode.Impulse);
    }

    private void Jump()
    {
        if (!grounded) return;

         Vector3 jumpForce = Vector3.up * jumpMagnitude;

        rb.AddForce(jumpForce, ForceMode.Impulse);
    }

    private void ToggleCrouch()
    {
        if (inCrouch && !canStandUp()) return;

        inCrouch = !inCrouch;

        crouchingCollider.enabled = inCrouch;
        standingCollider.enabled = !inCrouch;

        if (inCrouch) cameraHolder.transform.DOLocalMove(new Vector3(0f, 0.75f, 0f), 0.2f).SetEase(Ease.InOutFlash);
        else cameraHolder.transform.DOLocalMove(new Vector3(0f, 1.5f, 0f), 0.2f).SetEase(Ease.InOutFlash);
    }

    private void PlayMotion(Vector3 motion)
    {
        itemHolder.transform.localPosition += motion;
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * speed * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * speed * frequency / 2f) * amplitude * 2f;
        return pos;
    }

    private void ResetPosition()
    {
        if (itemHolder.transform.localPosition == startPos) return;
        itemHolder.transform.localPosition = Vector3.Lerp(itemHolder.transform.localPosition, startPos, Time.deltaTime);
    }

    #endregion

    #region Camera Functions

    private void Look()
    {
        // Turn camera
        transform.Rotate(Vector3.up * lookVector.x * sensitivity);

        // Look
        lookRotation += (-lookVector.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -xRotationLimit, xRotationLimit);

        // Apply rotation to camera holder
        cameraHolder.transform.eulerAngles = new Vector3(lookRotation, cameraHolder.eulerAngles.y, cameraHolder.eulerAngles.z);
    }

    private void TweenFOV()
    {
        float fov = playerCamera.m_Lens.FieldOfView;

        if (sprinting && rb.velocity.magnitude > 0.1f) playerCamera.m_Lens.FieldOfView = Mathf.Lerp(fov, sprintFov, Time.deltaTime * 2f);
        else playerCamera.m_Lens.FieldOfView = Mathf.Lerp(fov, sprintFov - 5f, Time.deltaTime * 2f);
    }

    #endregion

    #region Miscellaneous

    public void SetGrounded(bool state)
    {
        grounded = state;
    }

    private bool canStandUp()
    {
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit standUpHit, playerHeight + 0.25f, 7)) return false;
        else return true;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.25f))
        {
            if (slopeHit.normal != Vector3.up) return true;
            else return false;

        }
        return false;
    }

    #endregion
}

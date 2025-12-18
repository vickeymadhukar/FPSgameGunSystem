using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerr : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;

    [Header("Ground Check Settings")]
    [SerializeField] Transform groundCheck;   // ✅ like in your Movement script
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;

    [Header("Jump Settings")]
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float gravity = -9.81f;

    bool isGrounded;
    Quaternion targetRotation;
    Vector3 velocity; // ✅ for gravity/jump

    CameraController cameraController;
    Animator animator;
    CharacterController characterController;

    // New Input System
    public PlayerController inputActions;
    Vector2 moveInput;

    bool hasControll;
    private void Awake()
    {
        if (Camera.main != null)
            cameraController = Camera.main.GetComponent<CameraController>();

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        inputActions = new PlayerController();
      hasControll = true;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    public void setcontroll(bool hasControll)
    {
        this.hasControll = hasControll;
        characterController.enabled = hasControll;
        if (!hasControll)
        {
            animator.SetFloat("moveAmount",0f);
           
        }
    }


    private void Update()
    {
        float h = moveInput.x;
        float v = moveInput.y;
        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        var moveDir = cameraController != null
            ? cameraController.PlanarRotation * new Vector3(h, 0, v)
            : new Vector3(h, 0, v);

        var finalMove = moveDir * moveSpeed;
        characterController.Move((finalMove + velocity) * Time.deltaTime);







        if (!hasControll)
        {
            return;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (characterController.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (inputActions.Player.Jump.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("Jump Pressed");
        }
        velocity.y += gravity * Time.deltaTime;

        
     

        // Rotate towards move direction
        if (moveAmount > 0)
            targetRotation = Quaternion.LookRotation(moveDir);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);

        // Animator
        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
       
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
}

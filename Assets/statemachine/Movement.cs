using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6f;
    public float gravity = -9.81f;
    public float CrouchSpeed = 4f;
    public float ProneSpeed = 4f;
    public float walkspeed = 6f;
    // Gravity / Jump
    public LayerMask ground;
    public float checksphere = 0.4f;
    public Transform groundcheck;
    public float jumpheight = 10f;

    [Header("References")]
    public CharacterController controller;
    public Animator animator;

    private Vector3 velocity;
    [HideInInspector] public Vector2 inputMovement;

    private PlayerController playerInputActions;

    MovementBase currentstate;
    public Idelstate idel = new Idelstate();
    public Movingstate movingstate = new Movingstate();
    public Crouchstate crouchstate = new Crouchstate();
    public Pronestate pronestate = new Pronestate();

 //   public MultiAimConstraint aimConstraint; // assign in inspector
    [SerializeField] public Rig rig;

    public bool isAim = false;
    public bool isCrouching = false;
    public bool isProne = false;
    
    public bool Hascontrol = true;

   public bool CanJump = true;



    public Pistalpos pis;

    private void Awake()
    {
        playerInputActions = new PlayerController();
      
        pis = GetComponent<Pistalpos>();
        
    }

    private void Start()
    {
        SwitchState(idel);
       /// aimConstraint.weight = 0f;
        Hascontrol = true;
        CanJump = true;
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();

        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;

        playerInputActions.Player.crouch.performed += OnCrouch;
        playerInputActions.Player.prone.performed += OnProne;

       // playerInputActions.Player.Shoot.performed += OnAimStart;
      //  playerInputActions.Player.Shoot.canceled += OnAimCancel;



    }

    private void OnDisable()
    {
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Move.canceled -= OnMove;

        playerInputActions.Player.crouch.performed -= OnCrouch;
        playerInputActions.Player.prone.performed -= OnProne;

        //playerInputActions.Player.Shoot.performed -= OnAimStart;
        //playerInputActions.Player.Shoot.canceled -= OnAimCancel;

        playerInputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            if (isProne)
            {
                isProne = false;
                isCrouching = true;
            }
            else
            {
                isCrouching = !isCrouching;
            }

         
            animator.SetBool("crouch", isCrouching);
        }
        
    }


    private void OnProne(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isCrouching)
            {
                isCrouching = false;
                isProne = true;
            }
            else
            {
                isProne = !isProne;
            }

            
            animator.SetBool("prone", isProne);
        }
    }


/*
    private void OnAimStart(InputAction.CallbackContext context)
    {
        

    }


    

    private void OnAimCancel(InputAction.CallbackContext context)
    {
       
       
    }
*/
    public void SwitchState(MovementBase newState)
    {
        
        if (currentstate != null)
        {
            
            currentstate.ExitState(this);
        }

        
        currentstate = newState;

        currentstate.EnterState(this);
    }

    public void setControll(bool Hascontrol)
    {
        this.Hascontrol = Hascontrol;
        controller.enabled = Hascontrol;
        
        if (!Hascontrol)
        {
            animator.applyRootMotion = true;
        }
        else
        {
            animator.applyRootMotion = false;
        }
    }

    void Update()
    {
        
        currentstate.UpadteState(this);

        // Movement
        Vector3 move = transform.right * inputMovement.x + transform.forward * inputMovement.y;
        controller.Move(move * speed * Time.deltaTime);

        // Set Animator floats
        animator.SetFloat("VelocityX", inputMovement.x);
        animator.SetFloat("VelocityY", inputMovement.y);

        if (isCrouching)
            speed = CrouchSpeed;
        else if (isProne)
            speed = ProneSpeed;
        else
            speed = walkspeed;

        if (!Hascontrol)
            return;


        bool isGrounded = Physics.CheckSphere(groundcheck.position, checksphere, ground);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (CanJump && playerInputActions.Player.Jump.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
            Debug.Log("jumppressed");
        }

        animator.SetBool("midair", !isGrounded);


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }



}

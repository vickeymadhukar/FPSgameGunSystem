using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
 
    private PlayerController playerinput;

    private CharacterController controller;
    
    [SerializeField]
    private float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;



    // gravitychechk
    public Transform groundcheck;
    bool isGrounded;
    Vector3 hightvelocity;
    public float groundcircle = 0.4f;
    
    public LayerMask ground;
    private float gravityValue = -9.81f;




    public Transform cam;
    
    //player rotation speed;
    public float smoothtime=0.1f;
    float rotatesmoothvelocity;



    [SerializeField] private Animator animator;


    private void Awake()
    {
        playerinput = new PlayerController();    
        controller = GetComponent<CharacterController>();
    }


    private void OnEnable()
    {
        playerinput.Enable();
    }


    private void OnDisable()
    {
        playerinput.Disable();
    }
    // Update is called once per frame
    void Update()
    {

        Vector2 Movement = playerinput.Player.Move.ReadValue<Vector2>();

        Vector3 direction = new Vector3(Movement.x, 0, Movement.y).normalized;



        animator.SetFloat("VelocityX", Movement.x); // 🚀 Left/Right
        animator.SetFloat("VelocityY", Movement.y); // 🚀 Forward/Backward



        if (Movement.magnitude > 0.1f ) 
        {
            float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg+cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref rotatesmoothvelocity, smoothtime);

            Vector3 moveDir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;

            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            controller.Move( moveDir.normalized * playerSpeed * Time.deltaTime);
            Debug.Log("char " + moveDir.normalized * playerSpeed * Time.deltaTime);

        }



        if(playerinput.Player.Jump.triggered && isGrounded)
        {
            Debug.Log("jumppressed");
            hightvelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }


        isGrounded = Physics.CheckSphere(groundcheck.position, groundcircle, ground);

        if(isGrounded && hightvelocity.y < 0f)
        {

              hightvelocity.y = -2f;
        }

        hightvelocity.y += gravityValue * Time.deltaTime;
        controller.Move(hightvelocity * Time.deltaTime);

    }



}

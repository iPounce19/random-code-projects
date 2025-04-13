using System;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Simple_TopDown_Player_Movement : MonoBehaviour
{
    private characterClassStatsAbilities classStatsAbilities;
    private unitComponent unitComponent;


    [Header("GameObject References")]
    public Rigidbody rb;
    public LayerMask whatIsGround;
    public Transform orientation;
    public Transform cam;
    public TextMeshProUGUI text_speed;
    public Animator animator;


    [Header("Character Checks")]
    public bool isGrounded;
    public bool isJump;
    public bool forcedMovement = false;
    public bool isMoving;

    [Header("Float Values")]
    public float playerHeight;
    public float playerGroundDrag = 10f;
    public float playerAirSpeed; // Penalty when player is in Air
    public float playerGroundSpeed; // Penalty when player is in Ground
    public float groundAfterJumpCheckCooldown;

    float horizontalInput;
    float verticalInput;
    float baseSpeed;
    float speedBonus;
    float jumpForce;
    float airGroundSpeed;
    private float _nextJumpTime;
    float jumpCooldownTime;
    float ySpeed;
    Vector3 gravitySpeed;
    
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (this.gameObject.GetComponent<characterClassStatsAbilities>() != null)
        {
            classStatsAbilities = this.gameObject.GetComponent<characterClassStatsAbilities>();
            Debug.Log("Class Stats Abilities Found!");
        }
        if (this.gameObject.GetComponent<unitComponent>() != null)
        {
            unitComponent = GetComponent<unitComponent>();
            Debug.Log("Unit Component Found!");
        }
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        initializing();
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // Checks if the Player is grounded

        if (isGrounded)
        {
            rb.linearDamping = playerGroundDrag;
            airGroundSpeed = playerGroundSpeed;
            if(Input.GetButtonDown("Jump") && !isJumpCoolingDown && !forcedMovement)
            {
                jump();
            }
            if(!isJump)
            {
                ySpeed = 0f;
            }
            animator.SetBool("isGrounded", true);

        }
        else
        {
            rb.linearDamping = 1f;
            airGroundSpeed = playerAirSpeed;
            animator.SetBool("isGrounded", false);
        }

        ySpeed += Physics.gravity.y * Time.deltaTime;
        gravitySpeed.y = ySpeed;
        rb.AddForce(gravitySpeed, ForceMode.Force); // Gravity
        if (!forcedMovement)
        {
            movement();
        }
        
    }

    private void initializing()
    {
        baseSpeed = classStatsAbilities.runSpeed;
        jumpForce = classStatsAbilities.jumpForce;
        forcedMovement = unitComponent.isForcedMovement;
        unitComponent.isMoving = isMoving;
        //jumpCooldownTime = classStatsAbilities.jumpCooldown;
    }

    private void movement()
    {
        Vector3 moveDirection = orientation.transform.right * horizontalInput + orientation.transform.forward * verticalInput;
        Vector3 velocity = moveDirection.normalized * (baseSpeed + speedBonus) * 666.66f;
        rb.AddForce(velocity * airGroundSpeed * Time.deltaTime, ForceMode.Force);
        if (velocity != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            isMoving = true;
        }
        else
        {
            isMoving = false;
            animator.SetBool("isMoving", false);
        }
        text_speed.SetText("Speed: " + Convert.ToInt32(rb.linearVelocity.magnitude));
    }

    private void jump()
    {
        rb.AddForce(Vector3.up * jumpForce * 333.33f, ForceMode.Force);
        // ySpeed = jumpForce * 333.33f;
        isJump = true;
        startJumpCooldown();
        Invoke("gravityAfterJumpCheck", groundAfterJumpCheckCooldown);
        animator.SetTrigger("isJumping");

        Debug.Log("Jump!");
    }
    void gravityAfterJumpCheck()
    {
        isJump = false;
    }
    public bool isJumpCoolingDown => Time.time < _nextJumpTime;
    public void startJumpCooldown() => _nextJumpTime = Time.time + jumpCooldownTime;
}

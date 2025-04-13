using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    private characterClassStatsAbilities classStatsAbilities;

    [Header("GameObject References")]
    public LayerMask whatIsGround;
    public Transform orientation;
    public Transform cam;
    public Animator animator;
    public Rigidbody rb;
    public TextMeshProUGUI text_speed;


    [Header("Character Checks")]
    public bool grounded;
    public bool isJump;
    public bool jumpReady;
    public bool forcedMovement;


    [Header("Float Values")]

    public float playerHeight;
    public float groundAfterJumpCheckCooldown;
    public float playerGroundDrag;

    [Header("Ground Values")]
    public float groundSpeed;

    [Header("Air Values")]
    public float airSpeed;


    [Header("Keybinds")]
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode jump = KeyCode.Space;

    private float ySpeed;
    private float _nextJumpTime;
    private float tempSpeed;
    private float _speedBoostDuration;
    private bool speedBonusActive;
    float jumpCooldownTime;
    float horizontalInput;
    float verticalInput;
    float airGroundSpeed;
    float jumpForce;
    float baseSpeed;
    float speedBonus;
    Vector3 gravitySpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //rb.linearDamping = 5f; // drag
        animator = GetComponent<Animator>();
        classStatsAbilities = GetComponent<characterClassStatsAbilities>();
        isJump = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;



    }

    // Update is called once per frame
    void Update()
    {
        initializing();

        
        if (Time.time > _speedBoostDuration && speedBonusActive)
        {
            endTempSpeedBoost();
        }
        

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // old input 

        if (grounded) // GRAVITY
        {
            rb.linearDamping = playerGroundDrag;
            Debug.Log("IsGrounded");
            if (Input.GetButtonDown("Jump") && !isJumpCoolingDown)
            {
                rb.AddForce(Vector3.up * jumpForce * 333.33f, ForceMode.Force);
                // ySpeed = jumpForce * 333.33f;
                isJump = true;
                startJumpCooldown();
                Invoke("gravityAfterJumpCheck", groundAfterJumpCheckCooldown);
                animator.SetTrigger("isJumping");
                Debug.Log("Jump!");
                
            }
            if (!isJump)
            {
                ySpeed = 0f;
            }
            animator.SetBool("isGrounded", true);
            airGroundSpeed = groundSpeed;
        }
        else
        {
            rb.linearDamping = 1f;
            Debug.Log("NotGrounded");
            animator.SetBool("isGrounded", false);
            airGroundSpeed = airSpeed;
        }

        ySpeed += Physics.gravity.y * Time.deltaTime;

        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        Vector3 velocity = moveDirection.normalized * (baseSpeed + speedBonus) * 666.66f;

        if (velocity != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("horizontalInput", horizontalInput);
            animator.SetFloat("verticalInput", verticalInput);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        rb.AddForce((velocity * airGroundSpeed) * Time.deltaTime, ForceMode.Force);

        gravitySpeed.y = ySpeed;
        rb.AddForce(gravitySpeed, ForceMode.Force);
        text_speed.SetText("Speed: " + Convert.ToInt32(rb.linearVelocity.magnitude));
    }

    private void initializing()
    {
        baseSpeed = classStatsAbilities.runSpeed;
        jumpForce = classStatsAbilities.jumpForce;
        jumpCooldownTime = classStatsAbilities.jumpCooldown;
    }
    private void movement()
    {

    }
    void gravityAfterJumpCheck()
    {
        isJump = false;
    }

    
    void endTempSpeedBoost()
    {
        speedBonus -= tempSpeed;
        tempSpeed = 0f;
        speedBonusActive = false;
        Debug.Log("Speed Boost ended.");
    }
    public void startTempSpeedBoost(float addSpeed, float speedBoostDuration)
    {
        speedBonusActive = true;
        tempSpeed += addSpeed;
        speedBonus += tempSpeed;
        _speedBoostDuration = Time.time + speedBoostDuration;
        Debug.Log("Speed Boost started.");
    }

    

    public bool isJumpCoolingDown => Time.time < _nextJumpTime;
    public void startJumpCooldown() => _nextJumpTime = Time.time + jumpCooldownTime;
}

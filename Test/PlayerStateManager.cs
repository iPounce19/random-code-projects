using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState currentState;
    PlayerIdleState idle = new PlayerIdleState();
    PlayerWalkingState walk = new PlayerWalkingState();
    PlayerJumpState jumpState = new PlayerJumpState();

    [Header("GameObject References")]
    public LayerMask whatIsGround;
    public CharacterController characterController;
    public Transform orientation;
    public Transform cam;
    public Animator animator;


    [Header("Character Checks")]
    public bool grounded;


    [Header("Float Values")]
    public float baseSpeed;
    public float speedBonus;
    public float jumpForce;
    public float groundDrag; // not in use
    public float turnSmoothTime = 0.1f;

    [Header("Keybinds")]
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode jump = KeyCode.Space;

    public float ySpeed;
    private float _nextJumpTime;
    float horizontalInput;
    float verticalInput;
    float turnSmoothVelocity;

    void Start()
    {
        currentState = idle;
        currentState.EnterState(this);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        //////////
        grounded = Physics.Raycast(transform.position, Vector3.down, 2 * 0.5f + 0.2f, whatIsGround);

        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (!grounded) // GRAVITY
        {
            // characterController.Move(Vector3.down * 9.8f * Time.deltaTime);
        }
        if (Input.GetButtonDown("Jump"))
        {
            ySpeed = jumpForce;
        }
       if(Input.GetKey(up) || Input.GetKey(down) || Input.GetKey(right) || Input.GetKey(left))
        {
            currentState = walk;
            currentState.UpdateState(this);

            animator.SetBool("isMoving", true);
        }
       else if (Input.GetButtonDown("Jump"))
        {
            currentState = jumpState;
            currentState.EnterState(this);
        }

       else
        {
            currentState = idle;
            currentState.EnterState(this);

            animator.SetBool("isMoving", false);
        }
    }
}

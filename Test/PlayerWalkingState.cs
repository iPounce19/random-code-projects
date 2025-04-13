using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    Transform cam;
    CharacterController characterController;
    Transform orientation;
    Transform transform;
    float horizontalInput;
    float verticalInput;
    float turnSmoothVelocity;
    float turnSmoothTime;
    float baseSpeed;
    float speedBonus;
    float ySpeed;

    public override void EnterState(PlayerStateManager _player)
    {
        Debug.Log("in Walking State");
    }

    public override void UpdateState(PlayerStateManager _player)
    {
        Debug.Log("in Walking State");
        cam = _player.cam;
        transform = _player.transform;
        turnSmoothTime = _player.turnSmoothTime;
        baseSpeed = _player.baseSpeed;
        speedBonus = _player.speedBonus;
        characterController = _player.characterController;
        ySpeed = _player.ySpeed;


        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);
        direction.Normalize();

        ////////////////////////// ROTATION OF CHARACTER
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        //////////////////////////

        Vector3 velocity = moveDirection * (baseSpeed + speedBonus);
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);
    }

    public override void OnCollisionEnter(PlayerStateManager _player)
    {

    }
}

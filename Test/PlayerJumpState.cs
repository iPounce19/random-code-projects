using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager _player)
    {
        Debug.Log("in Jumping State");
    }

    public override void UpdateState(PlayerStateManager _player)
    {

    }

    public override void OnCollisionEnter(PlayerStateManager _player)
    {

    }
}

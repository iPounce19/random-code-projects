using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager _player)
    {
        Debug.Log("in Idle State");
    }

    public override void UpdateState(PlayerStateManager _player)
    {

    }

    public override void OnCollisionEnter(PlayerStateManager _player)
    {

    }
}

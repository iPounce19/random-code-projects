using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager _player);

    public abstract void UpdateState(PlayerStateManager _player);

    public abstract void OnCollisionEnter(PlayerStateManager _player);


}

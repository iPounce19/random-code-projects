using UnityEngine;

[System.Serializable]
public class PlayerJumping
{
    [SerializeField] public float jumpForce;
    [SerializeField] public float jumpCooldownTime;
    private float _nextJumpTime;

    public bool isJumpCoolingDown => Time.time < _nextJumpTime;
    public void startJumpCooldown() => _nextJumpTime = Time.time + jumpCooldownTime;
}

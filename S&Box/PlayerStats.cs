using Sandbox;
public sealed class PlayerStats : Component
{
	[Property] public float playerMaxHP { get; set; }
	[Property] public float playerMaxSpeedPercent { get; set; }
	[Property] public float playerMaxJumpHeightPercent { get; set; }
	[Property] public float playerBaseHP { get; set; }

	protected override void OnStart()
	{
		var playerController = this.GameObject.GetComponent<PlayerController>();
		
	}
	protected override void OnUpdate()
	{

	}
}

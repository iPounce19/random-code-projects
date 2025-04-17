using Sandbox;

public sealed class PistolFireSoundTest : Component
{
	protected override void OnEnabled()
	{
		var sound = Sound.Play( "pistol_fire" );
	}
}

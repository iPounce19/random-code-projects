using Sandbox;

public sealed class ParticleDestroy : Component
{
	public float timeToDestroy;


	protected override void OnStart()
	{
		destroyParticle();
	}

	async void destroyParticle()
	{
		await GameTask.DelaySeconds( timeToDestroy );
		GameObject.Destroy();
	}
}

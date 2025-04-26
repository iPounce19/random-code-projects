using Sandbox;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;

public sealed class Explode : Component
{
	public float explosionDamage { get; set; } = 0f;
	public float explosionRadius { get; set; } = 0f;
	public float explosionForce { get; set; } = 0f;

	public TeamType teamType { get; set; }


	void debugExplode()
	{

	}

	/// <summary>
	/// TODO, IMPLEMENT NO PENENTRATION FOR EXPLOSION
	/// </summary>
	public void goExplode()
	{
		// Neutral Explosion, damages both player and enemy alike
		var tr = Scene.Trace
			.Sphere( explosionRadius, this.GameObject.WorldPosition, this.GameObject.WorldPosition )
			.WithoutTags( "World" )
			.WithoutTags( "ladder" )
			.WithoutTags( "water" )
			.WithoutTags( "trigger" )
			.WithoutTags( "particle" );
		var result = tr.RunAll();

		foreach (var objects in result)
		{
			var hitObjects = objects.GameObject;
			var unit = hitObjects.GetComponent<UnitComponent>();
			var unitRB = hitObjects.GetComponent<Rigidbody>();
			//Log.Info(hitObjects.Name );
			Vector3 direciton = (hitObjects.WorldPosition - this.GameObject.WorldPosition).Normal;
			float distance = (hitObjects.WorldPosition - this.GameObject.WorldPosition).Length;
			float forceScale = 1 - (distance / explosionRadius);
			Vector3 force = direciton * explosionForce;
			if (teamType == TeamType.Player )
			{
				if ( unit != null && unit.teamType == TeamType.Enemy )
				{
					unit.onDamage( explosionDamage );
				}
			}
			else if ( teamType == TeamType.Enemy )
			{
				if ( unit != null && unit.teamType == TeamType.Player )
				{
					unit.onDamage( explosionDamage );
				}
			}
			if( unitRB != null )
			{
				unitRB.ApplyImpulseAt( hitObjects.WorldPosition, force + (Vector3.Up * 1000f));
			}

		}
	}

}


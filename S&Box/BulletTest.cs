using Sandbox;
using System.Diagnostics;

public sealed class BulletTest : Component
{
	public DamageInfo damageInfo { get; set; }

	public float damage;
	float currentRange;
	private Vector3 startPosition;
	public float maxRange { get; set; }

	public TeamType teamType;

	[Property] TrailRenderer trailRenderer;


	protected override void OnStart()
	{
		startPosition = WorldPosition;
		Log.Info("Damage: " +damage + " MaxRange: " + maxRange );
	}
	protected override void OnUpdate()
	{
		//Gizmo.Draw.LineSphere( WorldPosition, 30f);
		var explosion = this.GameObject.GetComponent<Explode>();
		var tr = Scene.Trace
			.Sphere( 1.8f, startPosition, this.GameObject.WorldPosition )
			.WithoutTags( "player" )
			.WithoutTags("particle")
			.Run();	

		if ( tr.Hit )
		{
			var hitObject = tr.GameObject;
			var unit = hitObject.GetComponent<UnitComponent>();
			
			if ( unit != null )
			{
				if(teamType == TeamType.Player)
				{
					if ( unit.teamType == TeamType.Enemy )
					{
						unit.onDamage( damage );
					}
				}
			}
			//Log.Info("Bullet hits" + hitObject.Name );
			if ( explosion != null )
			{
				explosion.goExplode(); 
			}
			//Log.Info( "Bullet hit " + hitObject.Name );
			this.GameObject.Destroy();
		}


		if (Vector3.DistanceBetween(startPosition, this.GameObject.WorldPosition) >= maxRange)
		{
			//Log.Info( "Bullet reached maxRange" );
			this.GameObject.Destroy();
		}
		
	}
}

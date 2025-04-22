using Sandbox;
using System.Diagnostics;

public sealed class BulletTest : Component
{
	public DamageInfo damageInfo { get; set; }

	public float damage;

	protected override void OnUpdate()
	{
		Gizmo.Draw.LineSphere( WorldPosition, 6f);
		
	}
}

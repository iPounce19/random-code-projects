using Sandbox;
using System.IO.Compression;
using System.Numerics;
using static Sandbox.VertexLayout;

public sealed class TopDownCamera : Component
{
	[Property] CameraComponent CameraComponent { get; set; }
	[Property] GameObject mainBody { get; set; }
	[Property] float cameraZOffset { get; set; }

	protected override void OnUpdate()
	{
		if (CameraComponent != null)
		{
			Vector3 topDownOffset = new Vector3( 0, 0, cameraZOffset );

			float playerYaw = mainBody.WorldRotation.Yaw();
			CameraComponent.WorldPosition = new Vector3( mainBody.WorldPosition.x, mainBody.WorldPosition.y, cameraZOffset );
			CameraComponent.WorldRotation = Rotation.Identity
				* Rotation.FromAxis( Vector3.Up, playerYaw )
				* Rotation.FromAxis( Vector3.Right, -90 )
				* Rotation.FromAxis( Vector3.Forward, 0 );
			Log.Info( CameraComponent.WorldRotation );
		}
	}
}

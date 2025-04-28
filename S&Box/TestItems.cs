using Sandbox;

public sealed class TestItems : Component
{

	[Property] PrefabFile UIPrefab { get; set; }
	[Property]public string itemName { get; set; }
	[Property] string itemDescription { get; set; }
	[Property] float zOffset { get; set; } = 40f;

	[Property] float timeToShow { get; set; } = 2f;

	[Property, Feature("Upgrades")] public bool canDoubleJump { get; set; } = false;
	[Property, Feature( "Upgrades" )] public bool canDash { get; set; } = false;
	[Property, Feature( "Upgrades" )] public bool isDoubleTapEnabled { get; set; } = false;
	[Property, Feature( "Upgrades" )] public bool doBulletsExplode { get; set; } = false;
	GameObject UI { get; set; }
	float timer;
	protected override void OnAwake()
	{
		if (!UIPrefab.IsValid() )
		{
			Log.Error( "UIPrefab is not valid" );
			return;
		}
		UI = GameObject.Clone(UIPrefab, this.GameObject.WorldTransform);
		UI.SetParent( this.GameObject, true );
		UI.Enabled = false;

	}
	protected override void OnFixedUpdate()
	{
		UI.LocalPosition = new Vector3( 0, 0, zOffset ) + this.WorldPosition;
		if (timer < Time.Now) UI.Enabled = false;
	}

	public void showItemUI()
	{
		UI.Enabled = true;
		timer = Time.Now + timeToShow;
	}

	public void useItem()
	{
		UI.Destroy();
		this.GameObject.Destroy();
	}
}

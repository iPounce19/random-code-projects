using Sandbox;	

public sealed class PlayerHP : Component
{
	[Property, ReadOnly]
	public float playerHP { get; private set; }

	[Property] SkinnedModelRenderer mainBody { get; set; }

	[Property, ReadOnly]
	private float maxHP { get; set; }

	protected override void OnStart()
	{
		var playerStats = this.GameObject.GetComponent<PlayerStats>();
		if ( playerStats != null )
		{
			maxHP = playerStats.playerMaxHP;
			playerHP = maxHP;
			Log.Info( $"Player has {playerHP} maxHP" );
		}
		else
		{
			Log.Error( "PlayerStats component not found!" );
		}
	}

	protected override void OnUpdate()
	{
	}

	public void TakeDamage( float damage )
	{
		if( damage < 0 ) return; // Prevent negative damage
		if ( !mainBody.IsValid() ) return;
		mainBody.Set( "b_hit", true );
		playerHP -= damage;
		if ( playerHP < 0 ) playerHP = 0;
		Log.Info( $"Player took {damage} damage, current HP: {playerHP}" );
	}

	public void Heal( float heal )
	{
		playerHP += heal;
		if ( playerHP > maxHP ) playerHP = maxHP;
		Log.Info( $"Player healed {heal}, current HP: {playerHP}" );
	}
}

using Sandbox;

public enum TeamType
{
	Neutral,
	Player,
	Enemy
}
public enum playerClass
{
	None,
	Assault,
	Medic,
	Engineer,
	Support
}
public sealed class UnitComponent : Component
{
	[Property, Feature( "Basic Information" )] public string unitName { get; set; }
	[Property, Feature( "Basic Information" )] public TeamType teamType { get; set; }
	[Property, Feature( "Basic Information" )] public playerClass playerClass { get; set; }
	[Property, Feature( "Basic Information" )] public int unitID { get; set; }
	[Property, Feature( "Basic Information" )] public int playerCredit { get; set; }
	[Property, Feature( "Basic Information" )] SkinnedModelRenderer mainBody { get; set; }

	[Property, FeatureEnabled( "Health" )] bool _health { get; set; } = true;
	[Property, Feature( "Health" ), ReadOnly] public float currentHP { get; set; }
	[Property, Feature( "Health" )] public float maxHP { get; set; } = 100f;


	[Property, FeatureEnabled( "Player Upgrades" )] bool _playerUpgrades { get; set; } = false;
	[Property, Feature( "Player Upgrades" ), Group( " Player Upgrades" )] public float movementSpeedIncrease { get; set; } = 0.0f;
	[Property, Feature( "Player Upgrades" ), Group( " Player Upgrades" )] public bool canDoubleJump { get; set; } = false;
	[Property, Feature( "Player Upgrades" ), Group(" Pistol Upgrades")] public bool isDoubleTapEnabled { get; set; } = false;
	[Property, Feature( "Player Upgrades" ), Group( " Pistol Upgrades" )] public bool doBulletsExplode { get; set; } = false;

	[Property, FeatureEnabled( "Enemy Upgrades" )] bool _enemyUpgrades { get; set; } = false;

	[Property, Feature( "Player References" )] WeaponPistolScript weaponPistol { get; set; }


	protected override void OnStart()
	{
		currentHP = maxHP;
	}

	[Button( "Damage 10 HP" )]
	public void debugDamage()
	{
		onDamage( 10f );
	}

	public void onDamage( float damage )
	{
		if ( !_health ) return;
		if ( damage < 0 ) return; // Prevent negative damage\
		currentHP -= damage;
		if ( currentHP < 0 ) currentHP = 0;

		playerMainBody();

	}

	public void playerMainBody()
	{
		if ( !mainBody.IsValid() ) return;
		mainBody.Set( "hit", true );
	}
	public void onHeal( float heal )
	{
		if ( !_health ) return;
		if ( heal < 0 ) return;
		currentHP += heal;
		if ( currentHP > maxHP ) currentHP = maxHP;
	}


	[Button( "Check Player Upgrades" )]
	public void checkPistolUpgrades()
	{
		if ( _playerUpgrades )
		{
			if ( isDoubleTapEnabled )
			{
				weaponPistol.isDoubleTapEnabled = isDoubleTapEnabled;
				Log.Info( "Double Tap Enabled" );
			}
			else
			{
				Log.Info( "Double Tap Disabled" );
			}
			if ( doBulletsExplode )
			{
				weaponPistol.doBulletsExplode = doBulletsExplode;
				Log.Info( "Explosive Bullets Enabled" );
			}
			else
			{
				Log.Info( "Explosive Bullets Disabled" );
			}
		}
		else
		{
			Log.Info( "Player Upgrades Disabled" );
		}
	}
}

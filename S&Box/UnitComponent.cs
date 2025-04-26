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

public enum headGear
{
	None
}
public enum  chestGear
{
	None
}
public enum legGear
{
	None
}
public enum footGear
{
	None,
	doubleJump,
}
public enum backGear
{
	None,
	booster,
}

public sealed class UnitComponent : Component
{
	[Property, Feature( "Basic Information" )] public string unitName { get; set; }
	[Property, Feature( "Basic Information" )] public TeamType teamType { get; set; }
	[Property, Feature( "Basic Information" )] public playerClass playerClass { get; set; }
	[Property, Feature( "Basic Information" )] public int unitID { get; set; }
	[Property, Feature( "Basic Information" )] public int playerCredit { get; set; }
	

	[Property, FeatureEnabled( "Health" )] bool _health { get; set; } = true;
	[Property, Feature( "Health" ), ReadOnly] public float currentHP { get; set; }
	[Property, Feature( "Health" )] public float maxHP { get; set; } = 100f;

	//########################################
	[Property, FeatureEnabled( "Player Upgrades" )] bool _playerUpgrades { get; set; } = false;
	[Property, Feature( "Player Upgrades" ), Group( " Player Upgrades" )] public float movementSpeedIncrease { get; set; } = 0.0f;
	/// <summary>
	/// UNDER CONSTRUCTION
	/// </summary>
	[Property, Feature( "Player Upgrades" ), Group( " Player Upgrades" )] public bool canDoubleJump { get; set; } = false;
	[Property, Feature( "Player Upgrades" ), Group( " Player Upgrades" )] public bool canDash { get; set; } = false;
	[Property, Feature( "Player Upgrades" ), Group( " Player Upgrades" )] public float dashSpeed { get; set; } = 0.0f;
	[Property, Feature( "Player Upgrades" ), Group( " Player Upgrades" )] public float dashCooldown { get; set; } = 5.0f;
	[Property, Feature( "Player Upgrades" ), Group(" Pistol Upgrades")] public bool isDoubleTapEnabled { get; set; } = false;
	[Property, Feature( "Player Upgrades" ), Group( " Pistol Upgrades" )] public bool doBulletsExplode { get; set; } = false;
	[Property, Feature( "Player Upgrades" ), Group( "Player Equipment" )] headGear headGear { get; set; } = headGear.None;
	[Property, Feature( "Player Upgrades" ), Group( "Player Equipment" )] chestGear chestGear { get; set; } = chestGear.None;
	[Property, Feature( "Player Upgrades" ), Group( "Player Equipment" )] legGear legGear { get; set; } = legGear.None;
	[Property, Feature( "Player Upgrades" ), Group( "Player Equipment" )] footGear footGear { get; set; } = footGear.None;
	[Property, Feature( "Player Upgrades" ), Group( "Player Equipment" )] backGear backGear { get; set; } = backGear.None;

	//#########################################
	[Property, FeatureEnabled( "Enemy Upgrades" )] bool _enemyUpgrades { get; set; } = false;

	//########################################
	[Property, Feature( "Player References" )] WeaponPistolScript weaponPistol { get; set; }

	[Property, Feature( "Player References" )] SkinnedModelRenderer mainBody { get; set; }
	[Property, Feature( "Player References" )] PlayerController playerController { get; set; }
	[Property, Feature ( "Player References" )] Rigidbody rigidbody{ get; set; }

	//########################################
	bool _canDoubleJump;
	float timeToDash;
	bool _canDash = true;
	bool _firstJump;

	TestItems item;
	protected override void OnStart()
	{
		currentHP = maxHP;
	}

	protected override void OnUpdate()
	{
		if (teamType == TeamType.Player )
		{
			var reach = playerController.EyePosition + playerController.EyeAngles.Forward * playerController.ReachLength;
			//Gizmo.Draw.Line( playerController.EyePosition, reach );
			var trReach = Scene.Trace
				.Ray( playerController.EyePosition, reach )
				.WithTag( "item" )
				.Run();

			if ( trReach.Hit )
			{
				item = trReach.GameObject.GetComponent<TestItems>();
				item.showItemUI();
				if ( Input.Pressed( "Use" ) )
				{
					applyItem();
					item.useItem();
				}
			}
		}
	}
	protected override void OnFixedUpdate()
	{
		//onDoubleJump();
		onDash();
		if ( timeToDash < Time.Now )
		{
			_canDash = true;
		}
	}

	[Button( "Damage 10 HP" )]
	public void debugDamage()
	{
		onDamage( 10f );
	}

	void onDoubleJump()
	{
		if ( !playerController.IsOnGround && canDoubleJump && Input.Pressed( "Jump" ) && _canDoubleJump  )
		{
			playerController.Jump( Vector3.Up * playerController.JumpSpeed );
			_canDoubleJump = false;
			Log.Info( "Double Jump" );
		}
		if (playerController.IsOnGround && !_canDoubleJump)
		{
			_canDoubleJump = true;
		}
	}
	void onDash()
	{
		if ( _canDash && canDash && Input.Pressed("Dash") && (Input.Down("Forward") || Input.Down( "Backward" ) || Input.Down( "Left" ) || Input.Down( "Right" )) )
		{
			rigidbody.ApplyImpulse( (playerController.Velocity * dashSpeed) + (Vector3.Up * 100f));
			timeToDash = Time.Now + dashCooldown;
			_canDash = false;
			Log.Info( "Dashing" );
		}
	}
	public void onDamage( float damage )
	{
		if ( !_health ) return;
		if ( damage < 0 ) return; // Prevent negative damage\
		currentHP -= damage;
		if ( currentHP < 0 ) currentHP = 0;

		playerHitAnimation();

	}

	public void playerHitAnimation()
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
				//weaponPistol.isDoubleTapEnabled = isDoubleTapEnabled;
				Log.Info( "Double Tap Enabled" );
			}
			else
			{
				Log.Info( "Double Tap Disabled" );
			}
			if ( doBulletsExplode )
			{
				//weaponPistol.doBulletsExplode = doBulletsExplode;
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
	void applyItem()
	{
		if ( item.canDoubleJump )
		{
			canDoubleJump = true;
			_canDoubleJump = false;
			//Log.Info( "Double Jump Enabled" );
		}
		if ( item.canDash )
		{
			canDash = true;
			//Log.Info( "Dash Enabled" );
		}
		if ( item.isDoubleTapEnabled )
		{
			isDoubleTapEnabled = true;
			//Log.Info( "Double Tap Enabled" );
		}
		if ( item.doBulletsExplode )
		{
			doBulletsExplode = true;
			//Log.Info( "Explosive Bullets Enabled" );
		}

	}
}

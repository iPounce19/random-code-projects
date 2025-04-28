using Sandbox;
using System;

public sealed class WeaponPistolScript : Component
{
	private enum weaponTier
	{
		Default = 0,
		Uncommon = 1,
		Rare = 2,
	}

	[Property, Group( "References" )] private PlayerController controller { get; set; }
	[Sync] [Property, Group( "References" )] private SkinnedModelRenderer mainBody { get; set; }
	[Property, Group( "References" )] private SkinnedModelRenderer gunModel { get; set; }
	[Property, Group( "References" )] private UnitComponent unitComponent { get; set; }
	[Property, Group( "References" )] private GameObject camera { get; set; }
	[Property, Group( "References" )] private GameObject muzzleFlashPosition { get; set; }
	[Property, Group( "References" )] private GameObject ejectPosition { get; set; }

	[Property, Group( "References" )] private SoundEvent gunClick { get; set; }
	[Property, Group( "References" )] private SoundEvent fireSound { get; set; }
	[Property, Group( "References" )] private SoundEvent reloadSound { get; set; }
	[Property, Group( "References" )] private string muzzleFlashPrefab { get; set; } = "weapon/pistol/pistol_muzzleflash.prefab";
	[Property, Group( "References" )] private string shellCasingPrefab { get; set; } = "weapon/pistol/pistol_shell.prefab";
	[Property, Group( "References" )] private string bulletPrefab { get; set; } = "weapon/pistol/bullet_Test.prefab";

	[Property] weaponTier gunTier { get; set; } = weaponTier.Default; // Default, Uncommon, Rare


	float Damage;
	float Range;
	float FireRate;
	float reloadTime;
	public int maxAmmo;
	///////////////////////////////////////////////// DEFAULT 

	/// <summary>
	/// Self explanatory
	/// </summary>
	[Property, Group( "A2 Default Pistol Stats" ), Feature( "Gun Stats" )] float Damage_default { get; set; } = 10f;
	/// <summary>
	/// How long is the gun's range. Raycast method
	/// </summary>
	 [Property, Group( "A2 Default Pistol Stats" ), Feature( "Gun Stats" )] float Range_default { get; set; } = 500f;
	/// <summary>
	/// How fast the gun will fire, in seconds. 0.2f = 5 shots per second.
	/// </summary>
	[Property, Group("A2 Default Pistol Stats"), Feature("Gun Stats")] float FireRate_default { get; set; } = 0.2f;
	/// <summary>
	/// Reload speed, how fast the character will reload and insert bullets. Doesn't affect animation
	/// </summary>
	[Property, Group("A2 Default Pistol Stats"), Feature("Gun Stats")] float reloadTime_default { get; set; } = 1.5f;
	/// <summary>
	/// Maximum Ammo for this gun.
	/// </summary>
	[Property, Group("A2 Default Pistol Stats"), Feature("Gun Stats")] int maxAmmo_default { get; set; } = 10;

	[Property, Group("A2 Default Pistol Stats"), Feature("Gun Stats")] float bulletSpeed_default { get; set; } = 50000f;

	/////////////////////////////////////////////////

	///////////////////////////////////////////////// UNCOMMON 

	/// <summary>
	/// Self explanatory
	/// </summary>
	[Property, Group( "A3 UNCOMMON Pistol Stats" ), Feature( "Gun Stats" )] float Damage_uncommon { get; set; } = 13f;
	/// <summary>
	/// How long is the gun's range. Raycast method
	/// </summary>
	[Property, Group("A3 UNCOMMON Pistol Stats"), Feature("Gun Stats")] float Range_uncommon { get; set; } = 700f;
	/// <summary>
	/// How fast the gun will fire, in seconds. 0.2f = 5 shots per second.
	/// </summary>
	[Property, Group("A3 UNCOMMON Pistol Stats"), Feature("Gun Stats")] float FireRate_uncommon { get; set; } = 0.16f;
	/// <summary>
	/// Reload speed, how fast the character will reload and insert bullets. Doesn't affect animation
	/// </summary>
	[Property, Group("A3 UNCOMMON Pistol Stats"), Feature("Gun Stats")] float reloadTime_uncommon { get; set; } = 1.3f;
	/// <summary>
	/// Maximum Ammo for this gun.
	/// </summary>
	[Property, Group("A3 UNCOMMON Pistol Stats"), Feature("Gun Stats")]  int maxAmmo_uncommon { get; set; } = 15;
	[Property, Group("A3 UNCOMMON Pistol Stats"), Feature("Gun Stats")] float bulletSpeed_uncommon { get; set; }

	/////////////////////////////////////////////////

	///////////////////////////////////////////////// RARE 

	/// <summary>
	/// Self explanatory
	/// </summary>
	[Property, Group("A4 RARE Pistol Stats"), Feature("Gun Stats")] float Damage_rare { get; set; } = 16f;
	/// <summary>
	/// How long is the gun's range. Raycast method
	/// </summary>
	[Property, Group("A4 RARE Pistol Stats"), Feature("Gun Stats")] float Range_rare { get; set; } = 900f;
	/// <summary>
	/// How fast the gun will fire, in seconds. 0.2f = 5 shots per second.
	/// </summary>
	[Property, Group("A4 RARE Pistol Stats"), Feature("Gun Stats")] float FireRate_rare { get; set; } = 0.12f;
	/// <summary>
	/// Reload speed, how fast the character will reload and insert bullets. Doesn't affect animation
	/// </summary>
	[Property, Group("A4 RARE Pistol Stats"), Feature("Gun Stats")] float reloadTime_rare { get; set; } = 1.1f;
	/// <summary>
	/// Maximum Ammo for this gun.
	/// </summary>
	[Property, Group("A4 RARE Pistol Stats"), Feature("Gun Stats")]  int maxAmmo_rare { get; set; } = 20;
	[Property, Group("A4 RARE Pistol Stats"), Feature("Gun Stats")] float bulletSpeed_rare { get; set; }

	/////////////////////////////////////////////////
	///
	[Property, Feature ( "Gun Stats" ), Group( "Exploding Bullets" )] float explosionDamage { get; set; } = 20f;
	[Property, Feature ( "Gun Stats" ), Group( "Exploding Bullets" )] float explosionRadius { get; set; } = 30f;
	[Property, Feature ( "Gun Stats" ), Group( "Exploding Bullets" )] float explosionForce { get; set; } = 100f;



	/// <summary>
	/// 25% chance to fire two bullet instead of one. Consumes two bullets.
	/// </summary>
	[Property, Group( "Gun Power UPs" ), ReadOnly] public bool isDoubleTapEnabled
	{
		get
		{
			if ( unitComponent.IsValid )
			{
				if ( unitComponent.isDoubleTapEnabled == true )
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}

	private bool _isDoubleTapEnabled
	{
		get
		{
			if ( isDoubleTapEnabled == true )
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
	[Property, Group( "Gun Power UPs" ), ReadOnly]
	public bool doBulletsExplode
	{
		get
		{
			if ( unitComponent.IsValid )
			{
				if ( unitComponent.doBulletsExplode == true )
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}
	private bool _doBulletsExplode
	{
		get
		{
			if( doBulletsExplode == true )
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
	[Property, Group( "Gun Power UPs" ), ReadOnly] public bool doBulletsPenetrate { get; set; }

	private bool _doBulletsPenetrate;

	[Property, Group( "Miscellaneous" )] float shellCasingTimeToDisappear { get; set; } = 10f;

	SoundHandle reloadSoundHandle = null;
	public bool isFiring; // For reference just in case if ModelRenderer is managed by another script
	bool _onEnable = false;
	bool _canFire;
	bool _isReloading = false;
	public int _currentAmmo;
	float _nextFire;
	float _reloadStopTime = 0f;


	Line line = new Line( new Vector3( 0, 0, 0 ), new Vector3( 10, 0, 0 ) );

	protected override void OnStart()
	{
		checkGunTier();
		_currentAmmo = maxAmmo;
		_canFire = true;
		//Log.Info( "Pistol Enabled" );	
	}

	protected override void OnEnabled()
	{
		onEnableDelay();
	}

	protected override void OnDisabled()
	{
		
		_onEnable = false;
		if(_isReloading) // Fixes reloading, unable to get maxAmmo if you disabled or switched to another weapon while the weapon is reloading...
		{
			//Future fix, Cancel animation, During reload animation, swapping back, and reloading again, the animation is not played.
			reloadSoundHandle.Stop();
			_isReloading = false;
			_reloadStopTime = 0f;
		}
	}


	protected override void OnUpdate()
	{
		//debugPistol();

	}
	protected override void OnFixedUpdate()
	{
		if ( IsProxy ) return;
		if ( !mainBody.IsValid() ) return;
		//Log.Info( "Double Tap" + isDoubleTapEnabled );
		//Log.Info("Double Tap" +_isDoubleTapEnabled );
		if (reloadSoundHandle != null)
		{
			if ( reloadSoundHandle.IsPlaying)
			{
				reloadSoundHandle.Transform = WorldTransform;
			}
		}

		if (this.GameObject.Enabled == true)
		{
			if (Input.Down("Attack1") && _canFire && !_isReloading && _onEnable)
			{
				if ( _isDoubleTapEnabled && _currentAmmo >= 2 && Random.Shared.Float( 0, 1 ) < 0.2f )
				{
					Log.Info( "Double Tap!" );
					doubleTap();
				}
				else
				{
					pistolAttack();
				}
			}


			if ( Input.Pressed( "Reload" ) && _currentAmmo < maxAmmo && _onEnable && !_isReloading )
			{
				_reloadStopTime = Time.Now + reloadTime;
				pistolReload();
			}

			if ( _reloadStopTime < Time.Now && _isReloading)
			{
				_isReloading = false;
				_currentAmmo = maxAmmo;
			}

			if ( _nextFire < Time.Now )
			{
				_canFire = true;
				isFiring = false;
			}


		}
	}

	[Button("Debug Check Gun Tier")]
	void checkGunTier()
	{
		switch(gunTier )
		{
			case weaponTier.Default:
				Damage = Damage_default;
				Range = Range_default;
				FireRate = FireRate_default;
				reloadTime = reloadTime_default;
				maxAmmo = maxAmmo_default;
				break;
			case weaponTier.Uncommon:
				Damage = Damage_uncommon;
				Range = Range_uncommon;
				FireRate = FireRate_uncommon;
				reloadTime = reloadTime_uncommon;
				maxAmmo = maxAmmo_uncommon;
				break;
			case weaponTier.Rare:
				Damage = Damage_rare;
				Range = Range_rare;
				FireRate = FireRate_rare;
				reloadTime = reloadTime_rare;
				maxAmmo = maxAmmo_rare;
				break;
			default:
				Log.Info( "Gun Tier not set" );
				break;
		}
	}

	void debugPistol()
	{
		Gizmo.Draw.Line( controller.EyeTransform.Position, controller.EyeTransform.Position + controller.EyeAngles.Forward * 5000f );
	}
	void pistolAttack()
	{
		if (_currentAmmo > 0 )
		{
			pistolFireSound();
			muzzleFlashs();
			spawnProjectile();
			mainBody.Set( "b_attack", true );
			_currentAmmo--;
			isFiring = true;
			_canFire = false;
			_nextFire = Time.Now + FireRate;

			shellCasingCreateDestroy();
			//Log.Info( "Pistol Ammo Count: " + _currentAmmo );
		}
		else
		{
			if ( !gunClick.IsValid ) return;
			Sound.Play( gunClick, Transform.World.Position );
			_nextFire = Time.Now + FireRate;
			_canFire = false;
			//Log.Info( "Pistol Click Sound Played" );
		}
	}

	private void pistolFireSound()
	{
		if ( !fireSound.IsValid ) return;
		Sound.Play( fireSound, Transform.World.Position);
		//Log.Info( "Pistol Fire Sound Played" );
	}
	private void pistolReloadSound()
	{
		if ( !reloadSound.IsValid ) return;
		reloadSoundHandle = Sound.Play( reloadSound, Transform.World.Position );
		//Log.Info( "Pistol Reload Sound Played" );
	}
	void pistolReload()
	{
		pistolReloadSound();
		_isReloading = true;
		mainBody.Set( "b_reload", true );
		//Log.Info( "Pistol Reloading.." );
	}

	private void spawnProjectile()
	{
		var projectile = GameObject.Clone( bulletPrefab, muzzleFlashPosition.WorldTransform );
		projectile.NetworkSpawn();
		Rigidbody rb = projectile.GetComponent<Rigidbody>();
		BulletTest bullet = projectile.GetComponent<BulletTest>();
		if ( !rb.IsValid) return;
		if ( !bullet.IsValid ) return;
		bullet.maxRange = Range;
		bullet.damage = Damage;
		bullet.teamType = unitComponent.teamType;
		rb.ApplyForce( controller.EyeAngles.Forward * bulletSpeed_default);
		if ( _doBulletsExplode )
		{
			projectile.AddComponent<Explode>();
			var explosion = projectile.GetComponent<Explode>();
			explosion.explosionDamage = explosionDamage;
			explosion.explosionRadius = explosionRadius;
			explosion.explosionForce = explosionForce;
			explosion.teamType = unitComponent.teamType;
			explosion.explodeType = ExplodeType.Small;
		}
		
	}

	private void shellCasingCreateDestroy()
	{
		var ejectedCasing = GameObject.Clone( shellCasingPrefab, ejectPosition.WorldTransform );
		var particleDestroy = ejectedCasing.GetComponent<ParticleDestroy>();
		ejectedCasing.NetworkSpawn();
		particleDestroy.timeToDestroy = shellCasingTimeToDisappear;
	}

	private async void muzzleFlashs()
	{
		GameObject muzzleFlashCreate = GameObject.Clone( muzzleFlashPrefab, muzzleFlashPosition.WorldTransform );
		muzzleFlashCreate.NetworkSpawn(); 
		await Task.DelaySeconds( 0.05f );
		muzzleFlashCreate.Destroy();
	}

	private async void onEnableDelay()
	{
		await Task.DelaySeconds( 0.5f );
		_onEnable = true;
	}

	async void doubleTap()
	{
		pistolAttack();
		await Task.DelaySeconds( 0.1f );
		pistolAttack();
	}

}

using Sandbox;
using System;

public sealed class WeaponPistolScript : Component
{
	[Property] SkinnedModelRenderer mainBody { get; set; }
	[Property] GameObject firePoint { get; set; }
	[Property] GameObject muzzleFlashPosition { get; set; }
	[Property] SoundEvent gunClick { get; set; }
	[Property] SoundEvent fireSound { get; set; }
	[Property] SoundEvent reloadSound { get; set; }
	[Property] float Damage { get; set; } = 10f;
	[Property] float Range { get; set; } = 500f;
	[Property] float FireRate { get; set; } = 0.2f;

	[Property] float reloadTime { get; set; } = 1.5f;
	/// <summary>
	/// Maximum Ammo for this gun.
	/// </summary>
	[Property] public int maxAmmo { get; set; } = 10;
	/// <summary>
	/// 25% chance to fire two bullet instead of one. Consumes two bullets.
	/// </summary>
	[Property] bool isDoubleTapEnabled { get; set; } = false; // Don't forget to test it to false, true is for testing.

	SoundHandle stopSound;
	public bool isFiring; // For reference just in case if ModelRenderer is managed by another script
	bool _onEnable = false;
	bool _canFire;
	bool _isReloading = false;
	public int _currentAmmo;
	float _nextFire;
	float _reloadStopTime = 0f;
	bool _canDoubleTap= false;

	protected override void OnStart()
	{
		_currentAmmo = maxAmmo;
		_canFire = true;
		Log.Info( "Pistol Enabled" );	
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
			stopSound.Stop();
			_isReloading = false;
			_reloadStopTime = 0f;
		}
	}

	protected override void OnFixedUpdate()
	{
		if ( !mainBody.IsValid() ) return;
		if (this.GameObject.Enabled == true)
		{
			if (Input.Down("Attack1") && _canFire && !_isReloading && _onEnable)
			{
				if ( isDoubleTapEnabled && _currentAmmo >= 2 && Random.Shared.Float( 0, 1 ) < 0.2f )
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

	void pistolAttack()
	{
		if (_currentAmmo > 0 )
		{
			pistolFireSound();
			muzzleFlashs();
			mainBody.Set( "b_attack", true );
			_currentAmmo--;
			isFiring = true;
			_canFire = false;
			_nextFire = Time.Now + FireRate;
			Log.Info( "Pistol Ammo Count: " + _currentAmmo );
		}
		else
		{
			if ( !gunClick.IsValid ) return;
			Sound.Play( gunClick, Transform.World.Position );
			_nextFire = Time.Now + FireRate;
			_canFire = false;
			Log.Info( "Pistol Click Sound Played" );
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
		stopSound = Sound.Play( reloadSound, Transform.World.Position );
		//Log.Info( "Pistol Reload Sound Played" );
	}
	void pistolReload()
	{
		pistolReloadSound();
		_isReloading = true;
		mainBody.Set( "b_reload", true );
		Log.Info( "Pistol Reloading.." );
	}

	private async void muzzleFlashs()
	{
		GameObject muzzleFlashCreate = GameObject.Clone( "weapon/pistol/pistol_muzzleflash.prefab", muzzleFlashPosition.WorldTransform );
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
	void pistolCommon()
	{
		Damage = 10f;
		Range = 500f;
		FireRate = 0.2f;
		maxAmmo = 10;
	}

	void pistolUncommon()
	{
		Damage = 15f;
		Range = 600f;
		FireRate = 0.17f;
		maxAmmo = 15;
	}
}

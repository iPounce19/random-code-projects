using Sandbox;
using System.Security.Cryptography;

public sealed class WeaponShotgunScript : Component
{
	[Property] SoundEvent fireSound { get; set; }
	[Property] SoundEvent reloadSound { get; set; }
	[Property] SoundEvent fireClick { get; set; }

	[Property] SoundEvent shotgunCockSound { get; set; }

	[Property] GameObject firePoint { get; set; }
	[Property] GameObject muzzleFlashPosition { get; set; }
	[Property] SkinnedModelRenderer mainBody { get; set; }

	[Property] float Damage { get; set; } = 8f;
	[Property] float Range { get; set; } = 100f;
	[Property] float FireRate { get; set; } = 1.5f;
	[Property] int shellsPerInsert { get; set; } = 1;

	[Property] float reloadSpeed { get;set; }

	[Property] float reloadTime { get; set; } = 0.6f;
	[Property] float afterReloadDelay { get; set; } = 0.5f;
	[Property] public int maxAmmo { get; set; } = 6;

	bool oneInTheChamber = false;
	bool _readytoInsert;
	bool _onEnable = false;
	bool _canFire;
	bool _canReload;
	bool _isReloading = false;
	public int _currentAmmo;
	float _nextFire;
	float _reloadAfterFire;

	protected override void OnStart()
	{
		_currentAmmo = maxAmmo;
		_canFire = true;
		Log.Info( "Shotgun Enabled" );
	}
	protected override void OnEnabled()
	{
		onEnableDelay();
	}

	protected override void OnDisabled()
	{
		_onEnable = false;
	}
	protected override void OnUpdate()
	{
		if(Input.Down("Attack1") && _canFire && _onEnable && !_isReloading)
		{
			shotgunAttack();
		}
		if(Input.Down("Reload" ) && _currentAmmo < maxAmmo && !_isReloading && _onEnable && _canReload )
		{
			_isReloading = true;
			_readytoInsert = true;
		}


		if ( Input.Down( "Attack1" ) )
		{
			_isReloading = false;
			_readytoInsert = false;
		}
		while ( _isReloading && (_currentAmmo != maxAmmo || _currentAmmo <= maxAmmo) && _readytoInsert  )
		{
			
			_readytoInsert = false;
			shotgunReload();
			if ( _currentAmmo >= maxAmmo )
			{
				//_isReloading = false;
				//_readytoInsert = false;
				//_nextFire = Time.Now + afterReloadDelay;
				break;
			}

		}
		
		

		if (!_isReloading)
		{
			mainBody.Set( "b_reloading", false );
		}

		if (_nextFire <	 Time.Now)
		{
			_canFire = true;
		}
		if (_reloadAfterFire < Time.Now )
		{
			_canReload = true;
		}
	}
	void shotgunAttack()
	{
		if(_currentAmmo > 0)
		{
			if ( !mainBody.IsValid() ) return;
			mainBody.Set( "b_attack", true );
			_canFire = false;
			_canReload = false;
			_nextFire = Time.Now + FireRate;
			_reloadAfterFire = Time.Now + FireRate;
			_currentAmmo--;
			shotgunAttackSound();
			Log.Info( "Shotgun Ammo Count: " + _currentAmmo );
		}
		else
		{
			shotgunClickSound();
		}
	}

	private async void shotgunReload()
	{
		mainBody.Set( "b_reloading", true );
		await Task.DelaySeconds( 0.05f );
		mainBody.Set( "b_reloading_insert", true );
		if ( !reloadSound.IsValid() ) return;
		shotgunReloadSound();
		_currentAmmo = _currentAmmo + shellsPerInsert;
		if( _currentAmmo >= maxAmmo )
		{
			_currentAmmo = maxAmmo;
			_isReloading = false;
			_readytoInsert = false;
			_nextFire = Time.Now + afterReloadDelay;
		}
		await Task.DelaySeconds( 0.5f );
		_readytoInsert = true;
	}

	async void shotgunAttackSound()
	{
		if(!fireSound.IsValid() && !shotgunCockSound.IsValid) return;
		Sound.Play(fireSound, Transform.World.Position );
		await Task.DelaySeconds( 0.3f );
		Sound.Play( shotgunCockSound, Transform.World.Position );
	}

	void shotgunReloadSound()
	{
		if ( !reloadSound.IsValid() ) return;
	}

	void shotgunClickSound()
	{
		if ( !fireClick.IsValid() ) return;

		Sound.Play( fireClick, Transform.World.Position );
		_nextFire = Time.Now + 0.5f;
		_canFire = false;
	}
	private async void onEnableDelay()
	{
		await Task.DelaySeconds( 0.5f );
		_onEnable = true;
	}
}

using Sandbox;

public sealed class WeaponPistolScript : Component
{
	[Property] SkinnedModelRenderer mainBody { get; set; }
	[Property] GameObject firePoint { get; set; }

	[Property] float Damage { get; set; } = 10f;
	[Property] float Range { get; set; } = 500f;
	[Property] float FireRate { get; set; } = 0.2f;

	[Property] float reloadTime { get; set; } = 1.5f;
	[Property] int maxAmmo { get; set; } = 10;

	bool _canFire;
	bool _isReloading = false;
	int _currentAmmo;
	float _nextFire;
	float _reloadStopTime = 0f;

	protected override void OnStart()
	{
		_currentAmmo = maxAmmo;
		_canFire = true;
		Log.Info( "Pistol Enabled" );
	}

	protected override void OnFixedUpdate()
	{
		if ( !mainBody.IsValid() ) return;
		if (this.GameObject.Enabled == true)
		{
			if (Input.Down("Attack1") && _currentAmmo > 0 && _canFire && !_isReloading)
			{
				pistolAttack();
			}

			if(Input.Pressed( "Reload" ) && _currentAmmo < maxAmmo)
			{
				_reloadStopTime = Time.Now + reloadTime;
				pistolReload();
			}

			if ( _reloadStopTime < Time.Now && _isReloading)
			{
				_isReloading = false;
				Log.Info( "Reload Complete" );
			}

			if ( _nextFire < Time.Now )
			{
				_canFire = true;
			}
		}
	}

	void pistolAttack()
	{
		mainBody.Set( "b_attack", true );
		_currentAmmo--;
		_canFire = false;
		_nextFire = Time.Now + FireRate;
		Log.Info( "Pistol Ammo Count: " + _currentAmmo );
	}

	void pistolReload()
	{
		_isReloading = true;
		mainBody.Set( "b_reload", true );
		_currentAmmo = maxAmmo;
		Log.Info( "Pistol Reloading.." );
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

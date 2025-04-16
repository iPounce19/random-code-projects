using Sandbox;
using System.Security.Cryptography;

public sealed class WeaponShotgunScript : Component
{
	[Property] SoundEvent fireSound { get; set; }
	[Property] SoundEvent reloadSound { get; set; }
	[Property] SoundEvent fireClick { get; set; }

	[Property] GameObject firePoint { get; set; }
	[Property] GameObject muzzleFlashPosition { get; set; }
	[Property] SkinnedModelRenderer mainBody { get; set; }

	[Property] float Damage { get; set; } = 8f;
	[Property] float Range { get; set; } = 100f;
	[Property] float FireRate { get; set; } = 1.5f;

	[Property] float reloadTime { get; set; } = 0.6f;
	[Property] int maxAmmo { get; set; } = 6;

	bool oneInTheChamber = false;
	bool _onEnable = false;
	bool _canFire;
	bool _isReloading = false;
	int _currentAmmo;
	float _nextFire;

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
	protected override void OnUpdate()
	{
		if(Input.Down("Attack1") && _canFire && _onEnable && _currentAmmo > 0)
		{
			shotgunAttack();
		}
		if(_nextFire <	 Time.Now)
		{
			_canFire = true;
		}
	}
	void shotgunAttack()
	{
		if ( !mainBody.IsValid() ) return;
		mainBody.Set( "b_attack", true );
		_canFire = false;
		_nextFire = Time.Now + FireRate;
		_currentAmmo--;
		Log.Info( "Shotgun Ammo Count: " + _currentAmmo );
	}

	void shotgunReload()
	{

	}
	private async void onEnableDelay()
	{
		await Task.DelaySeconds( 0.5f );
		_onEnable = true;
	}
}

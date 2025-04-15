using Sandbox;
using System.Security.Cryptography.X509Certificates;

public sealed class WeaponHolderScript : Component
{

	public enum weaponList
	{
		None = 0,
		Pistol = 1,
		Shotgun = 2,
		Melee_Punch = 3,
		Big_Pistol = 4,
	}
	[Property] SkinnedModelRenderer mainBody { get; set;}

	[Property] GameObject Shotgun { get; set;}
	[Property] GameObject Pistol {  get; set;}
	[Property] GameObject bigPistol { get; set; }
	[Property] GameObject meleePunch { get; set;}
	[Property] weaponList weaponEquip { get; set;}

	protected override void OnStart() 
	{
		//On Start Check what Weapon is Equiped
		checkWeaponHold( weaponEquip );
	}
	protected override void OnUpdate()
	{
		switch (true)
		{
			case var _ when Input.Pressed( "Holster" ):
				Log.Info( "Switch to Holster" );
				weaponEquip = weaponList.None;
				checkWeaponHold( weaponEquip );
				break;
			case var _ when Input.Pressed( "Slot1" ):
				Log.Info( "Switch to Pistol" );
				weaponEquip = weaponList.Pistol;
				checkWeaponHold( weaponEquip );
				break;
			case var _ when Input.Pressed( "Slot2" ):
				Log.Info( "Switch to Shotgun" );
				weaponEquip = weaponList.Shotgun;
				checkWeaponHold( weaponEquip );
				break;
			case var _ when Input.Pressed( "Slot3" ):
				Log.Info( "Switch to Big Pistol" );
				weaponEquip = weaponList.Big_Pistol;
				checkWeaponHold( weaponEquip );
				break;
			case var _ when Input.Pressed( "Melee" ):
				Log.Info( "Switch to Melee - Punch " );
				weaponEquip = weaponList.Melee_Punch;
				checkWeaponHold( weaponEquip );
				break;
			default:
				break;
		}
	}

	public void checkWeaponHold(weaponList weaponEquip)
	{
		// Checks if there is a SkinnedModelRenderer
		if ( !mainBody.IsValid() ) return;

		switch (weaponEquip)
		{
			case weaponList.None:
				Log.Info( "Holster" );
				mainBody.Set( "holdtype", 0 );
				changeWeapon();
				break;
			case weaponList.Pistol:
				Log.Info( "Pistol" );
				mainBody.Set( "holdtype", 1 );
				mainBody.Set( "holdtype_handedness", 1 ); // Pistol Pose
				mainBody.Set( "holdtype_attack", 1 ); //Pistol Recoil
				changeWeapon();
				break;
			case weaponList.Shotgun:
				Log.Info( "Shotgun" );
				mainBody.Set( "holdtype", 3 );
				mainBody.Set( "holdtype_handedness", 0 ); // Shotgun pose
				changeWeapon();
				break;
			case weaponList.Melee_Punch:
				Log.Info( "Melee" );
				mainBody.Set( "holdtype", 5 );
				mainBody.Set( "holdtype_handedness", 0 ); // Two Fist punchy
				changeWeapon();
				break;
			case weaponList.Big_Pistol:
				Log.Info( "Big_Pistol" );
				mainBody.Set( "holdtype", 1 );
				mainBody.Set( "holdtype_handedness", 0 ); // Pistol Pose
				mainBody.Set( "holdtype_attack", 1 ); //Pistol Recoil
				changeWeapon();
				break;
			default:
				Log.Info( "Test" );
				break;
		}
	}

	private void changeWeapon()
	{
		switch (weaponEquip)
		{
			case weaponList.None:
				Pistol.Enabled = false;
				bigPistol.Enabled = false;
				Shotgun.Enabled = false;
				meleePunch.Enabled = false;
				break;
			case weaponList.Shotgun: //Enable Shotgun
				Pistol.Enabled = false;
				bigPistol.Enabled = false;
				Shotgun.Enabled = true;
				meleePunch.Enabled = false;
				break;
			case weaponList.Pistol: //Enable Pistol
				Pistol.Enabled = true;
				bigPistol.Enabled = false;
				Shotgun.Enabled = false;
				meleePunch.Enabled = false;
				break;
			case weaponList.Melee_Punch: // Disable both Guns
				Pistol.Enabled = false;
				bigPistol.Enabled = false;
				Shotgun.Enabled = false;
				meleePunch.Enabled = true;
				break;
			case weaponList.Big_Pistol: // Enable Big Pistol
				Pistol.Enabled = false;
				bigPistol.Enabled = true;
				Shotgun.Enabled = false;
				meleePunch.Enabled = true;
				break;
			default:
				break;
		}
	}
}

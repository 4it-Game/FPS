using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public Transform weaponHold; 
	public Weapon startingWeapon;
	Weapon equippedWeapon;

	void Start(){
		if(startingWeapon != null){
			EquipWeapon (startingWeapon);
		}
	}
		
	public void EquipWeapon(Weapon gunToEquip){
		if (equippedWeapon != null){
			Destroy (equippedWeapon.gameObject);
		}
		equippedWeapon = Instantiate (gunToEquip, weaponHold.position, weaponHold.rotation) as Weapon;
		equippedWeapon.transform.parent = weaponHold;
	}

	public void Aim(bool isAim){
		if (equippedWeapon != null) {
			equippedWeapon.Aim (isAim);		
		}
	}

	public void OnTriggerHold(){
		if (equippedWeapon != null) {
			equippedWeapon.OnTriggerHold ();		
		}
	}

	public void OnTriggerRelease(){
		if (equippedWeapon != null) {
			equippedWeapon.OnTriggerRelease ();		
		}
	}

	public void LookAt(Vector3 aimPoint){
		if (equippedWeapon != null) {
			equippedWeapon.LookAt (aimPoint);	
		}
	}

	public void OnWalking(){
		if (equippedWeapon != null) {
			equippedWeapon.OnWalking ();		
		}
	}
}

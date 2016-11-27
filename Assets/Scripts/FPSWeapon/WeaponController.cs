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
}

using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {

	private string weaponLayerName = "Weapon"; 
	[SerializeField]
	private PlayerWeapon primaryWeapon;
	private PlayerWeapon currentWeapon;
	private WeaponGraphics currentGraphics;
	[SerializeField]
	private Transform weaponHolder;

	void Awake () {
		EquipWeapon (primaryWeapon);
	}
	
	void EquipWeapon(PlayerWeapon _weapon){
		currentWeapon = _weapon;
		GameObject _weaponInstance = (GameObject)Instantiate (_weapon.weaponModel, weaponHolder.position, weaponHolder.rotation);
		_weaponInstance.transform.SetParent (weaponHolder);

		currentGraphics = _weaponInstance.GetComponent<WeaponGraphics> ();
		if(currentGraphics == null){
			Debug.LogError ("WeaponMannager: No Weapon Graphic Found in weapon object " + _weaponInstance.name);
		}
		Util.SetLayerRecursively (_weaponInstance, LayerMask.NameToLayer(weaponLayerName));
	}

	public PlayerWeapon  GetCurrentWeapon(){
		return currentWeapon;
	} 

	public WeaponGraphics GetCurrentGraphics(){
		return currentGraphics;
	}
}

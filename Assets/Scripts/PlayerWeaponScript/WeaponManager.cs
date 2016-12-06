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
	[Header("Recoil")]
	public Vector2 kickMinMax = new Vector2(.05f, .02f);
	public Vector2 recoilAngleMinMax = new Vector2(3,5);
	public float recoilMoveSettleTime = .1f;
	public float recoilRotationSetTime = .1f;
	[HideInInspector]
	Vector3 aimGunMove;
	[HideInInspector]
	Vector3 recoilSmoothVelocity;
	[Header("Aim")]
	public Vector2 holdPos = new Vector2 (-0.2f, 0.05f);
	public float weaponPush = 0.2f;

	void Awake () {
		EquipWeapon (primaryWeapon);
	}

	void Update(){
		//animate the recoil
		weaponHolder.GetChild(0).transform.localPosition = Vector3.SmoothDamp(weaponHolder.GetChild(0).transform.localPosition, aimGunMove, ref recoilSmoothVelocity, recoilMoveSettleTime);
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

	public Transform Shell
	{
		get { return currentGraphics.shell; }
	}

	public Transform ShellEjection
	{
		get { return currentGraphics.shellEjection; }
	}

	public void Recoil(){
		//recoil the gun
		weaponHolder.GetChild(0).transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
	}

	public void Aim(bool isAim){
		if(isAim){
			aimGunMove = new Vector3 (holdPos.x, holdPos.y, weaponPush);
		}else{
			aimGunMove = Vector3.zero;

		}
	}
}

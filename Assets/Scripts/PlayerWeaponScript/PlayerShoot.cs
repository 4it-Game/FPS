using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

	private const string PLAYE_TAG = "Player";

	[SerializeField]
	private Camera cam;
	[SerializeField]
	private LayerMask shootMask;
	private PlayerWeapon currentWeapon;
	private WeaponManager weaponMannger; 
	PlayerWeapon.FireMode fireMode;
	bool triggerreleasedSinceLasetShot;
	int shotsreamingInBurst;


	void Start () {
		
		weaponMannger = GetComponent<WeaponManager> ();
		currentWeapon = weaponMannger.GetCurrentWeapon ();
		shotsreamingInBurst = currentWeapon.burstCount;
		fireMode = currentWeapon.fireMode;

		if (cam == null){
			Debug.LogError ("PlayerShoot: No Camera Found!");
			this.enabled = false;
		}
	}

	void Update () {

		if(PauseMenu.IsOn)
			return;

		if (Input.GetButton ("Fire1")) {
			OnTriggerHold ();
		} else {
			OnTriggerRelease ();
		}

		if (Input.GetMouseButton (1)) {
			weaponMannger.Aim (true);
		} else {
			weaponMannger.Aim (false);
		}
	}

	//call on the server when the player shoot
	[Command]
	void CmdOnShoot(){
		RpcShootEffects ();
	}

	//call on every effectable client
	[ClientRpc]
	void RpcShootEffects(){
		weaponMannger.GetCurrentGraphics ().muzzleFlash.Play ();
	}

	[Client]
	void Shoot(){
		if (!isLocalPlayer)
			return;
		//call the onShoot method on the server
		CmdOnShoot ();

		if (Time.time > currentWeapon.nextShotTime) {
			if(fireMode == PlayerWeapon.FireMode.Burst){
				if(shotsreamingInBurst == 0){
					return;
				}

				shotsreamingInBurst --;
			}else if (fireMode == PlayerWeapon.FireMode.Single){
				if(!triggerreleasedSinceLasetShot){
					return;
				}
			}

			weaponMannger.Recoil ();

			RaycastHit hit;
			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, shootMask)) {
				//call hit method on server

				if(hit.collider.tag == PLAYE_TAG){
					CmdPlayerShot (hit.collider.name, currentWeapon.damage, transform.name);
				}

				CmdOnHit (hit.point, hit.normal);
			}
		}
	}

	[Command]
	void CmdPlayerShot(string _playerId, int _damage, string _sourcePlayerID){
		Debug.Log ("Player shot " + _playerId);
		PlayerManager _player = GameManager.GetPlayer (_playerId);
		_player.RpcTakeDamage (_damage, _sourcePlayerID);
	}

	public void OnTriggerHold(){
		Shoot ();
		triggerreleasedSinceLasetShot = false;
	}

	public void OnTriggerRelease(){
		triggerreleasedSinceLasetShot = true;
		shotsreamingInBurst = currentWeapon.burstCount;
	}

	//when hit something when call on server it takes hitpoint and normal of surface
	[Command]
	void CmdOnHit(Vector3 _pos, Vector3 _normal){
		RpcDoHitEffect (_pos, _normal);
	}

	//call on every effected client
	[ClientRpc]
	void RpcDoHitEffect(Vector3 _pos, Vector3 _normal){
		GameObject _hitEffect = (GameObject)Instantiate (weaponMannger.GetCurrentGraphics().hitEfectPrefab, _pos, Quaternion.LookRotation(_normal));
		Destroy (_hitEffect, 2f);
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(WeaponManager))]
public class PlayerShoot : MonoBehaviour {

	[SerializeField]
	private Camera cam;
	[SerializeField]
	private LayerMask mask;

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

	public void Shoot(){
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

			ShootEffect ();

			weaponMannger.Recoil ();

			RaycastHit hit;
			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask)) {
				OnHit (hit.point, hit.normal);
			}
		}
	}

	public void OnTriggerHold(){
		Shoot ();
		triggerreleasedSinceLasetShot = false;
	}

	public void OnTriggerRelease(){
		triggerreleasedSinceLasetShot = true;
		shotsreamingInBurst = currentWeapon.burstCount;
	}

	void ShootEffect(){
		weaponMannger.GetCurrentGraphics ().muzzleFlash.Play ();
		Instantiate (weaponMannger.Shell, weaponMannger.ShellEjection.position, weaponMannger.ShellEjection.rotation);
	}

	void OnHit(Vector3 _pos, Vector3 _normal){
		GameObject _hitEffect = (GameObject)Instantiate (weaponMannger.GetCurrentGraphics().hitEfectPrefab, _pos, Quaternion.LookRotation(_normal));
		Destroy (_hitEffect, 2f);

		for(int i = 0; i < weaponMannger.GetCurrentGraphics().projectileSpawn.Length; i ++){
			weaponMannger.GetCurrentGraphics ().projectileSpawn [i].LookAt (_pos);
			currentWeapon.nextShotTime = Time.time + weaponMannger.msBetweenShots / 1000;
			Projectile newProjectile = Instantiate (weaponMannger.GetCurrentGraphics().projectile, weaponMannger.GetCurrentGraphics().projectileSpawn[i].position, weaponMannger.GetCurrentGraphics().projectileSpawn[i].rotation) as Projectile;
			newProjectile.SetSpeed (weaponMannger.muzzleVelocity);

		}
	}
}

using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public enum FireMode{ Auto, Burst, Single};
	public FireMode fireMode;

	public Transform[] projectileSpawn;
	public Projectile projectile;
	public float msBetweenShots = 100;
	public float muzzleVelocity = 35;
	public int burstCount;

	[Header("Recoil")]
	public Vector2 kickMinMax = new Vector2(.05f, .02f);
	public Vector2 recoilAngleMinMax = new Vector2(3,5);
	public float recoilMoveSettleTime = .1f;
	public float recoilRotationSetTime = .1f;

	Vector3 aimGunMove;

	[Header("Efects")]
	public Transform shell;
	public Transform shellEjection;
	Muzzleflash muzzleFlash;
	float nextShotTime;

	bool triggerreleasedSinceLasetShot;
	int shotsreamingInBurst;

	Vector3 recoilSmoothVelocity;
	float recoilAngle;
	float recoilRotationSmoothDumpVelocity;

	void Start(){
		muzzleFlash = GetComponent<Muzzleflash> ();
		shotsreamingInBurst = burstCount;
		aimGunMove = Vector3.zero;
	}

	void Update(){
		//animate the recoil
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition, aimGunMove, ref recoilSmoothVelocity, recoilMoveSettleTime);
//		recoilAngle = Mathf.SmoothDamp (recoilAngle, 0, ref recoilRotationSmoothDumpVelocity, recoilRotationSetTime);
//		transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;
	}

	public void Shoot(){

		if (Time.time > nextShotTime) {
			if(fireMode == FireMode.Burst){
				if(shotsreamingInBurst == 0){
					return;
				}

				shotsreamingInBurst --;
			}else if (fireMode == FireMode.Single){
				if(!triggerreleasedSinceLasetShot){
					return;
				}
			}

			for(int i = 0; i < projectileSpawn.Length; i ++){

				nextShotTime = Time.time + msBetweenShots / 1000;
				Projectile newProjectile = Instantiate (projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
				newProjectile.SetSpeed (muzzleVelocity);

			}

			Instantiate (shell, shellEjection.position, shellEjection.rotation);
			muzzleFlash.Activate ();

			//recoil the gun
			transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
//			recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
//			recoilAngle = Mathf.Clamp (recoilAngle, 0, 30);
		}
	}

	public void Aim(bool isAim){
		if(isAim){
			aimGunMove = Vector3.zero;
		}else{
			aimGunMove = new Vector3 (0.2f,0,0);
		}
	}

	public void LookAt(Vector3 aimPoint){
		transform.LookAt (aimPoint);

	}

	public void OnTriggerHold(){
		Shoot ();
		triggerreleasedSinceLasetShot = false;
	}

	public void OnTriggerRelease(){
		triggerreleasedSinceLasetShot = true;
		shotsreamingInBurst = burstCount;
	}

	public void OnWalking(){
		Debug.Log ("he is walking");
	}

}

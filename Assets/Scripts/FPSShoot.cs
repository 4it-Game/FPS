using UnityEngine;
using System.Collections;

public class FPSShoot : MonoBehaviour {

	private const string ENEMY_TAG = "Enemy";

	public FPSWeapon weapon;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	void Start () {
		if (cam == null) {
			Debug.Log ("FPSShoot: No camera referanced found!");
			this.enabled = false;
		}
	}

	void Update(){
		if (Input.GetButtonDown("Fire1")) {
			Shoot ();
		}
	}

	void Shoot(){
		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask)) {
			if (_hit.collider.tag == ENEMY_TAG){
				Debug.Log("Hit");
			}
		}
	}
}

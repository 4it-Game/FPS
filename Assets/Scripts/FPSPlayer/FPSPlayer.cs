using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FPSMotor))]
[RequireComponent(typeof(WeaponController))]
public class FPSPlayer : LivingEntity {

	// Component caching
	private FPSMotor motor;
	WeaponController gunController;
	public Camera viewCamera;

	protected override void Start (){
		base.Start ();
		motor = GetComponent<FPSMotor>();
		gunController = GetComponent<WeaponController> ();
	}

	void Update ()
	{
		//Calculate movement velocity as a 3D vector
		float _xMov = Input.GetAxis("Horizontal");
		float _zMov = Input.GetAxis("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		// Final movement vector
		Vector3 _velocity = (_movHorizontal + _movVertical);

		//Apply movement
		motor.Move(_velocity);

		//Aim input
		Ray ray = viewCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		Debug.DrawRay (ray.origin, ray.direction * 200, Color.green);

		RaycastHit hit;
		if (Physics.Raycast (ray, out hit))
			if (Input.GetMouseButton (0)) {
			Debug.Log (hit.point);
				gunController.Aim (hit.point);
				gunController.OnTriggerHold ();
			}

		//weapon input
//		if (Input.GetMouseButton(0)){
//			gunController.OnTriggerHold ();
//		}

		if (Input.GetMouseButtonUp(0)){
			gunController.OnTriggerRelease ();
		}
	}
}

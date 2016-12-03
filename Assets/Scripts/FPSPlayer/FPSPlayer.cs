using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FPSMotor))]
[RequireComponent(typeof(WeaponController))]
public class FPSPlayer : LivingEntity {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 5f;

	// Component caching
	private FPSMotor motor;
	WeaponController gunController;
	public Camera viewCamera;

	protected override void Start (){
		base.Start ();
		Cursor.lockState = CursorLockMode.Locked;
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
		Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

		//Apply movement
		motor.Move(_velocity);

		//calculate rotation using mouse turning
		float _yRot = Input.GetAxis("Mouse X") * lookSensitivity;

		// Final rotate vector
		Vector3 _rotation = new Vector3(0, _yRot, 0);
		//Apply Rotation
		motor.Rotate(_rotation);

		//calculate rotation for camera turning
		float _xRot = Input.GetAxis("Mouse Y") * lookSensitivity;

		// Final rotate vector
		Vector3 _cameraRotation = new Vector3(-_xRot, 0, 0);

		//Apply Camera Rotation
		motor.RotateCamera(_cameraRotation);


//		//gun point lookAt
//		Ray ray = viewCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
//		Debug.DrawRay (ray.origin, ray.direction * 1000, Color.green);
//
//		RaycastHit hit;
//		if (Physics.Raycast (ray, out hit)) {
//			gunController.LookAt (hit.point);		
//		}
	
		if (Input.GetMouseButton (0)) {
			gunController.OnTriggerHold ();
		}
		//weapon aim input
		gunController.Aim (Input.GetMouseButton(1));

		if (Input.GetMouseButtonUp(0)){
			gunController.OnTriggerRelease ();
		}

		//gun waking
		if(_velocity != Vector3.zero){
			gunController.OnWalking ();
		}

		//escape cursor
		if(Input.GetKeyDown("escape")){
			Cursor.lockState = CursorLockMode.None;
		}
	}
}

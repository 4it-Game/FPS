using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FPSMotor))]
//[RequireComponent(typeof(WeaponController))]
public class FPSPlayer : LivingEntity {

	[Header("Player Behavior")]
	[SerializeField]
	private float walkingSpeed = 5f;
	[SerializeField]
	private float runningSpeed = 15f;
	[SerializeField]
	private float inAirSpeed = 1f;
	[SerializeField]
	private float jumpVelocity = 20f;
	bool isGrounded = false;
	float maxSlope = 60;

	[Header("Look Behavior")]
	[SerializeField]
	private float lookSensitivity = 5f;
	[SerializeField]
	private float aimSensitivity = 1f;
	public float minimumY = -60F;
	public float maximumY = 60F;
	float rotationY = 0;
	// Component caching

	private FPSMotor motor;
//	WeaponController gunController;
	public Camera viewCamera;

	protected override void Start (){
		base.Start ();
		Cursor.lockState = CursorLockMode.Locked;
		motor = GetComponent<FPSMotor>();
//		gunController = GetComponent<WeaponController> ();
	}

	void Update ()
	{
		//Calculate movement velocity as a 3D vector
		float _xMov = Input.GetAxis("Horizontal");
		float _zMov = Input.GetAxis("Vertical");
		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		// Final movement vector
		Vector3 _velocity = (_movHorizontal + _movVertical).normalized;

		if (isGrounded) {
			motor.movementSpeed = walkingSpeed;
		} else {
			motor.movementSpeed = inAirSpeed;
		}


		if (Input.GetButton ("Run")) {
			motor.movementSpeed = runningSpeed;
		}

		//Apply movement
		motor.Move(_velocity);

		//calculate rotation using mouse turning
		float _yRot = Input.GetAxis("Mouse X") * lookSensitivity;

		// Final rotate vector
		Vector3 _rotation = new Vector3(0, _yRot, 0);
		//Apply Rotation
		motor.Rotate(_rotation);

		//calculate rotation for camera turning
		rotationY += Input.GetAxis("Mouse Y") * lookSensitivity;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		// Final rotate vector
		Vector3 _cameraRotation = new Vector3(-rotationY, viewCamera.transform.localEulerAngles.y, 0);

		//Apply Camera Rotation
		motor.RotateCamera(_cameraRotation);

		//jump
		if(Input.GetButton("Jump") && isGrounded){
			GetComponent<Rigidbody> ().AddForce (0, jumpVelocity, 0);
		}

		//escape cursor
		if(Input.GetKeyDown("escape")){
			Cursor.lockState = CursorLockMode.None;
		}
	}

	void OnCollisionStay(Collision collition){
		foreach (ContactPoint contact in collition.contacts) {
			if(Vector3.Angle(contact.normal, Vector3.up) < maxSlope){
				isGrounded = true;
			}
		}
	}

//	void Shoot(){
//		gun point lookAt
//		Ray ray = viewCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
//		Debug.DrawRay (ray.origin, ray.direction * 1000, Color.green);
//
//		RaycastHit hit;
//		if (Physics.Raycast (ray, out hit)) {
//			gunController.LookAt (hit.point);		
//		}
//
//		if (Input.GetMouseButton (0)) {
//			gunController.OnTriggerHold ();
//		}
//		//weapon aim input
//		gunController.Aim (Input.GetMouseButton(1));
//		if (Input.GetMouseButton (1)) {
//			lookSensitivity = aimSensitivity;
//		} else {
//			lookSensitivity = 5;
//		}
//
//
//		if (Input.GetMouseButtonUp(0)){
//			gunController.OnTriggerRelease ();
//		}
//
//		//gun waking
//		if(_velocity != Vector3.zero){
//			gunController.OnWalking ();
//		}
//	}

	void OnCollisionExit(){
		isGrounded = false;
	}
}

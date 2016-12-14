using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FPSMotor))]
public class FPSPlayer : MonoBehaviour {

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
	public float camYRotationLimit = 60f;
	float rotationY = 0;
	// Component caching

	private FPSMotor motor;

	void Start (){
		Cursor.lockState = CursorLockMode.Locked;
		motor = GetComponent<FPSMotor>();
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
		rotationY = Mathf.Clamp (rotationY, -camYRotationLimit, camYRotationLimit);
		// Final rotate vector
		Vector3 _cameraRotation = new Vector3(-rotationY, motor.cam.transform.localEulerAngles.y, 0);

		//Apply Camera Rotation
		motor.RotateCamera(_cameraRotation);

		//jump
		if(Input.GetButton("Jump") && isGrounded && !PauseMenu.IsOn){
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

	void OnCollisionExit(){
		isGrounded = false;
	}
}

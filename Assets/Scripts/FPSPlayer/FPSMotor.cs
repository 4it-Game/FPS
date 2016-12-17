using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FPSMotor : MonoBehaviour {

	[SerializeField]
	public Camera cam;
	[HideInInspector]
	public float movementSpeed;
	float wallkingSpeed;
	float curSpeed;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private Vector3 cameraRotation = Vector3.zero;
	private Rigidbody rb;

	void Start (){
		rb = GetComponent<Rigidbody>();
		rb.useGravity = true;
	}

	// Gets a movement vector
	public void Move (Vector3 _velocity){
		wallkingSpeed = Mathf.SmoothDamp (wallkingSpeed, movementSpeed,ref curSpeed, 1f);
		velocity = _velocity * wallkingSpeed;
	}

	// Gets a Rotation vector
	public void Rotate(Vector3 _rotation){
		rotation = _rotation;
	}

	// Gets a Camera Rotation vector
	public void RotateCamera(Vector3 _cameraRotation){
		cameraRotation = _cameraRotation;
	}

	// Run every physics iteration
	void FixedUpdate (){
		
		if (PauseMenu.IsOn) {
			if (Cursor.lockState != CursorLockMode.None)
				Cursor.lockState = CursorLockMode.None;
			return;
		}
			
		if (Cursor.lockState != CursorLockMode.Locked) {
			Cursor.lockState = CursorLockMode.Locked;
		}
		
		PerformMovement();
		PerformRotation ();
	}

	//Perform movement based on velocity variable
	void PerformMovement (){
		if (velocity != Vector3.zero)
		{
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}

	}

	//player rotation
	void PerformRotation(){
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		if(cam != null){
			cam.transform.localEulerAngles = cameraRotation;
		}
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FPSMotor))]
public class FPSController : MonoBehaviour {

	// Component caching
	private FPSMotor motor;

	void Start ()
	{
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
		Vector3 _velocity = (_movHorizontal + _movVertical);

		//Apply movement
		motor.Move(_velocity);

	}
}

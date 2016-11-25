using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FPSMotor : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;

	private Vector3 velocity = Vector3.zero;

	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Gets a movement vector
	public void Move (Vector3 _velocity)
	{
		velocity = _velocity * speed;
	}

	// Run every physics iteration
	void FixedUpdate ()
	{
		PerformMovement();
	}

	//Perform movement based on velocity variable
	void PerformMovement ()
	{
		if (velocity != Vector3.zero)
		{
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}

	}
}

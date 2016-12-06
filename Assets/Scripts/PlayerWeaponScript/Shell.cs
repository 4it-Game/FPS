using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour {

	public Rigidbody rb;
	public float forceMin;
	public float forceMax;

	void Start(){
		float force = Random.Range (forceMin, forceMax);
		rb.AddForce (transform.right * force);
		rb.AddTorque (Random.insideUnitSphere * force);
		Destroy (gameObject, 5f);
	}
}

using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;
	public Color trailColor;
	float speed = 10;

	float lifetime = 3;

	public float maxDist = 1000000;
	public GameObject decolHitWall;
	public float floatInFrontOfWall = 0.01f;

	void Start(){
		Destroy (gameObject, lifetime);

		GetComponent<TrailRenderer> ().material.SetColor("_TintColor", trailColor);
	}

	public void SetSpeed(float newSpeed){
		speed = newSpeed;
	}

	void Update () {
		float moveDitance = speed * Time.deltaTime;
		CheckCollition (moveDitance);
		transform.Translate (Vector3.forward * moveDitance); 
	}

	void CheckCollition(float moveDistance){
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)){
			OnHitObject (hit);	
		}

		if(Physics.Raycast(transform.position, transform.forward, out hit, maxDist)){
			if(decolHitWall && hit.transform.tag == "Untagged"){
				Instantiate (decolHitWall, hit.point + (hit.normal * floatInFrontOfWall), Quaternion.LookRotation(hit.normal));
				GameObject.Destroy (gameObject);
			}
		}
	}

	void OnHitObject(RaycastHit hit){
		//damge

		GameObject.Destroy (gameObject);
	}
}

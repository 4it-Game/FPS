using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;
	public Color trailColor;
	float speed = 10;
	float damage = 1;

	float lifetime = 3;

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
	}

	void OnHitObject(RaycastHit hit){
		IDamageable damagebaleObject = hit.collider.GetComponent<IDamageable> ();
		if(damagebaleObject != null){
			damagebaleObject.TakeHit (damage, hit);
		}
		GameObject.Destroy (gameObject);
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

	NavMeshAgent pathFinder;
	Transform target;

	protected override void Start () {
		base.Start ();
		pathFinder = GetComponent<NavMeshAgent> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		StartCoroutine (UpdatePath());
	}

	IEnumerator UpdatePath(){
		float refreashRate = 1;

		while (target != null){
			Vector3 targetPosition = new Vector3 (target.position.x - 2,0,target.position.z - 2);
			if(!dead){
				pathFinder.SetDestination (targetPosition);
			}
			yield return new WaitForSeconds (refreashRate);
		}
	}
}

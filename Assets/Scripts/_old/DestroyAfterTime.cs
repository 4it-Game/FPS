using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	[SerializeField]
	float lifeTime = 4;

	void Start () {
		StartCoroutine (Fade());
	}

	IEnumerator Fade(){
		yield return new WaitForSeconds (lifeTime);
		GameObject.Destroy (gameObject);
	}
}

using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour {

	public Rigidbody rb;
	public float forceMin;
	public float forceMax;

	float lifeTime = 4;
	float fadeTime = 2;

	void Start(){
		float force = Random.Range (forceMin, forceMax);
		rb.AddForce (transform.right * force);
		rb.AddTorque (Random.insideUnitSphere * force);

		StartCoroutine (Fade());
	}

	IEnumerator Fade(){
		yield return new WaitForSeconds (lifeTime);

		float precent = 0;
		float fadeSpeed = 1 / fadeTime;
		Material mat = GetComponent<Renderer> ().material;
		Color initialColor = mat.color;

		while (precent < 1){
			precent += Time.deltaTime * fadeSpeed;
			mat.color = Color.Lerp (initialColor,Color.clear, precent);
			yield return null;
		}
		Destroy (gameObject);
	}

}

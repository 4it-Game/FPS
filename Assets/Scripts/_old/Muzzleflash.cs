using UnityEngine;
using System.Collections;

public class Muzzleflash : MonoBehaviour {

	public GameObject flashHolder;
	public Sprite[] flashSprites;
	public SpriteRenderer[] spriteRenders;
	public float flashTime;

	void Start(){
		DeActivate ();
	}

	public void Activate(){
		flashHolder.SetActive (true);

		int flashSpriteIndex = Random.Range (0, flashSprites.Length);
		for(int i = 0; i < spriteRenders.Length; i ++){
			spriteRenders [i].sprite = flashSprites [flashSpriteIndex];
		}

		Invoke ("DeActivate", flashTime);
	}

	public void DeActivate(){
		flashHolder.SetActive (false);
	}
}

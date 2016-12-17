using UnityEngine;
using System.Collections;

public class KillFeed : MonoBehaviour {

	[SerializeField]
	GameObject killfeedItemPrefab;

	void Start () {
		GameManager.singelton.onPlayerCallback += OnKill;
	}

	public void OnKill(string player, string source){
		GameObject itemGo = (GameObject)Instantiate (killfeedItemPrefab, this.transform);
		itemGo.GetComponent<KillFeedItem> ().Setup (player, source);

		Destroy (itemGo.gameObject, 4f);
	}
}

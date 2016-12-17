using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

	[SerializeField]
	GameObject playerScoreboardItem;
	[SerializeField]
	Transform playerScoreList;

	void OnEnable(){
		PlayerManager[] players = GameManager.GetAllPlayers ();

		foreach(PlayerManager player in players){
			GameObject itemGO = (GameObject)Instantiate (playerScoreboardItem, playerScoreList);
			PlayerScoreItem item = itemGO.GetComponent<PlayerScoreItem> ();
			if(item != null){
				item.Setup (player.username, player.kills.ToString(), player.deaths.ToString());
			}
		}
	}

	void OnDisable(){
		foreach(Transform child in playerScoreList){
			Destroy (child.gameObject);
		}
	}
}

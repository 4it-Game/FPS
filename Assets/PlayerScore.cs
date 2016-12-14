using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerManager))]
public class PlayerScore : MonoBehaviour {

	PlayerManager player;

	void Start () {
		player = GetComponent<PlayerManager> ();
		StartCoroutine (SyncScoreLoop());
	}

	void OnDestroy(){
		if(player != null)
			SyncNow ();
	}

	IEnumerator SyncScoreLoop(){
		while(true){

			yield return new WaitForSeconds (5f);

			SyncNow ();
		}
	}

	void SyncNow(){
		if(UserAccountManager.IsLoggedIn){
			UserAccountManager.instance.GetData (OnDataRecieved);
		}
	}

	void OnDataRecieved(string data){
		
		if (player.kills == 0 && player.deaths == 0)
			return;
		
		int kills = DataTranslater.DataToKill (data);
		int deaths = DataTranslater.DataToDeaths (data);

		int newKills = player.kills + kills;
		int newDeaths = player.deaths + deaths;

		string newData = DataTranslater.VAluesToData (newKills, newDeaths);

		Debug.Log ("Syncing: " + newData);

		player.kills = 0;
		player.deaths = 0;

		UserAccountManager.instance.SendData (newData);
	}
}

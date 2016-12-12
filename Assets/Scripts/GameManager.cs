using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager singelton;

	public MatchSettings matchSettings;

	[SerializeField]
	private GameObject gameCamera;

	void Awake(){
		if (singelton != null) {
			Debug.Log ("More than one game mannager in the scene");
		} else {
			singelton = this;
		}
	}

	public void SetGameCAmeraActive(bool isActive){
		if(gameCamera == null){
			return;
		}

		gameCamera.SetActive (isActive);
	}

	#region Player registering

	private const string PLAYER_ID_PREFIX = "Player ";

	private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager> ();

	public static void RegisterPlayer(string _netID, PlayerManager _player){
		string _player_ID = PLAYER_ID_PREFIX + _netID;
		players.Add (_player_ID, _player);
		_player.transform.name = _player_ID;
	}

	public static void UnRegistationPlayer(string _playerID){
		players.Remove (_playerID);
	}

	public static PlayerManager GetPlayer(string _playerId){
		return players[_playerId];
	}

//	void OnGUI(){
//		GUILayout.BeginArea (new Rect(100,300, 100, 500));
//		GUILayout.BeginVertical ();
//		foreach(string _playerID in players.Keys){
//			GUILayout.Label (_playerID + "-" + players[_playerID].transform.name);
//		}
//		GUILayout.EndVertical ();
//		GUILayout.EndArea ();
//	}

	#endregion
}

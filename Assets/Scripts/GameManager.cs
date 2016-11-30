using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private const string PLAYER_ID_PREFIX = "Player ";

	private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager> ();

	public static void RegisterPlayer(string _serverId, PlayerManager _player){

		string _playerID = PLAYER_ID_PREFIX + _serverId;
		players.Add (_playerID, _player);
		_player.transform.name = _playerID;
	}

	public static void UnRegisterPlayer (string _playerID) {
		players.Remove (_playerID);
	}


}

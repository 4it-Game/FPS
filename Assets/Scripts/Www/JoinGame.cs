﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;

public class JoinGame : MonoBehaviour {

	List<GameObject> roomlist = new List<GameObject> ();

	[SerializeField]
	private Text status;

	[SerializeField]
	private GameObject roomListItemPrefab;
	[SerializeField]
	private Transform roomListParent;

	private NetworkManager networkManager;

	void Start(){
		networkManager = NetworkManager.singleton;
		if(networkManager.matchMaker == null){
			networkManager.StartMatchMaker ();
		}

		RefreshRoomList ();
	}

	public void RefreshRoomList(){
		ClearRoomList ();

		if(networkManager.matchMaker == null){
			networkManager.StartMatchMaker ();
		}

		networkManager.matchMaker.ListMatches (0, 20, "", true, 0, 0, OnMatchList);
		status.text = "Loading...";
	}

	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList){
		status.text = "";

		if (!success || matchList == null){
			status.text = "Couldn't connect to matchmaking, reason: " + extendedInfo + ".";
			return;
		}

		foreach (MatchInfoSnapshot match in matchList){
			GameObject _roomListItem = Instantiate (roomListItemPrefab);
			_roomListItem.transform.SetParent (roomListParent);

			RoomListItem roomListItem = _roomListItem.GetComponent<RoomListItem> ();
			if(roomListItem != null){
				roomListItem.Setup (match, JoinRoom);
			}
			roomlist.Add (_roomListItem);
		}

		if(roomlist.Count == 0){
			status.text = "No rooms at the server right now.";
		}
	}

	void ClearRoomList(){
		for(int i = 0; i < roomlist.Count; i++){
			Destroy (roomlist[i]);
		}

		roomlist.Clear ();
	}

	public void JoinRoom (MatchInfoSnapshot _match)
	{
		networkManager.matchMaker.JoinMatch (_match.networkId, "", "127.0.0.1", "", 0, 1, networkManager.OnMatchJoined);
		StartCoroutine (WaitForJoin());
	}

	IEnumerator WaitForJoin(){
		ClearRoomList ();

		int joinCountDown = 10;
		while(joinCountDown > 0){
			status.text = "JOINING...(" + joinCountDown + ")";
			yield return new WaitForSeconds (1);
			joinCountDown--;
		}

		status.text = "Faild to Connect";
		yield return new WaitForSeconds (1);

		MatchInfo matchInfo = networkManager.matchInfo;
		if(matchInfo != null){
			networkManager.matchMaker.DropConnection (matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
			networkManager.StopHost ();	
		}

		RefreshRoomList ();
	}
}

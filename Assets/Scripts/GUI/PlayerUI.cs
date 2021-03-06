﻿using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	GameObject pauseMenu;

	[SerializeField]
	GameObject scoreboard;

	void Start(){
		PauseMenu.IsOn = false;
		pauseMenu.SetActive (false);
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			TogglePauseMenu ();
		}

		if (Input.GetKeyDown (KeyCode.Tab)) {
			scoreboard.SetActive (true);
		} else if(Input.GetKeyUp (KeyCode.Tab)){
			scoreboard.SetActive (false);
		}
	}

	public void TogglePauseMenu (){
		pauseMenu.SetActive (!pauseMenu.activeSelf);
		PauseMenu.IsOn = pauseMenu.activeSelf;
	}
}

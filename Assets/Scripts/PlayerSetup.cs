using UnityEngine; 
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerManager))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;
	[SerializeField]
	string remoteLayerName = "RemotePlayer";
	[SerializeField]
	string dontDrawLayerName = "DontDraw";
	[SerializeField]
	GameObject playerGrahpic;
	[SerializeField]
	GameObject playerUIPrefabs;
	[HideInInspector]
	public GameObject playerUIInstance;

	void Start(){
		if (!isLocalPlayer) {
			DisableComponents ();
			AssingRemotelayer ();
		} else {
			//disable player grahpics
			Util.SetLayerRecursively(playerGrahpic, LayerMask.NameToLayer(dontDrawLayerName));
			//Draw player ui
			playerUIInstance = Instantiate(playerUIPrefabs);
			playerUIInstance.name = playerUIPrefabs.name;
			//SetPlayerUI
			PlayerUI HUD = playerUIInstance.GetComponent<PlayerUI>();
			if(HUD == null)
				Debug.LogError ("No HUD Found In PlayerUI Prefab");
			//SetPlayerUI
//			PlayerUI playerUI = playerUIInstance.GetComponent<PlayerUI>();
//			if(HUD == null)
//				Debug.LogError ("No HUD Found In PlayerUI Prefab");
//			
			GetComponent<PlayerManager> ().SetupPlayer();
		}
	}

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		string _netID = GetComponent<NetworkIdentity> ().netId.ToString();

		PlayerManager _player = GetComponent<PlayerManager> ();

		GameManager.RegisterPlayer (_netID, _player);
	} 

	void AssingRemotelayer(){
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName );
	}

	void DisableComponents(){
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable [i].enabled = false;
		}
	}

	void OnDisable(){

		Destroy (playerUIInstance); 

		if(isLocalPlayer)
			GameManager.singelton.SetGameCAmeraActive (true);

		GameManager.UnRegistationPlayer (transform.name);
	}
}

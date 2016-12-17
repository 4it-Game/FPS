using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class PlayerManager : NetworkBehaviour {

	[SyncVar]
	private bool _isDead = false;

	public bool isDead{
		get { return _isDead;}
		protected set{ _isDead = value;}
	}

	[SerializeField]
	private int maxHealth = 100;
	[SyncVar]
	private int currentHelath;

	[SyncVar]
	public string username = "Loading...";

	public int kills;
	public int deaths;

	[SerializeField]
	private Behaviour[] disableWhenDeath;
	[SerializeField]
	private GameObject[] disableObjectsWhenDeath;
	private bool[] wasEnabled;

	[SerializeField]
	private GameObject deathEffect;

	[SerializeField]
	private GameObject spawnEffect; 

	private bool firestSetup = true;

	public void SetupPlayer(){

		if(isLocalPlayer){
			//switch cameara
			GameManager.singelton.SetGameCAmeraActive (false);
			GetComponent<PlayerSetup> ().playerUIInstance.SetActive (true);
		}

		CmdBroadcastNewPlayerSetup ();
	}

	[Command]
	private void CmdBroadcastNewPlayerSetup(){
		RpcSetupPlayerOnAllClients();
	}

	[ClientRpc]
	private void RpcSetupPlayerOnAllClients(){
		if(firestSetup){
			wasEnabled = new bool[disableWhenDeath.Length];
			for(int i = 0; i < wasEnabled.Length; i++){
				wasEnabled [i] = disableWhenDeath [i].enabled;
			}

			firestSetup = false;
		}

		SetDefaults ();	
	}

	public void SetDefaults(){
		isDead = false;
		currentHelath = maxHealth;
		for(int i=0; i < disableWhenDeath.Length; i++){
			disableWhenDeath [i].enabled = wasEnabled [i];
		}

		//enable objects
		for(int i = 0; i < disableObjectsWhenDeath.Length; i++){
			disableObjectsWhenDeath [i].SetActive (true);
		}

		Collider _col = GetComponent<Collider> ();
		if(_col != null){
			_col.enabled = true;
		}

		//effect swhen player spwan
		GameObject _spawnEffect = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
		Destroy (_spawnEffect, 3f);
	}

	[ClientRpc]
	public void RpcTakeDamage(int _amount, string _sourcePlayerID){

		if(isDead){
			return;
		}

		currentHelath -= _amount;

		Debug.Log (transform.name + " now has " + currentHelath + " health");

		if(currentHelath <= 0){
			Die (_sourcePlayerID);
		}
	}

	private void Die(string _sourceID){
		isDead = true;
		PlayerManager _sourcePlayer = GameManager.GetPlayer (_sourceID);
		if (_sourcePlayer != null) {
			_sourcePlayer.kills++;
			GameManager.singelton.onPlayerCallback.Invoke (username, _sourcePlayer.username);
		}

		deaths++;
		//Disable component
		for(int i = 0; i < disableWhenDeath.Length; i++){
			disableWhenDeath [i].enabled = false;
		}
		//Disable objects
		for(int i = 0; i < disableObjectsWhenDeath.Length; i++){
			disableObjectsWhenDeath [i].SetActive (false);
		}
			
		//Disable colliders
		Collider _col = GetComponent<Collider> ();
		if(_col != null){
			_col.enabled = false;
		}

		//switch cameara
		if(isLocalPlayer){
			GameManager.singelton.SetGameCAmeraActive (true);
			GetComponent<PlayerSetup> ().playerUIInstance.SetActive (false);
		}

		Debug.Log(transform.name + " isDead.");

		//spawn death effect
		GameObject _deathEffect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy (_deathEffect, 3f);
		//respwan
		StartCoroutine(Respwan());

	}

	IEnumerator Respwan(){
		yield return new WaitForSeconds (GameManager.singelton.matchSettings.respwanTime);

		Transform _startSpwanPoint = NetworkManager.singleton.GetStartPosition ();
		transform.position = _startSpwanPoint.position;
		transform.rotation = _startSpwanPoint.rotation;

		yield return new WaitForSeconds (0.1f);

		SetupPlayer ();

		Debug.Log(transform.name + " Respwaned.");
	}
}

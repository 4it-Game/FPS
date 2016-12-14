using UnityEngine;
using System.Collections;
using DatabaseControl;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour {

	public static UserAccountManager instance;

	void Awake(){
		if(instance != null){
			Destroy (gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad (this);
	}

	public static string loggedIn_Username { get; protected set;} //stores username once logged in
	private static string loggedIn_Password = ""; //stores password once logged in

	public static bool IsLoggedIn { get; protected set;}

	public string loggedInScene = "Lobby"; 
	public string loggedOutScene = "Login"; 

	public delegate void OnReceivedDataCallback(string data);

	public void LogOut(){
		loggedIn_Username = "";
		loggedIn_Password = "";

		IsLoggedIn = false;

		Debug.Log ("User LogOut.");

		SceneManager.LoadScene (loggedOutScene);
	}

	public void LogIn(string username, string password){
		loggedIn_Username = username;
		loggedIn_Password = password;

		IsLoggedIn = true;

		Debug.Log ("Logged in as : " + username);

		SceneManager.LoadScene (loggedInScene);
	}

	public void SendData (string data) { //called when the 'Send Data' button on the data part is pressed
		if (IsLoggedIn) {
			//ready to send request
			StartCoroutine (sendSendDataRequest (loggedIn_Username, loggedIn_Password, data)); //calls function to send: send data request
		}
	}

	IEnumerator sendSendDataRequest(string username, string password, string data) {

		IEnumerator eee = DC.SetUserData (username, password, data);
		while (eee.MoveNext()) {
			yield return eee.Current;
		}
		WWW returneddd = eee.Current as WWW;
		if (returneddd.text == "ContainsUnsupportedSymbol") {
			//One of the parameters contained a - symbol
			Debug.Log ("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
		}
		if (returneddd.text == "Error") {
			//Error occurred. For more information of the error, DC.Login could
			//be used with the same username and password
			Debug.Log ("Data Upload Error: Contains Unsupported Symbol '-'");
		}
	}

	public void GetData (OnReceivedDataCallback OnDataReceived) { //called when the 'Get Data' button on the data part is pressed

		if (IsLoggedIn) {
			//ready to send request
			StartCoroutine (sendGetDataRequest (loggedIn_Username, loggedIn_Password, OnDataReceived)); //calls function to send get data request
		}
	}

	IEnumerator sendGetDataRequest(string username, string password, OnReceivedDataCallback OnDataReceived) {

		string data = "ERROR";

		IEnumerator eeee = DC.GetUserData (username, password);
		while (eeee.MoveNext()) {
			yield return eeee.Current;
		}
		WWW returnedddd = eeee.Current as WWW;
		if (returnedddd.text == "Error") {
			//Error occurred. For more information of the error, DC.Login could
			//be used with the same username and password
			Debug.Log ("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
		} else {
			if (returnedddd.text == "ContainsUnsupportedSymbol") {
				//One of the parameters contained a - symbol
				Debug.Log ("Get Data Error: Contains Unsupported Symbol '-'");
			} else {
				//Data received in returned.text variable
				string DataRecieved = returnedddd.text;
				data = DataRecieved;
			}
		}
		if(OnDataReceived != null)
			OnDataReceived.Invoke (data);
	}
}

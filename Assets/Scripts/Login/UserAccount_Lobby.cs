using UnityEngine;
using UnityEngine.UI;

public class UserAccount_Lobby : MonoBehaviour {

	public Text userName;

	void Start(){
		if(UserAccountManager.IsLoggedIn)
			userName.text = UserAccountManager.loggedIn_Username;
		

	}

	public void LogOut(){
		if(UserAccountManager.IsLoggedIn)
			UserAccountManager.instance.LogOut();
	}
}

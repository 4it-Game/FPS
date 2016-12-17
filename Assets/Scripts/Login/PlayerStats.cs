using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

	public Text killCount;
	public Text deathCount;

	void Start () {
		if(UserAccountManager.IsLoggedIn)
			UserAccountManager.instance.GetData (OnReceivedData);
	}

	void OnReceivedData(string data){
		if(killCount == null || deathCount == null){
			return;
		}

		killCount.text = DataTranslater.DataToKill(data).ToString() + " kills";
		deathCount.text = DataTranslater.DataToDeaths(data).ToString() + " deaths";
	}
}

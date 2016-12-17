using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreItem : MonoBehaviour {

	[SerializeField]
	Text usernameTxt;
	[SerializeField]
	Text killsTxt;
	[SerializeField]
	Text deathsTxt;

	public void Setup(string username, string kills, string deaths){
		usernameTxt.text = username;
		killsTxt.text = kills;
		deathsTxt.text = deaths;
	}
}

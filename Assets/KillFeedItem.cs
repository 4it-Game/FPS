using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour {

	[SerializeField]
	Text text;

	public void Setup(string player, string source){
		text.text = source + " killed " + "<color=red>" + player + "</color>";
	}
}

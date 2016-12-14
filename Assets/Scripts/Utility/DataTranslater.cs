using UnityEngine;
using System;

public class DataTranslater : MonoBehaviour {

	private static string KILLS_SYMBOL = "[KILL]";
	private static string DEATH_SYMBOL = "[DEATH]";

	public static string VAluesToData(int kills, int deaths){
		return KILLS_SYMBOL + kills + "/" + DEATH_SYMBOL + deaths;
	}

	public static int DataToKill (string data){
		return int.Parse(DataToValue (data, KILLS_SYMBOL));
	}

	public static int DataToDeaths(string data){
		return int.Parse(DataToValue (data, DEATH_SYMBOL));
	}

	private static string DataToValue(string data, string symbol){
		string[] pieces = data.Split ('/');
		foreach(string piece in pieces){
			if (piece.StartsWith (symbol)) {
				return piece.Substring (symbol.Length);
			}
		}

		Debug.LogError (data + " not found in data " + data);
		return "";
	}
}

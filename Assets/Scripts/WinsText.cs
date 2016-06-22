using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinsText : MonoBehaviour {

	private Text text;

	void Start () {
		text = GetComponent<Text> ();
		Debug.Log ("won: " + PlayerPrefs.GetInt ("games_won_count"));
		text.text = PlayerPrefs.GetInt ("games_won_count").ToString();
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LossesText : MonoBehaviour {

	private Text text;

	void Start () {
		text = GetComponent<Text> ();
		Debug.Log ("lost: " + PlayerPrefs.GetInt ("games_lost_count"));
		text.text = PlayerPrefs.GetInt ("games_lost_count").ToString();
	}
}


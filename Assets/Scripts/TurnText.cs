using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TurnText : MonoBehaviour {

	private Text turnText;

	private void Start () {
		turnText = GetComponent<Text> ();
		UpdateDisplay (true);
	}
	
	public void UpdateDisplay (bool isWhiteTurn) {
		turnText.text = isWhiteTurn ? "White turn" : "Black turn";
	}
}

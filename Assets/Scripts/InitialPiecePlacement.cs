using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InitialPiecePlacement : MonoBehaviour {

	private Text text;

	private void Start() {
		text = GetComponent<Text> ();
		text.text = "Click the board to place your King.";
	}

	public void UpdateText(string displayString) {
		text.text = displayString;
	}
}

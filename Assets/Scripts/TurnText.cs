using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TurnText : MonoBehaviour {

	private Text turnText;
	private BoardManager boardManager;
	private bool isWhiteTurn = true;

	private void Start () {
		turnText = GetComponent<Text> ();
		boardManager = GameObject.FindObjectOfType<BoardManager>();
	}
	
	private void Update () {
		if (boardManager.isWhiteTurn != isWhiteTurn) {
			isWhiteTurn = boardManager.isWhiteTurn;
			UpdateText ();
		}
	}
	private void UpdateText() {
		turnText.text = isWhiteTurn ? "White turn" : "Black turn";
	}
}

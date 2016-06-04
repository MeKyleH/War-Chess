using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TurnText : MonoBehaviour {

	private Text turnText;
	private TurnManager turnManager;

	private void Start () {
		turnManager = GameObject.FindObjectOfType<TurnManager> ();
		if (!turnManager) {
			Debug.Log (name + " couldn't find turnManager.");
		}
		turnText = GetComponent<Text> ();
		UpdateDisplay (turnManager.isWhiteTurn);
	}
	
	public void UpdateDisplay (bool isWhiteTurn) {
		if (turnText) {
			turnText.text = isWhiteTurn ? "White turn" : "Black turn";
		}
	}


}

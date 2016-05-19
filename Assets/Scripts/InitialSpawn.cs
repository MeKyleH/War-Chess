using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InitialSpawn : MonoBehaviour {

	private BoardManager boardManager;
	private Text text;
	private string displayTextString = "Choose where to place King.";

	private void Start () {
		boardManager = GameObject.FindObjectOfType<BoardManager> ();
		text = GetComponent<Text> ();
		if (!boardManager) {
			Debug.Log (name + " couldn't find boardManager");
		}
		if (!text) {
			Debug.Log (name + " couldn't find text");
		}
		//WHITE PLACEMENT
		UpdateDisplay (displayTextString);
	}


	private void UpdateDisplay(string newText) {
		text.text = newText;
	}
}

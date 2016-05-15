using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Button : MonoBehaviour {

	public Defender whiteDefenderPrefab;
	public Defender blackDefenderPrefab;
	public static Defender selectedDefender;

	private Text costText;
	private Button[] buttonArray;
	private BoardManager boardManager;

	void Start() {
		buttonArray = GameObject.FindObjectsOfType<Button> ();
		costText = GetComponentInChildren<Text> ();
		boardManager = GameObject.FindObjectOfType<BoardManager> ();
		if (!costText) {
			Debug.LogWarning (name + " has no cost");
		}
		if (!boardManager) {
			Debug.Log(name + " couldn't find boardManager");
		}
		costText.text = whiteDefenderPrefab.goldCost.ToString();
	}

	public void SelectThisButton() {
		foreach (Button thisButton in buttonArray) {
			thisButton.GetComponent<Image> ().color = Color.black;
		}
		GetComponent<Image> ().color = Color.white;
		selectedDefender = boardManager.isWhiteTurn ? whiteDefenderPrefab : blackDefenderPrefab;
	}
}

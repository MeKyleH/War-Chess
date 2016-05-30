using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Button : MonoBehaviour {

	public Defender whiteDefenderPrefab;
	public Defender blackDefenderPrefab;
	public static Defender selectedDefender;

	private Text costText;
	private Button[] buttonArray;
	private TurnManager turnManager;

	void Start() {
		buttonArray = GameObject.FindObjectsOfType<Button> ();
		costText = GetComponentInChildren<Text> ();
		turnManager = GameObject.FindObjectOfType<TurnManager> ();
		if (!costText) {
			Debug.LogWarning (name + " has no cost");
		}
		if (!turnManager) {
			Debug.Log(name + " couldn't find turnManager");
		}
		costText.text = whiteDefenderPrefab.goldCost.ToString();
	}

	public void SelectThisButton() {
		foreach (Button thisButton in buttonArray) {
			thisButton.GetComponent<Image> ().color = Color.black;
		}
		GetComponent<Image> ().color = Color.white;
		selectedDefender = turnManager.isWhiteTurn ? whiteDefenderPrefab : blackDefenderPrefab;
	}
}

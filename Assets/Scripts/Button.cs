using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Button : MonoBehaviour {

	public GameObject whiteDefenderPrefab; 
	public static GameObject selectedDefender;

	private Text costText;
	private Button[] buttonArray;

	void Start() {
		buttonArray = GameObject.FindObjectsOfType<Button> ();
		costText = GetComponentInChildren<Text> ();
		if (!costText) {
			Debug.LogWarning (name + " has no cost");
		}
		costText.text = whiteDefenderPrefab.GetComponent<Defender>().goldCost.ToString();
	}

	void OnMouseDown() {
		foreach (Button thisButton in buttonArray) {
			thisButton.GetComponent<SpriteRenderer> ().color = Color.black;
		}
		GetComponent<SpriteRenderer> ().color = Color.white;
		selectedDefender = whiteDefenderPrefab;
	}

	void SelectThisButton() {
		foreach (Button thisButton in buttonArray) {
			thisButton.GetComponent<SpriteRenderer> ().color = Color.black;
		}
		GetComponent<SpriteRenderer> ().color = Color.white;
		selectedDefender = whiteDefenderPrefab;
	}
}

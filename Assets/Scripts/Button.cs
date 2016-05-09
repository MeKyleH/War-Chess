using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Button : MonoBehaviour {

	public GameObject defenderPrefab; 
	public static GameObject selectedDefender;

	private Text costText;
	private Button[] buttonArray;

	void Start() {
		buttonArray = GameObject.FindObjectsOfType<Button> ();
		costText = GetComponentInChildren<Text> ();

		if (!costText) {
			Debug.LogWarning (name + " has no cost");
		}
		costText.text = defenderPrefab.GetComponent<Defender>().starCost.ToString();
	}

	void OnMouseDown() {
		foreach (Button thisButton in buttonArray) {
			thisButton.GetComponent<SpriteRenderer> ().color = Color.black;
		}
		GetComponent<SpriteRenderer> ().color = Color.white;
		selectedDefender = defenderPrefab;
	}
}

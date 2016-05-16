using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
public class GoldDisplay : MonoBehaviour {
	public enum Status {SUCCESS, FAILURE};

	private Text goldText;
	private int whiteGold = 15;
	private int blackGold = 15;
	private bool isWhiteTurn = true;
	private BoardManager boardManager;

	// Use this for initialization
	void Start () {
		boardManager = GameObject.FindObjectOfType<BoardManager> ();
		if (!boardManager) {
			Debug.Log (name + " couldn't find boardManager");
		}
		goldText = GetComponent<Text> ();
		UpdateDisplay ();

	}

	void Update() {
		if (boardManager.isWhiteTurn != isWhiteTurn) {
			isWhiteTurn = !isWhiteTurn;
			UpdateDisplay ();
		}
	}

	public void AddGold(int amount) {
		if (boardManager.isWhiteTurn) {
			whiteGold += amount;
		} else {
			blackGold += amount;
		}
		UpdateDisplay ();
	}

	public Status UseGold(int amount) {
		if (boardManager.isWhiteTurn) {
			if (whiteGold >= amount) {
				whiteGold -= amount;
				UpdateDisplay ();
				return Status.SUCCESS;
			}
		} else {
			if (blackGold >= amount) {
				blackGold -= amount;
				UpdateDisplay ();
				return Status.SUCCESS;
			}
		}
		return Status.FAILURE;
	}

	public void ResetGold() {
		whiteGold = 15;
		blackGold = 15;
		UpdateDisplay ();
	}

	private void UpdateDisplay(){
		goldText.text = boardManager.isWhiteTurn ? whiteGold.ToString () : blackGold.ToString();
	}
}

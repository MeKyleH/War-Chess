using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour {

	public GameObject buyButton;
	public GameObject buyMenu;
	public GameObject takeGoldButton;
	public GoldDisplay goldDisplay;
	public GameObject moveButton;

	public bool isBuyMode = false;

	private BoardManager boardManager;

	private void Start() {
		boardManager = GameObject.FindObjectOfType<BoardManager> ();
		if (!boardManager) {
			Debug.Log (name + " couldn't find boardManager");
		}
	}

	public void OpenBuyMenu() {
		buyMenu.SetActive (true);
		moveButton.SetActive (true);
		buyButton.SetActive (false);
		isBuyMode = true;
	}

	public void MovePiece() {
		buyButton.SetActive (true);
		buyMenu.SetActive (false);
		moveButton.SetActive (false);
		isBuyMode = false;
	}

	public void TakeGold() {
		goldDisplay.AddGold (1);
		isBuyMode = false;
		EndTurn ();
	}

	public void EndTurn () {
		boardManager.isWhiteTurn = !boardManager.isWhiteTurn;
		boardManager.EnPassantMove.SetValue (-1, 0);
		boardManager.UnselectChessman ();
	}
}

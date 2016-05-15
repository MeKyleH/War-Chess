using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour {

	public GameObject buyButton;
	public GameObject buyMenu;
	public GameObject takeGoldButton;
	public GoldDisplay goldDisplay;
	public GameObject moveButton;

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
		boardManager.isBuyMode = true;
	}

	public void MovePiece() {
		buyButton.SetActive (true);
		buyMenu.SetActive (false);
		moveButton.SetActive (false);
		boardManager.isBuyMode = false;
	}

	public void TakeGold() {
		goldDisplay.AddGold (1);
		buyButton.SetActive (true);
		buyMenu.SetActive (false);
		moveButton.SetActive (false);
		boardManager.isBuyMode = false;
		EndTurn ();
	}

	public void EndTurn () {
		boardManager.isWhiteTurn = !boardManager.isWhiteTurn;
		boardManager.EnPassantMove.SetValue (-1, 0);
		boardManager.UnselectChessman ();
		MovePiece ();
	}
}

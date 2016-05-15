using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour {

	public GameObject buyButton;
	public GameObject buyMenu;
	public GameObject takeGoldButton;
	public GoldDisplay goldDisplay;
	public GameObject moveButton;

	public void OpenBuyMenu() {
		buyMenu.SetActive (true);
		moveButton.SetActive (true);
		buyButton.SetActive (false);
	}

	public void MovePiece() {
		buyButton.SetActive (true);
		buyMenu.SetActive (false);
		moveButton.SetActive (false);
	}

	public void TakeGold() {
		goldDisplay.AddGold (1);
		EndTurn ();
	}

	public void EndTurn() {
		//TODO WRITE ENDTURN METHOD
		Debug.Log("Turn Ended");
	}
}

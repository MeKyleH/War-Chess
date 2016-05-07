using UnityEngine;
using System.Collections;

public class OptionsManager : MonoBehaviour {

	public GameObject buyMenu;
	public GameObject buyButton;
	public GameObject takeGoldButton;
	public GoldDisplay goldDisplay;

	public void OpenBuyMenu() {
		buyMenu.SetActive (true);
		buyButton.SetActive (false);
		takeGoldButton.SetActive (false);
	}

	public void CloseBuyMenu() {
		buyMenu.SetActive (false);
		buyButton.SetActive (true);
		takeGoldButton.SetActive (true);
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

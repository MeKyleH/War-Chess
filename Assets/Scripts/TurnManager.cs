﻿using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour {

	public GameObject buyButton;
	public GameObject buyMenu;
	public GameObject takeGoldButton;
	public GoldDisplay goldDisplay;
	public GameObject moveButton;
	public bool isWhiteTurn = true;

	private BoardManager boardManager;
	private TurnText turnText;
	private bool canPlayerClick;

	private void Start() {
		boardManager = GameObject.FindObjectOfType<BoardManager> ();
		if (!boardManager) {
			Debug.Log (name + " couldn't find boardManager");
		}
		turnText = GameObject.FindObjectOfType<TurnText> ();
		if (!turnText) {
			Debug.Log (name + " couldn't find turnText");
		}
		ToggleButtonActivation(CheckCanPlayerClick());
	}

	private void Update() {
		if(CheckCanPlayerClick() != canPlayerClick) {
			ToggleButtonActivation (CheckCanPlayerClick());
		}
	}

	private void ToggleButtonActivation(bool activation) {
		buyButton.SetActive (activation);
		takeGoldButton.SetActive (activation);
		moveButton.SetActive (activation);
		canPlayerClick = activation;
	}

	private bool CheckCanPlayerClick() {
		bool isWhiteTeam = PhotonNetwork.player.GetTeam () == PunTeams.Team.blue;
		return (isWhiteTeam && isWhiteTurn) 
			|| (!isWhiteTeam && !isWhiteTurn);
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
		isWhiteTurn = !isWhiteTurn;
		/*if (!boardManager.isBuyMode && boardManager.EnPassantMove != null) {
			boardManager.EnPassantMove = new int[2] {-1,-1};
		}*/
		turnText.UpdateDisplay (isWhiteTurn);
		boardManager.UnselectChessman ();
		MovePiece ();
	}
}

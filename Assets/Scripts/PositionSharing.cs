using UnityEngine;
using System.Collections;

public class PositionSharing : Photon.MonoBehaviour {

	//PhotonView photonView;
	NetworkManager networkManager;
	BoardManager boardManager;

	void Start() {
		//photonView = GetComponent<PhotonView> ();
		networkManager = GameObject.FindObjectOfType <NetworkManager> ();
		if (!networkManager) {
			Debug.Log (name + " couldn't find networkManager.");
		}
		boardManager = GameObject.FindObjectOfType<BoardManager> ();
		if (!boardManager) {
			Debug.Log (name + " couldn't find boardManager.");
		}
	}



	//get the string form of piece locations from the player who is ending their turn for syncing
	public string GetPieceLocationsString() {
		bool[,] pieceLocations = new bool[8,8];
		Chessman chessman;

		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				chessman = BoardManager.Instance.Chessmans [x, y];
				if (chessman == null) {
					pieceLocations [x, y] = true;
				} else {
					pieceLocations [x, y] = false;
				}
			}
		}
//		Debug.Log("11111" +ConvertBoolArrToString(pieceLocations));
		return ConvertBoolArrToString(pieceLocations);
	}

	public string GetPieceLocationsString(Chessman[,] chessmans) {
		bool[,] pieceLocations = new bool[8,8];
		Chessman chessman;

		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				chessman = chessmans [x, y];
				if (chessman == null) {
					pieceLocations [x, y] = true;
				} else {
					pieceLocations [x, y] = false;
				}
			}
		}
//		Debug.Log("22222" +ConvertBoolArrToString(pieceLocations));
		return ConvertBoolArrToString(pieceLocations);
	}

	//converts players move from bool array to a string for sharing
	public string ConvertBoolArrToString(bool[,] pieceLocations){
		string boolString = "";

		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				boolString += pieceLocations[x,y].ToString() + ",";
			}
		}
		return boolString;
	}
		
	//converts string of piece locations to a bool array for comparing with a players current piece locations
	public bool[,] ConvertStringToBoolArr(string boolString) {
		bool[,] boolArr = new bool[8,8];

		string[] longArr = boolString.Split (',');
//		Debug.Log ("LongArr[0]: " +longArr[0]);
		int count = 0;
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				boolArr[x,y] = longArr[count] == "True"? true : false;
				count++;
			}
		}
//		Debug.Log("pieceLocations[0,0]: " +pieceLocations[0,0]);
		return boolArr;
	}

	//updates a players piece locations with that of the other players'
	public void SyncPieceLocations(string othersPiecePositionsStr) {
		Chessman[,] myChessmans = BoardManager.Instance.Chessmans;
		bool[,] othersPiecePositionsBoolArr = ConvertStringToBoolArr (othersPiecePositionsStr);
		bool[,] myPiecePositionsBoolArr = ConvertStringToBoolArr(GetPieceLocationsString (myChessmans));
		bool foundMismatchedPiece = false;
		Debug.Log ("myChessmansStr " +GetPieceLocationsString (myChessmans));
		Debug.Log ("othersPiecePositionsStr " +othersPiecePositionsStr);
		Debug.Log ("(1,1) - me: " + myPiecePositionsBoolArr [1, 1] + " them: " + othersPiecePositionsBoolArr [1, 1]);

		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
//				Debug.Log ("x = " + x + " y = " + y + " myPiecePositionsBoolArr[x,y]: " + myPiecePositionsBoolArr [x, y] + " othersPiecePositionsBoolArr[x,y]: " + othersPiecePositionsBoolArr [x, y]);
//TODO PIECE POSITIONS ARE BEING SYNCED, BUT THEY ARE BEING OVERWRITTEN INCORRECTLY. NEED TO LOOK AT MYCHESSMANS AFTER THE LOOP AND ALSO AT THE LOGIC WHEN THERE IS A MISMATCH
				if (myPiecePositionsBoolArr[x,y] != othersPiecePositionsBoolArr [x, y]) {
					Debug.Log ("FOUND A MISMATCH");
					foundMismatchedPiece = true;
					myChessmans [x, y] = new Pawn ();
//					Debug.Log ("SOMETHING WENT HORRIBLY WRONG!!!!!!!!!! x: " +x+ " y: " + y);
//					Debug.Log ("myPiecePositions: " +myPiecePositions[x,y]+ " pieceLocations: " +pieceLocations[x,y]);
				}
			}
		}
		BoardManager.Instance.Chessmans = myChessmans;


		if (foundMismatchedPiece) {
//			photonView.RPC ("EndTurn_RPC", PhotonTargets.All, networkManager.isWhiteTurn, chessmans);
		} else {
			Debug.Log ("THIS WORKS PERFECTLY!!!!!!!!!!!");
		}
	}
}

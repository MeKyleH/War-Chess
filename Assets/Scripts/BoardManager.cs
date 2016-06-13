using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {
	
	public bool isBuyMode = false;
	public List<GameObject> chessmanPrefabs;
	public Material selectedMat;
	public int[] EnPassantMove{ set; get;}
	public static BoardManager Instance{ set; get; }
	public Chessman[,] Chessmans{ set; get; }

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;
	private Quaternion orientation = Quaternion.Euler(0, 0, 0);
	private bool [,] allowedMoves{ set; get; }
	private int selectionX = -1;
	private int selectionY = -1;
	private Chessman selectedChessman;
	private List<GameObject> activeChessman;
	private Material previousMat;
	private TurnManager turnManager;
	private FogManager fogManager;
	private GoldDisplay goldDisplay;
	private PhotonView photonView;
	private bool isWhitePlayer;

	private void Start() {
		Instance = this;
		isWhitePlayer = PhotonNetwork.player.GetTeam () == PunTeams.Team.blue;
		turnManager = GameObject.FindObjectOfType<TurnManager> ();
		if (!turnManager) {
			Debug.Log (name + " couldn't find TurnManager");
		}
		fogManager = GameObject.FindObjectOfType<FogManager> ();
		if (!fogManager) {
			Debug.Log (name + " couldn't find fogManager.");
		}
		goldDisplay = GameObject.FindObjectOfType<GoldDisplay> ();
		if (!goldDisplay) {
			Debug.Log (name + " couldn't find GoldDisplay");
		}
		photonView = GetComponent<PhotonView> ();
		if (!photonView) {
			Debug.Log (name + " coudln't find photonView.");
		}
		SetupBoard ();
	}

	private void Update () {
		UpdateSelection ();
		if (Input.GetMouseButtonDown (0) && ((turnManager.isWhiteTurn && isWhitePlayer) || (!turnManager.isWhiteTurn && !isWhitePlayer))) {
			ProcessClick ();
		}
	}

	private void ProcessClick ()
	{
		if (selectionX >= 0 && selectionY >= 0) {
			//BUY MODE
			if (isBuyMode) {
				ProcessPiecePurchase ();
			}
			//MOVE MODE MODE
			else {
				//select the chessman
				if (selectedChessman == null) {
					SelectChessman (selectionX, selectionY);
				}
				//move the chessman
				else {
					MoveChessman (selectionX, selectionY);
				}
			}
		}
	}

	void ProcessPiecePurchase ()
	{
		Defender defender = Button.selectedDefender;
		if (!defender) {
			return;
		}
		//WHITE TURN
		if (turnManager.isWhiteTurn && (selectionY == 0 || selectionY == 1)) {
			//prevent spawning over an existing piece
			if (Chessmans [selectionX, selectionY] != null) {
				return;
			}
			if (goldDisplay.UseGold (defender.goldCost) == GoldDisplay.Status.SUCCESS) {
				SpawnChessman (defender.spawnIndex, selectionX, selectionY);
				turnManager.EndTurn ();
			} else {
				Debug.Log ("Not enough gold to buy");
			}
		}
		//BLACK TURN
		else if (!turnManager.isWhiteTurn && (selectionY == 7 || selectionY == 6)) {
			//prevent spawning over an existing piece
			if (Chessmans [selectionX, selectionY] != null) {
				return;
			}
			if (goldDisplay.UseGold (defender.goldCost) == GoldDisplay.Status.SUCCESS) {
				SpawnChessman (defender.spawnIndex, selectionX, selectionY);
				turnManager.EndTurn ();
			} else {
				Debug.Log ("Not enough gold to buy");
			}
		}
	}

	private void SelectChessman(int x, int y) {
		if (x == -1 || y == -1) {
			return;
		}
		//check if a piece is here
		if (Chessmans [x, y] == null) {
			return;
		}
		//check that the piece being selected is your color
		if (Chessmans [x, y].isWhite != turnManager.isWhiteTurn) {
			return;
		}
		bool hasAtleastOneMove = false;
		allowedMoves = Chessmans [x, y].PossibleMove ();

		//checks if there is a possible move
		for (int i = 0; i < 8; i++) {
			if (hasAtleastOneMove) {
				break;
			}
			for (int j = 0; j < 8; j++) {
				if (allowedMoves [i, j]) {
					hasAtleastOneMove = true;
					break;
				}
			}
		}

		if (!hasAtleastOneMove) {
			return;
		}

		//Select new chessman and highlight
		selectedChessman = Chessmans [x, y];
		selectedChessman.isSelectedChessman = true;
		previousMat = selectedChessman.GetComponent<MeshRenderer> ().material;
		selectedMat.mainTexture = previousMat.mainTexture;
		selectedChessman.GetComponent<MeshRenderer> ().material = selectedMat;
		BoardHighlights.Instance.HighlightAllowedMoves (allowedMoves);
	}

	private void MoveChessman(int x, int y) {
		if (allowedMoves[x,y] == true) {
			Chessman c = Chessmans [x, y];
			if (c != null && c.isWhite != turnManager.isWhiteTurn) {
				//Captured a piece

				//If it is the king 
				if (c.GetType () == typeof(King)) {
					EndGame ();
					return;
				}
				activeChessman.Remove(c.gameObject);
				PhotonNetwork.Destroy (c.gameObject); //TODO REMOVE THIS AS IT IS LIKELY HANDLED BY THE COLLIDERS OF THE CHESSMAN
			}

			//reset enPassant
			if (x == EnPassantMove [0] && y == EnPassantMove [1]) {
				//WHITE
				if (turnManager.isWhiteTurn) {
					c = Chessmans [x, y - 1];
				}
				//BLACK
				else {
					c = Chessmans [x, y + 1];
				}
				//DESTROY ENPASSANT PIECE
				activeChessman.Remove(c.gameObject);
				PhotonNetwork.Destroy (c.gameObject);
			}
			EnPassantMove [0] = -1;
			EnPassantMove [1] = -1;

			if(selectedChessman.GetType() == typeof(Pawn)) {
				//promotion
				//WHITE TEAM
				if (y == 7) {
					activeChessman.Remove (selectedChessman.gameObject);
					PhotonNetwork.Destroy (selectedChessman.gameObject);
					selectedChessman = Chessmans [x, y];
					SpawnChessman (1, x, y);
					selectedChessman = Chessmans [x, y];
				} else if (y == 0) {
					activeChessman.Remove(selectedChessman.gameObject);
					PhotonNetwork.Destroy (selectedChessman.gameObject);
					SpawnChessman (7, x, y);
					selectedChessman = Chessmans [x, y];
				}

				//enpassant
				//WHITE TEAM
				if(selectedChessman.CurrentY == 1 && y == 3) {
					EnPassantMove [0] = x;
					EnPassantMove [1] = y-1;
				}
				//BLACK TEAM
				else if(selectedChessman.CurrentY == 6 && y == 4) {
					EnPassantMove [0] = x;
					EnPassantMove [1] = y+1;
				}
			}
			fogManager.AddSingleFog (selectedChessman.CurrentX, selectedChessman.CurrentY);

			photonView.RPC ("MoveChessman_RPC", PhotonTargets.All, GetPrefabIndex (selectedChessman),selectedChessman.CurrentX, selectedChessman.CurrentY, x, y, PhotonNetwork.player.GetTeam () == PunTeams.Team.blue);

			Chessmans [selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
			float previousZ = selectedChessman.transform.position.y;
			selectedChessman.transform.position = GetTileCenter (x, y, previousZ);
			selectedChessman.SetPosition (x, y);
			Chessmans [x, y] = selectedChessman;
			fogManager.RemoveSingleFog (x, y);
			turnManager.EndTurn();
		}

		// unselect piece at end
		selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
		selectedChessman.isSelectedChessman = false;
		selectedChessman = null;
		BoardHighlights.Instance.HideHighlights ();
	}

	//returns the prefab index of the selected chessman to be passed to MoveChessman_RPC
	private int GetPrefabIndex(Chessman chessman) {
		bool whiteOrBlack = PhotonNetwork.player.GetTeam () == PunTeams.Team.blue;
		if (selectedChessman.GetType () == typeof(Pawn)) {
			return whiteOrBlack ? 5 : 11;
		}
		else if (selectedChessman.GetType () == typeof(Knight)) {
			return whiteOrBlack ? 4 : 10;
		}
		else if (selectedChessman.GetType () == typeof(Bishop)) {
			return whiteOrBlack ? 3 : 9;
		}
		else if (selectedChessman.GetType () == typeof(Rook)) {
			return whiteOrBlack ? 2 : 8;
		}
		else if (selectedChessman.GetType () == typeof(Queen)) {
			return whiteOrBlack ? 1 : 7;
		}
		else if (selectedChessman.GetType () == typeof(King)) {
			return whiteOrBlack ? 0 : 6;
		}
		return 0;
	}

	[PunRPC]
	public void MoveChessman_RPC(int index, int previousX, int previousY, int newX, int newY, bool isWhitePlayer) {
		if (PhotonNetwork.player.GetTeam () == PunTeams.Team.blue != isWhitePlayer) {
			//remove old piece from it's position
			Chessmans [previousX, previousY] = null;

			//add new piece in the new position
			GameObject go = chessmanPrefabs [index];
			Chessmans [newX, newY] = go.GetComponent<Chessman> ();
			Chessmans [newX, newY].SetPosition (newX, newY); 
		}
	}

	public void UnselectChessman() {
		if (selectedChessman) {
			selectedChessman.GetComponent<MeshRenderer> ().material = previousMat;
			selectedChessman = null;
			BoardHighlights.Instance.HideHighlights ();
		}
	}

	private void UpdateSelection() {
		if (!Camera.main) {
			return;
		}
		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (
			   Input.mousePosition), out hit, 25.0f, LayerMask.GetMask ("ChessPlane"))) {
			selectionX = (int)hit.point.x;
			selectionY = (int)hit.point.z;
		} else {
			selectionX = -1;
			selectionY = -1;
		}
	}

	public void SpawnChessman(int index, int x, int y) {
		//prevents duplicate pieces from spawning
		if (Chessmans [x, y] != null) {
			return;
		}
		// creates new piece
		float z;
		if (index == 13) {
			z = 0.8f;
			return;
		}
		if (index == 5 || index == 11) {
			z = -0.2f;
		} else {
			z = -0.1f;
		}

		GameObject go = PhotonNetwork.Instantiate (chessmanPrefabs [index].name, GetTileCenter(x,y,z), orientation,0) as GameObject;
		go.transform.SetParent (transform);
		Chessmans [x, y] = go.GetComponent<Chessman> ();
		Chessmans [x, y].SetPosition (x, y);
		activeChessman.Add (go);
		photonView.RPC ("ProcessPiecePurchase_RPC", PhotonTargets.All, index, x, y, PhotonNetwork.player.GetTeam () == PunTeams.Team.blue);
	}

	[PunRPC]
	public void ProcessPiecePurchase_RPC(int index, int x, int y, bool isWhitePlayer) {
		if (PhotonNetwork.player.GetTeam () == PunTeams.Team.blue != isWhitePlayer) {
			GameObject go = chessmanPrefabs [index];
			Chessmans [x, y] = go.GetComponent<Chessman> ();
			Chessmans [x, y].SetPosition (x, y);
			activeChessman.Add (go);
		}
	}

	//Used to spawn a normal game of chess
	public void SpawnAllChessmans() {
		//SPAWN WHITE TEAM
		//King
		SpawnChessman (0, 3,0);

		//Queen
		SpawnChessman (1, 4,0);

		//Rooks
		SpawnChessman (2, 0,0);
		SpawnChessman (2, 7,0);

		//Bishops
		SpawnChessman (3, 2,0);
		SpawnChessman (3, 5,0);

		//Knights
		SpawnChessman (4, 1,0);
		SpawnChessman (4, 6,0);

		//Pawns
		for(int i = 0; i < 8; i++) {
			SpawnChessman (5, i,1);
		}

		//SPAWN BLACK TEAM
		//King
		SpawnChessman (6, 4,7);

		//Queen
		SpawnChessman (7, 3,7);

		//Rooks
		SpawnChessman (8, 0,7);
		SpawnChessman (8, 7,7);

		//Bishops
		SpawnChessman (9, 2,7);
		SpawnChessman (9, 5,7);

		//Knights
		SpawnChessman (10, 1,7);
		SpawnChessman (10, 6,7);

		//Pawns
		for(int i = 0; i < 8; i++) {
			SpawnChessman (11, i,6);
		}
	}

	//Used to spawn a normal game of chess
	public void SpawnBaseChessmans(bool isWhitePlayer) {
		//SPAWN WHITE TEAM
		if (isWhitePlayer) {
			//King
			SpawnChessman (0, 3, 0);

			//Queen
			SpawnChessman (1, 4, 0);

			//Rooks
			SpawnChessman (2, 0, 0);
//			SpawnChessman (2, 7, 0);

			//Bishops
			SpawnChessman (3, 2, 0);
//			SpawnChessman (3, 5, 0);

			//Knights
			SpawnChessman (4, 1, 0);
//			SpawnChessman (4, 6, 0);

			//Pawns
			for (int i = 2; i < 6; i++) {
				SpawnChessman (5, i, 1);
			}
		} else {
			//SPAWN BLACK TEAM
			//King
			SpawnChessman (6, 4, 7);

			//Queen
			SpawnChessman (7, 3, 7);

			//Rooks
//			SpawnChessman (8, 0, 7);
			SpawnChessman (8, 7, 7);

			//Bishops
//			SpawnChessman (9, 2, 7);
			SpawnChessman (9, 5, 7);

			//Knights
//			SpawnChessman (10, 1, 7);
			SpawnChessman (10, 6, 7);

			//Pawns
			for(int i = 2; i < 6; i++) {
				SpawnChessman (11, i,6);
			}
		}
	}

	void SetupBoard ()
	{
		activeChessman = new List<GameObject> ();
		Chessmans = new Chessman[8, 8];
		EnPassantMove = new int[2] {-1,-1};
	}

	private Vector3 GetTileCenter(int x, int y, float z) {
		Vector3 origin = Vector3.zero;
		origin.x += (TILE_SIZE * x) + TILE_OFFSET;
		origin.z += (TILE_SIZE * y) + TILE_OFFSET;
		origin.y += z;
		return origin;	
	}

	private void EndGame() {
		if (turnManager.isWhiteTurn) {
			Debug.Log ("White team wins");
		} else {
			Debug.Log ("Black team wins");
		}
		foreach (GameObject go in activeChessman) {
			PhotonNetwork.Destroy (go);
		}

		turnManager.EndGame ();
		BoardHighlights.Instance.HideHighlights ();
		SpawnAllChessmans ();
		goldDisplay.ResetGold ();
	}
}

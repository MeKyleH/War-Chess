using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {
	
	public bool isWhiteTurn = true;
	public bool isBuyMode = false;
	public bool isNormalGame = true;
	public bool isInitialPlacement;
	public List<GameObject> chessmanPrefabs;
	public Material selectedMat;
	public int[] EnPassantMove{ set; get;}
	public static BoardManager Instance{ set; get; }
	public Chessman[,] Chessmans{ set; get; }
//	public bool[,] fogLocation = new bool[8,8]; //TODO MAKE THIS PRIVATE

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
	private GoldDisplay goldDisplay;

	private void Start() {
		Instance = this;
		turnManager = GameObject.FindObjectOfType<TurnManager> ();
		if (!turnManager) {
			Debug.Log (name + " couldn't find TurnManager");
		}
		goldDisplay = GameObject.FindObjectOfType<GoldDisplay> ();
		if (!goldDisplay) {
			Debug.Log (name + " couldn't find GoldDisplay");
		}
		if (isNormalGame) {
			SpawnAllChessmans ();
		} else {
			isInitialPlacement = true;
		}
	}

	private void Update () {
		UpdateSelection ();
		if (Input.GetMouseButtonDown (0)) {
			ProcessClick ();
		}
	}

	private void ProcessClick ()
	{
		if (selectionX >= 0 && selectionY >= 0) {
			//BUY MODE
			if (isBuyMode) {
				Defender defender = Button.selectedDefender;
				if (!defender) {
					return;
				}
				//WHITE TURN
				if (isWhiteTurn && selectionY == 0 || selectionY == 1) {
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
				} //BLACK TURN
				else if (!isWhiteTurn && selectionY == 7 || selectionY == 6) {
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
			//INITIAL PLACEMENT OF PIECES
			else if (isInitialPlacement) {

			}
			//MOVE MODE MODE
			else {
				if (selectedChessman == null) {
					//select the chessman
					SelectChessman (selectionX, selectionY);
				}
				else {
					//move the chessman
					MoveChessman (selectionX, selectionY);
				}
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
		if (Chessmans [x, y].isWhite != isWhiteTurn) {
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
		previousMat = selectedChessman.GetComponent<MeshRenderer> ().material;
		selectedMat.mainTexture = previousMat.mainTexture;
		selectedChessman.GetComponent<MeshRenderer> ().material = selectedMat;
		BoardHighlights.Instance.HighlightAllowedMoves (allowedMoves);
	}

/*	public void UpdateFog() {
		Debug.Log ("INSIDE UPDATE FOG");
		float startTime = Time.timeSinceLevelLoad;
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				if (allowedMoves[i,j] == true) {
					SpawnChessman (12, i, j);
				}
			}
		}
		float endTime = Time.timeSinceLevelLoad;
		Debug.Log (endTime - startTime);
		Debug.Log ("FOG UPDATED");
	}
*/
	private void MoveChessman(int x, int y) {
		if (allowedMoves[x,y] == true) {
			Chessman c = Chessmans [x, y];
			if (c != null && c.isWhite != isWhiteTurn) {
				//Captured a piece

				//If it is the king 
				if (c.GetType () == typeof(King)) {
					EndGame ();
					return;
				}
				activeChessman.Remove(c.gameObject);
				Destroy (c.gameObject);
			}

			//reset enPassant
			if (x == EnPassantMove [0] && y == EnPassantMove [1]) {
				//WHITE
				if (isWhiteTurn) {
					c = Chessmans [x, y - 1];
				}
				//BLACK
				else {
					c = Chessmans [x, y + 1];
				}
				//DESTROY ENPASSANT PIECE
				activeChessman.Remove(c.gameObject);
				Destroy (c.gameObject);
			}
			EnPassantMove [0] = -1;
			EnPassantMove [1] = -1;

			if(selectedChessman.GetType() == typeof(Pawn)) {
				//promotion
				//WHITE TEAM
				if (y == 7) {
					activeChessman.Remove (selectedChessman.gameObject);
					Destroy (selectedChessman.gameObject);
					selectedChessman = Chessmans [x, y];
					SpawnChessman (1, x, y);
					selectedChessman = Chessmans [x, y];
				} else if (y == 0) {
					activeChessman.Remove(selectedChessman.gameObject);
					Destroy (selectedChessman.gameObject);
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
				
			Chessmans [selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
			float previousZ = selectedChessman.transform.position.y;
			selectedChessman.transform.position = GetTileCenter (x, y, previousZ);
			selectedChessman.SetPosition (x, y);
			Chessmans [x, y] = selectedChessman;
			isWhiteTurn = !isWhiteTurn;
		}

		// unselect piece at end
		selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
		selectedChessman = null;
		BoardHighlights.Instance.HideHighlights ();
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

	private void SpawnChessman(int index, int x, int y) {
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
		GameObject go = Instantiate (chessmanPrefabs [index], GetTileCenter(x,y,z), orientation) as GameObject;
		go.transform.SetParent (transform);
		Chessmans [x, y] = go.GetComponent<Chessman> ();
		Chessmans [x, y].SetPosition (x, y);
		activeChessman.Add (go);
	}

	//Used to spawn initial pieces at player choices
	private void SpawnIniticalSpecificChessman() {
		activeChessman = new List<GameObject>();
		Chessmans = new Chessman[8,8];
		EnPassantMove = new int[2]{ -1, -1 };

		//SPAWN WHITE TEAM
		//King
		SpawnChessman (0, 3,0);

		//Queen
		SpawnChessman (1, 4,0);

		//Pawns
		for(int i = 2; i < 6; i++) {
			SpawnChessman (5, i,1);
		}

		//SPAWN BLACK TEAM
		//King
		SpawnChessman (6, 4,7);

		//Queen
		SpawnChessman (7, 3,7);

		//Pawns
		for(int i = 2; i < 6; i++) {
			SpawnChessman (11, i,6);
		}
	}

	//Used to spawn a normal game of chess
	private void SpawnAllChessmans() {
		activeChessman = new List<GameObject>();
		Chessmans = new Chessman[8,8];
		EnPassantMove = new int[2]{ -1, -1 };

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

	private Vector3 GetTileCenter(int x, int y, float z) {
		Vector3 origin = Vector3.zero;
		origin.x += (TILE_SIZE * x) + TILE_OFFSET;
		origin.z += (TILE_SIZE * y) + TILE_OFFSET;
		origin.y += z;
		return origin;	
	}

	private void EndGame() {
		if (isWhiteTurn) {
			Debug.Log ("White team wins");
		} else {
			Debug.Log ("Black team wins");
		}
		foreach (GameObject go in activeChessman) {
			Destroy (go);
		}

		isWhiteTurn = true;
		BoardHighlights.Instance.HideHighlights ();
		SpawnAllChessmans ();
		goldDisplay.ResetGold ();
	}
}

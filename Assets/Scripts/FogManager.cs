using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogManager : MonoBehaviour {

	private BoardManager boardManager;
	private TurnManager turnManager;
	private GameObject[,] Fogs { set; get; }

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;
	private const float FOG_Z_OFFSET = 0.68f;
	private Quaternion orientation = Quaternion.Euler(0, 0, 0);
	private bool isWhitePlayer;

	private void Start() {
		isWhitePlayer = PhotonNetwork.player.GetTeam () == PunTeams.Team.blue;
		Fogs = new GameObject[8, 8];
		boardManager = GameObject.FindObjectOfType<BoardManager> ();
		if (!boardManager) {
			Debug.Log (name + " couldn't find boardManager.");
		}
		turnManager = GameObject.FindObjectOfType<TurnManager> (); 
		if (!turnManager) {
			Debug.Log (name + " couldn't find turnManager.");
		}
		AddBaseFog ();
	}

	public void AddBaseFog ()
	{
		for (int y = 0; y < 8; y++) {
			if ((isWhitePlayer && (y == 0 || y == 1)) || (!isWhitePlayer && (y == 6 || y == 7))) {
				continue;
			}
			for (int x = 0; x < 8; x++) {
				if (Fogs [x, y] != null) {
					continue;
				}
				GameObject go = Instantiate (boardManager.chessmanPrefabs [12], GetTileCenter (x, y), orientation) as GameObject;			
				Fogs [x, y] = go;
			}
		}
	}	

	public void AddSingleFog (int x, int y) {
		if ((isWhitePlayer && (y == 0 || y == 1)) || (!isWhitePlayer && (y == 6 || y == 7))) {
			return;
		}
		GameObject go = Instantiate (boardManager.chessmanPrefabs [12], GetTileCenter (x, y), orientation) as GameObject;			
		Fogs [x, y] = go;
	}

	public void UpdatePieceMoves(Chessman[,] Chessmans) {
		AddBaseFog ();
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				if (Chessmans [x, y] != null && ((isWhitePlayer && Chessmans[x,y].isWhite) || (!isWhitePlayer && !Chessmans[x,y].isWhite))) {
					RemoveAllowedMovesFog (Chessmans [x, y].PossibleMove ());
					RemoveSingleFog (x, y);
				}
			}
		}
	}

	private void RemoveAllowedMovesFog(bool [,] allowedMoves) {
		for (int y = 0; y < 8; y++) {
			if ((isWhitePlayer && (y == 0 || y == 1)) || (!isWhitePlayer && (y == 6 || y == 7))) {
				continue;
			}
			for (int x = 0; x < 8; x++) {
				if (allowedMoves [x, y]) {
					RemoveSingleFog (x, y);
				}
			}
		}
	}

	public void RemoveSingleFog(int x, int y) {
		GameObject selectedFog = Fogs [x, y];
		Fogs [x, y] = null;
		Destroy (selectedFog);
	}

	private Vector3 GetTileCenter(int x, int y) {
		Vector3 origin = Vector3.zero;
		origin.x += (TILE_SIZE * x) + TILE_OFFSET;
		origin.z += (TILE_SIZE * y) + TILE_OFFSET;
		origin.y += FOG_Z_OFFSET;
		return origin;	
	}
}

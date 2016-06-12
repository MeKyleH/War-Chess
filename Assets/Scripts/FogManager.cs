using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogManager : MonoBehaviour {

	private BoardManager boardManager;
	private List<GameObject> fogs;

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;
	private const float FOG_Z_OFFSET = 0.6f;
	private Quaternion orientation = Quaternion.Euler(0, 0, 0);
	private bool isWhitePlayer;

	private void Start() {
		isWhitePlayer = PhotonNetwork.player.GetTeam () == PunTeams.Team.blue;

		boardManager = GameObject.FindObjectOfType<BoardManager> ();
		if (!boardManager) {
			Debug.Log (name + " couldn't find boardManager.");
		}
		fogs = new List<GameObject> ();
		bool[,] nonMoves = new bool[8,8];
		for (int y = 0; y < 8; y++) {
			if ((isWhitePlayer && (y == 0 || y == 1)) || (!isWhitePlayer && (y == 6 || y == 7))) {
				continue;
			}
			for (int x = 0; x < 8; x++) {
				nonMoves [x, y] = true;
			}
		}
		AddBoardFog (nonMoves);
	}

	public void AddBoardFog(bool[,] nonMoves) {
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				if (nonMoves [x, y]) {
					GameObject go = Instantiate (boardManager.chessmanPrefabs [12], GetTileCenter(x,y), orientation) as GameObject;
				}
			}
		}
	}

	private Vector3 GetTileCenter(int x, int y) {
		Vector3 origin = Vector3.zero;
		origin.x += (TILE_SIZE * x) + TILE_OFFSET;
		origin.z += (TILE_SIZE * y) + TILE_OFFSET;
		origin.y += FOG_Z_OFFSET;
		return origin;	
	}

}

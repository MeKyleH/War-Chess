using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogManager : MonoBehaviour {

	private BoardManager boardManager;
	private Fog[,] Fogs { set; get; }

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;
	private const float FOG_Z_OFFSET = 0.6f;
	private Quaternion orientation = Quaternion.Euler(0, 0, 0);
	private bool isWhitePlayer;

	private void Start() {
		isWhitePlayer = PhotonNetwork.player.GetTeam () == PunTeams.Team.blue;
		Fogs = new Fog[8, 8];
		boardManager = GameObject.FindObjectOfType<BoardManager> ();
		if (!boardManager) {
			Debug.Log (name + " couldn't find boardManager.");
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
				GameObject go = Instantiate (boardManager.chessmanPrefabs [12], GetTileCenter (x, y), orientation) as GameObject;			
				Fogs [x, y] = go.GetComponent<Fog> ();
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

	public void RemoveSingleFog(int x, int y) {

	}
}

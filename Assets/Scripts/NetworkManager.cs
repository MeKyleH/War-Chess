using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviour {

	public Text connectionText;
	public Text teamColorText;
	public Transform[] spawnPoints;
	public Camera lobbyCamera;
	public GameObject lobbyPanel;
	public InputField username;
	public InputField roomName;
	public GameObject inGameElements;
	public GameObject outOfGameElements;
	public GameObject turnManagerObj;
	public GameObject board;

	GameObject player;
	PhotonView photonView;

	private const string VERSION = "0.3";
	public bool isWhiteTurn;
	private bool joinedRoom = false;
	private TurnManager turnManager;
	private BoardManager boardManager;
	private TurnText turnText;
	private PositionSharing positionSharing;

	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
		PhotonNetwork.ConnectUsingSettings (VERSION);
		PhotonNetwork.autoCleanUpPlayerObjects = false;

		photonView = GetComponent<PhotonView> ();
		StartCoroutine ("UpdateConnectionString");
	}

	// displays the connection status string at the bottom to the player
	IEnumerator UpdateConnectionString() {
		while (true) {
			connectionText.text = PhotonNetwork.connectionStateDetailed.ToString ();
			yield return null;
		}
	}


	//activates the window for joining games
	void OnJoinedLobby() {
		lobbyPanel.SetActive (true);
	}

	//called by the button when a player has entered their username and room
	public void JoinRoom() {
		PhotonNetwork.player.name = username.text;
		RoomOptions ro = new RoomOptions () {isVisible = true, maxPlayers = 2};
		PhotonNetwork.JoinOrCreateRoom (roomName.text, ro, TypedLobby.Default);
	}


	//On initial room Join spawn the player immediately
	void OnJoinedRoom() {
		SetupRoom ();
		//set player teams
		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.player.SetTeam (PunTeams.Team.blue);
		} else {
			PhotonNetwork.player.SetTeam (PunTeams.Team.red);
		}
		StartSpawnProcess (0f);

		teamColorText.text = PhotonNetwork.player.GetTeam() == PunTeams.Team.blue? "White Team" : "Black Team";
	}

	//transitions from out of game to in game
	void SetupRoom() {
		StopCoroutine ("UpdateConnectionString");
		outOfGameElements.SetActive (false);
		inGameElements.SetActive (true);
		turnManagerObj.SetActive (true);
		board.SetActive (true);
		turnManager = GameObject.FindObjectOfType<TurnManager> ();
		if (!turnManager) {
			Debug.Log (name + " couldn't find turnManager.");
		}
		turnText = GameObject.FindObjectOfType<TurnText> ();
		if (!turnText) {
			Debug.Log (name + " couldn't find turnText.");
		}
		positionSharing = GameObject.FindObjectOfType<PositionSharing> ();
		if (!positionSharing) {
			Debug.Log (name + " coudln't find positionSharing.");
		}
		isWhiteTurn = turnManager.isWhiteTurn;
		joinedRoom = true;
	}

	void OnPhotonPlayerConnected(PhotonPlayer player) {
		Debug.Log ("OnPhotonPlayerConnected: " + player.name);

		//Update whose turn it is when a new player joins
		if (PhotonNetwork.isMasterClient) {
			string finalPieceLocations = positionSharing.GetPieceLocationsString ();
			photonView.RPC ("EndTurn_RPC", PhotonTargets.All, isWhiteTurn, finalPieceLocations);
		}
	}

	//sets the lobby camera active and prepares to spawn the player
	void StartSpawnProcess(float respawnTime) {
		lobbyCamera.enabled = true;
		StartCoroutine ("SpawnPlayer", respawnTime);
	}

	//spawns the player and disables the lobby camera
	IEnumerator SpawnPlayer(float respawnTime) {
		yield return new WaitForSeconds(respawnTime);
		bool isWhiteTeam = PhotonNetwork.player.GetTeam() == PunTeams.Team.blue;
	
		int index = isWhiteTeam? 0: 1;
		player = PhotonNetwork.Instantiate ("Player Camera",
			spawnPoints [index].position,
			spawnPoints [index].rotation,
			0);
		lobbyCamera.enabled = false;
	}

	void Update() {
		if (!joinedRoom) {
			return;
		}
		if (turnManager.isWhiteTurn != isWhiteTurn) {
			string finalPieceLocations = positionSharing.GetPieceLocationsString ();
			photonView.RPC ("EndTurn_RPC", PhotonTargets.All, !isWhiteTurn, finalPieceLocations);
		}
	}

	//used for syncing player turns
	[PunRPC]
	public void EndTurn_RPC(bool isWhiteTurn, string finalPieceLocations) {
		positionSharing.SyncPieceLocations (finalPieceLocations);
		this.isWhiteTurn = isWhiteTurn;
		turnManager.isWhiteTurn = isWhiteTurn;
		turnText.UpdateDisplay (isWhiteTurn);
	}

	//TODO CREATE METHOD THAT CONVERTS STRING TO JSON AND JSON TO STRING TRANSFER
	//THE STRING VIA RPC THAT CONTAINS ALL OF THE CHESSMAN MOVES AND THEN SHARES IT 
	//WITH ALL PLAYERS
}

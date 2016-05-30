using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviour {

	public Text connectionText;
	public Text teamColorText;
	public Transform[] spawnPoints;
	public Camera sceneCamera;
	public GameObject serverWindow; //TODO RENAME TO LOBBYWINDOW
	public InputField username;
	public InputField roomName;
	public GameObject inGameElements;
	public GameObject outOfGameElements;
	public GameObject turnManager;

	GameObject player;
	PhotonView photonView;

	private const string VERSION = "0.2";

	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
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
		serverWindow.SetActive (true);
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
		outOfGameElements.SetActive (false);
		inGameElements.SetActive (true);
		turnManager.SetActive (true);
		StopCoroutine ("UpdateConnectionString");
	}

	//sets the lobby camera active and prepares to spawn the player
	void StartSpawnProcess(float respawnTime) {
		sceneCamera.enabled = true;
		StartCoroutine ("SpawnPlayer", respawnTime);
	}

	//spawns the player and disables the lobby camera
	IEnumerator SpawnPlayer(float respawnTime) {
		yield return new WaitForSeconds(respawnTime);
		bool isWhiteTeam = PhotonNetwork.player.GetTeam() == PunTeams.Team.blue;
	
		string playerCamera = isWhiteTeam ? "White Player Camera" : "Black Player Camera";
		int index = isWhiteTeam? 0: 1;
		player = PhotonNetwork.Instantiate (playerCamera,
			spawnPoints [index].position,
			spawnPoints [index].rotation,
			0);
		sceneCamera.enabled = false;
	}

	//used for syncing player turns
//	[PunRPC]
//	void EndTurn_RPC(bool isWhiteTurn) {
//		this.isWhiteTurn = isWhiteTurn;
//		turnText.text = this.isWhiteTurn ? "White Turn" : "Black Turn";
//	}

	void MovePiece_RPC(){

	}

}

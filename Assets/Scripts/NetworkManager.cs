using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviour {

	[SerializeField] Text connectionText;
	[SerializeField] Text teamColorText;
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] Camera sceneCamera;
	[SerializeField] GameObject serverWindow; //TODO RENAME TO LOBBYWINDOW
	[SerializeField] InputField username;
	[SerializeField] InputField roomName;
	[SerializeField] Text turnText;

	GameObject player;
	PhotonView photonView;

	private const string VERSION = "0.2";
	private ExitGames.Client.Photon.Hashtable customProps;
	private BoardManager boardManager;
	public bool isWhiteTurn; //TODO MAKE PRIVATE

	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings (VERSION);
		PhotonNetwork.autoCleanUpPlayerObjects = false;

		photonView = GetComponent<PhotonView> ();
		StartCoroutine ("UpdateConnectionString");

		boardManager = GameObject.FindObjectOfType<BoardManager> ();
		isWhiteTurn = boardManager.isWhiteTurn;
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
		customProps = new ExitGames.Client.Photon.Hashtable ();
	}


	//On initial room Join spawn the player immediately
	void OnJoinedRoom() {
		serverWindow.SetActive (false);
		StopCoroutine ("UpdateConnectionString");
		connectionText.text = "";
		turnText.text = this.isWhiteTurn ? "White Turn" : "Black Turn";

		//set player teams
		if (PhotonNetwork.isMasterClient) {
			customProps.Add ("isWhiteTeam", true);
		} else {
			customProps.Add ("isWhiteTeam", false);
		}
		PhotonNetwork.player.SetCustomProperties(customProps);
		StartSpawnProcess (0f);

		string teamColor = (bool)PhotonNetwork.player.customProperties ["isWhiteTeam"] == true? "WHITE" : "BLACK";
		teamColorText.text = "You are on the " +teamColor+ " Team.";

		if (boardManager.Chessmans[0,0] == null) {
			Debug.Log ("FOUND CHESSMANS!!!!!!!!!!!!!!!");
		}
	}

	//sets the lobby camera active and prepares to spawn the player
	void StartSpawnProcess(float respawnTime) {
		sceneCamera.enabled = true;
		StartCoroutine ("SpawnPlayer", respawnTime);
	}

	//spawns the player and disables the lobby camera
	IEnumerator SpawnPlayer(float respawnTime) {
		yield return new WaitForSeconds(respawnTime);
		bool isWhiteTeam = (bool)PhotonNetwork.player.customProperties ["isWhiteTeam"] == true;
	
		string playerCamera = isWhiteTeam ? "White Player Camera" : "Black Player Camera";
		int index = isWhiteTeam? 0: 1;
		player = PhotonNetwork.Instantiate (playerCamera,
			spawnPoints [index].position,
			spawnPoints [index].rotation,
			0);
		sceneCamera.enabled = false;
	}

	void Update() {
		if (boardManager.isWhiteTurn != isWhiteTurn) {
			photonView.RPC ("EndTurn_RPC", PhotonTargets.All, !isWhiteTurn);
		}
	}

	//used for syncing player turns
	[PunRPC]
	void EndTurn_RPC(bool isWhiteTurn) {
		this.isWhiteTurn = isWhiteTurn;
		boardManager.isWhiteTurn = isWhiteTurn;
		turnText.text = this.isWhiteTurn ? "White Turn" : "Black Turn";
	}

	void MovePiece_RPC(){

	}

}

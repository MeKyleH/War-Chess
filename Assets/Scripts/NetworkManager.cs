using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviour {

	[SerializeField] Text connectionText;
	[SerializeField] Text teamColorText;
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] Camera sceneCamera;
	[SerializeField] string roomName = "Kyle";

	GameObject player;

	private const string VERSION = "0.2";
	private ExitGames.Client.Photon.Hashtable customProps;


	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings (VERSION);
		PhotonNetwork.autoCleanUpPlayerObjects = false;
	}

	void Update() {
		connectionText.text = PhotonNetwork.connectionStateDetailed.ToString ();
	}

	void OnJoinedLobby() {
		RoomOptions ro = new RoomOptions () {isVisible = true, maxPlayers = 2};
		PhotonNetwork.JoinOrCreateRoom (roomName, ro, TypedLobby.Default);
		customProps = new ExitGames.Client.Photon.Hashtable ();
	}

	//On initial room Join spawn the player immediately
	void OnJoinedRoom() {
		if (PhotonNetwork.isMasterClient) {
			customProps.Add ("isWhiteTeam", true);
		} else {
			customProps.Add ("isWhiteTeam", false);
		}
		PhotonNetwork.player.SetCustomProperties(customProps);
		StartSpawnProcess (0f);

		string teamColor = (bool)PhotonNetwork.player.customProperties ["isWhiteTeam"] == true? "WHITE" : "BLACK";
		teamColorText.text = "You are on the " +teamColor+ " Team.";
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

}

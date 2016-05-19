using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	[SerializeField] Text connectionText;
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] Camera sceneCamera;
	[SerializeField] string roomName = "Kyle";

	GameObject player;

	private const string VERSION = "0.1";

	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings (VERSION);
	}

	void Update() {
		connectionText.text = PhotonNetwork.connectionStateDetailed.ToString ();
	}

	void OnJoinedLobby() {
		RoomOptions ro = new RoomOptions () {isVisible = true, maxPlayers = 2};
		PhotonNetwork.JoinOrCreateRoom (roomName, ro, TypedLobby.Default);
	}

	//On initial room Join spawn the player immediately
	void OnJoinedRoom() {
		StartSpawnProcess (0f);
	}

	//sets the lobby camera active and prepares to spawn the player
	void StartSpawnProcess(float respawnTime) {
		sceneCamera.enabled = true;
		StartCoroutine ("SpawnPlayer", respawnTime);
	}

	//spawns the player and disables the lobby camera
	IEnumerator SpawnPlayer(float respawnTime) {
		yield return new WaitForSeconds(respawnTime);

		int index = Random.Range (0, spawnPoints.Length);
		player = PhotonNetwork.Instantiate ("Player Camera",
			spawnPoints [index].position,
			spawnPoints [index].rotation,
			0);
		sceneCamera.enabled = false;
	}

}

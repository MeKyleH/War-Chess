using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public string roomName = "Room47";
	public string playerPrefabName = "Player Camera";
	public Transform spawnPoint;

	private const string VERSION = "1.0";
	private RoomOptions roomOptions;

	void Start () {
		PhotonNetwork.ConnectUsingSettings (VERSION);
	}

	void OnJoinedLobby() {
		roomOptions = new RoomOptions () { isVisible = false, maxPlayers = 2 };
		PhotonNetwork.JoinOrCreateRoom (roomName, roomOptions, TypedLobby.Default);
	}

	void OnJoinedRoom() {
		PhotonNetwork.Instantiate (playerPrefabName, spawnPoint.position, spawnPoint.rotation, 0);			
	}

	public void OnConnectedToMaster()
	{
		PhotonNetwork.CreateRoom(roomName,roomOptions, TypedLobby.Default);
	}

}

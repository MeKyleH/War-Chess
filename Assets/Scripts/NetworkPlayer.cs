using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {

	Vector3 position;
	Quaternion rotation;
	BoardManager boardManager;

	void Start () {
		boardManager = GameObject.FindObjectOfType<BoardManager> ();

		if (photonView.isMine) {
			//ENABLE ANY SETTINGS THAT HAVE BEEN DISABLED FROM THE GENERIC PLAYER PREFAB (DON'T FORGET GRAVITY FOR RIGIDBODIES)
			GetComponent<Camera>().enabled = true;
		} else {
			StartCoroutine ("UpdateData");
		}
	}

	// Manages the smoothing of the data from other players (photon views)
	IEnumerator UpdateData() {
		while(true) {
			//transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothing);
			//transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
			yield return null;
		}
	}

	// Sends and receives information from other players (photon views)
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
//			stream.SendNext(boardManager.isWhiteTurn);
//			Debug.Log ("Sent Message: " + boardManager.isWhiteTurn);
		} else {
//			boardManager.isWhiteTurn = (bool)stream.ReceiveNext();
//			Debug.Log ("Received Message: " + boardManager.isWhiteTurn);
		}
	}

}

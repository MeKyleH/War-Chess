using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {

	TurnManager turnManager;

	void Start () {
		turnManager = GameObject.FindObjectOfType<TurnManager> ();
		if (!turnManager) {
			Debug.Log (name + " couldn't find turnManager.");
		}
		if (photonView.isMine) {
			//ENABLE ANY SETTINGS THAT HAVE BEEN DISABLED FROM THE GENERIC PLAYER PREFAB (DON'T FORGET GRAVITY FOR RIGIDBODIES)
			GetComponent<Camera>().enabled = true;
		} 
	}


	// Sends and receives information from other players (photon views)
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			Debug.Log ("Sent Message: " + turnManager.isWhiteTurn);
		} else {
			Debug.Log ("Received Message: " + turnManager.isWhiteTurn);
		}
	}

}

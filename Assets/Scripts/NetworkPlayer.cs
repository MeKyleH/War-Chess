using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {

	Vector3 position;
	Quaternion rotation;
	float smoothing = 10f; //higher is slower and more realistic


	void Start () {
		if (photonView.isMine) {
			//ENABLE ANY SETTINGS THAT HAVE BEEN DISABLED FROM THE GENERIC PLAYER PREFAB (DON'T FORGET GRAVITY FOR RIGIDBODIES)
			GetComponent<Camera>().enabled = true;
		} else {
			//StartCoroutine ("UpdateData");
		}
	}

	// Manages the smoothing of the data from other players (photon views)
	IEnumerator UpdateData() {
		while(true) {
			transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothing);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
			yield return null;
		}
	}

	// Sends and receives information from other players (photon views)
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
		} else {
			position = (Vector3)stream.ReceiveNext ();
			rotation = (Quaternion)stream.ReceiveNext ();
		}
	}
}

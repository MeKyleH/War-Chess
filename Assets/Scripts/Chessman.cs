using UnityEngine;
using System.Collections;

public abstract class Chessman : Photon.MonoBehaviour {

	public int CurrentX{ set; get; }
	public int CurrentY{ set; get; }
	public bool isWhite;
	public bool isSelectedChessman = false;

	public void SetPosition(int x, int y) {
		CurrentX = x;
		CurrentY = y;
	}

	public virtual bool[,] PossibleMove() {
		return new bool[8,8];
	}

	void OnTriggerEnter(Collider collider) {
		if (isSelectedChessman) {
			collider.GetComponent<PhotonView> ().RPC ("DestroyChessman_RPC", PhotonTargets.All);
		}
	}

	[PunRPC]
	public void DestroyChessman_RPC() {
		if (photonView.isMine) {
			PhotonNetwork.Destroy (gameObject);
		}
	}
}

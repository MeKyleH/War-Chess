using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
public class GoldDisplay : MonoBehaviour {
	public enum Status {SUCCESS, FAILURE};

	private Text goldText;
	private int gold = 15;
	private TurnManager turnManager;

	// Use this for initialization
	void Start () {
		turnManager = GameObject.FindObjectOfType<TurnManager> ();
		if (!turnManager) {
			Debug.Log (name + " couldn't find turnManager");
		}
		goldText = GetComponent<Text> ();
		UpdateDisplay ();
	}

	public static T GetCustomProperty<T>(PhotonView view, string property, T offlineValue, T defaultValue) {
		//checks if the player is offline
		if (PhotonNetwork.offlineMode == true) {
			return offlineValue;
		}
		//in online mode, get the custom property from the custom properties
		else {
			//ensures the value is valid and return it otherwise return the default
			if (view != null && view.owner != null && view.owner.customProperties.ContainsKey (property) == true) {
				return(T)view.owner.customProperties [property];
			}
			return defaultValue;
		}
	}

	public void AddGold(int amount) {
		gold += amount;
		UpdateDisplay ();
	}

	public Status UseGold(int amount) {
		if (gold >= amount) {
			gold -= amount;
			UpdateDisplay ();
			return Status.SUCCESS;
		}
		return Status.FAILURE;
	}

	public void ResetGold() {
		gold = 15;
		UpdateDisplay ();
	}

	private void UpdateDisplay(){
		goldText.text = gold.ToString ();
	}
}

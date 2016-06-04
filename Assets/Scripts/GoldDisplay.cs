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
		UpdateGoldDisplay ();
	}

	public void AddGold(int amount) {
		gold += amount;
		UpdateGoldDisplay ();
	}

	public Status UseGold(int amount) {
		if (gold >= amount) {
			gold -= amount;
			UpdateGoldDisplay ();
			return Status.SUCCESS;
		}
		return Status.FAILURE;
	}

	public void ResetGold() {
		gold = 15;
		UpdateGoldDisplay ();
	}

	private void UpdateGoldDisplay(){
		goldText.text = gold.ToString ();
	}
}

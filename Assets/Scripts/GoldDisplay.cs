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

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
public class GoldDisplay : MonoBehaviour {

	private Text goldText;
	private int gold = 15;
	public enum Status {SUCCESS, FAILURE};

	void Start () {
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

	private void UpdateDisplay(){
		goldText.text = gold.ToString ();
	}
}

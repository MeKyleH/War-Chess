using UnityEngine;
using System.Collections;

public class Defender : MonoBehaviour {

	public int starCost = 1;

	private GoldDisplay starDisplay;

	void Start() {
		starDisplay = GameObject.FindObjectOfType<GoldDisplay> ();
	}

	public void AddStars(int amount) {
		starDisplay.AddGold (amount);
	}
}

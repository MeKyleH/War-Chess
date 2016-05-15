using UnityEngine;
using System.Collections;

public class Defender : MonoBehaviour {

	public int goldCost = 1;
	public int spawnIndex;

	private GoldDisplay goldDisplay;

	void Start() {
		goldDisplay = GameObject.FindObjectOfType<GoldDisplay> ();
	}

	public void AddGold(int amount) {
		goldDisplay.AddGold (amount);
	}
}

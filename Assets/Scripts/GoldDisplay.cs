using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
public class GoldDisplay : MonoBehaviour {
	public enum Status {SUCCESS, FAILURE};
	public AudioClip takeGoldSFX;
	public AudioClip useGoldSFX;

	private Text goldText;
	private int gold = 15;
	private TurnManager turnManager;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		turnManager = GameObject.FindObjectOfType<TurnManager> ();
		if (!turnManager) {
			Debug.Log (name + " couldn't find turnManager");
		}
		audioSource = GetComponent<AudioSource> ();
		goldText = GetComponent<Text> ();
		UpdateGoldDisplay ();
	}

	public void AddGold(int amount) {
		gold += amount;
		audioSource.clip = takeGoldSFX;
		audioSource.Play ();
		UpdateGoldDisplay ();
	}

	public Status UseGold(int amount) {
		if (gold >= amount) {
			gold -= amount;
			audioSource.clip = useGoldSFX;
			audioSource.Play ();
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

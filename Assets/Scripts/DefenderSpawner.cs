using UnityEngine;
using System.Collections;

public class DefenderSpawner : MonoBehaviour {

	public Camera myCamera;

	private GameObject parent;
	private GoldDisplay goldDisplay;

	void Start() {
		parent = GameObject.Find ("Defenders");
		goldDisplay = GameObject.FindObjectOfType<GoldDisplay> ();

		if (!parent) {
			parent = new GameObject ("Defenders");
		}
	}

	void OnMouseDown() {
		Vector2 rawPos = CalculateWorldPointOfMouseClick ();
		Debug.Log ("rawPos: " + rawPos);
		Vector2 roundedPos = SnapToGrid (rawPos);
		GameObject defender = Button.selectedDefender;

		int defenderCost = defender.GetComponent<Defender>().starCost;
		if (goldDisplay.UseGold (defenderCost) == GoldDisplay.Status.SUCCESS) {
			SpawnDefender (roundedPos, defender);
		} else {
			Debug.Log ("Insufficient gold to spawn");
		}
	}

	void SpawnDefender (Vector2 roundedPos, GameObject defender)
	{
		Quaternion zeroRot = Quaternion.identity;
		GameObject newDef = Instantiate (defender, roundedPos, zeroRot) as GameObject;
		newDef.transform.parent = parent.transform;
	}

	Vector2 SnapToGrid(Vector2 rawWorldPos){
		float newX = Mathf.RoundToInt (rawWorldPos.x);
		float newY = Mathf.RoundToInt (rawWorldPos.y);

		return new Vector2 (newX, newY);
	}
		
	Vector2 CalculateWorldPointOfMouseClick() {
		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;
		float distanceFromCamera = 10f;

		Vector3 weirdTriplet = new Vector3 (mouseX, mouseY, distanceFromCamera);
		Vector2 worldPos = myCamera.ScreenToWorldPoint (weirdTriplet);

		return worldPos;
	}
}

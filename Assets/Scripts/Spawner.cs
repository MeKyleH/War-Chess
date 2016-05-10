using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject[] attackerPrefabArray;
	public float multiplyer;

	private bool warmedUp = false;

	void Update () {
		if (Time.timeSinceLevelLoad >= 5) {
			warmedUp = true;
		}

		foreach (GameObject thisAttacker in attackerPrefabArray) {
			if(IsTimeToSpawn(thisAttacker) && warmedUp) {
				Spawn(thisAttacker);
			}
		}
	}
		
	bool IsTimeToSpawn(GameObject attackerGameObject) {
		Attacker attacker = attackerGameObject.GetComponent<Attacker> ();
		float meanSpawnDelay = attacker.seenEverySeconds;
		float spawnsPerSecond = multiplyer / meanSpawnDelay;

		//can't spawn faster than frame rate
		if (Time.deltaTime > meanSpawnDelay) {
			Debug.LogWarning ("Spawn rate capped by frame rate");
		}

		//normalizes spawn time
		float threshold = spawnsPerSecond * Time.deltaTime / 5;
		return Random.value < threshold;
	}

	public void Spawn(GameObject myGameObject) {
		GameObject myAttacker = Instantiate (myGameObject) as GameObject;
		myAttacker.transform.parent = transform;
		myAttacker.transform.position = transform.position;
	}
}

using UnityEngine;
using System.Collections;

public class TimedSpawn : MonoBehaviour {

	public GameObject attacker;
	public float[] fixedSpawnTimes;

	private Spawner spawner;
	private bool[] hasSpawned;

	void Start () {
		hasSpawned = new bool[fixedSpawnTimes.Length];
		spawner = GetComponent<Spawner> ();
	}
	
	void Update () {
		for (int i = 0; i < fixedSpawnTimes.Length; i++) {
			if (hasSpawned[i]) {
				continue;
			}
			if (Time.timeSinceLevelLoad >= fixedSpawnTimes[i]) {
				spawner.Spawn (attacker);
				hasSpawned [i] = true;
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

	public GameObject projectile, gun;
	private GameObject projectileParent;
	private Animator animator;
	private Spawner myLaneSpawner;

	void Start() {
		animator = GameObject.FindObjectOfType<Animator>();

		//Creates parent if necessary
		projectileParent = GameObject.Find("Projectiles");
		if (!projectileParent) {
			projectileParent = new GameObject("Projectiles");
		}

		//set mylaneSpawner
		SetMyLaneSpawner ();
	}

	void Update() {
		if (IsAttackerAheadInLane ()) {
			animator.SetBool ("isAttacking", true);
		} else {
			animator.SetBool ("isAttacking", false);
		}
	}

	private void Fire() {
		GameObject newProjectile = Instantiate (projectile) as GameObject;
		newProjectile.transform.parent = projectileParent.transform;
		newProjectile.transform.position = gun.transform.position;
	}

	void SetMyLaneSpawner() {
		Spawner[] spawnerArray = GameObject.FindObjectsOfType<Spawner> ();

		foreach (Spawner spawner in spawnerArray) {
			if (spawner.transform.position.y == transform.position.y) {
				myLaneSpawner = spawner;
				return;
			}
			Debug.LogError (name + " can't find spawner in lane");
		}
	}

	bool IsAttackerAheadInLane() {
		//exit if no attackers in lane
		if (myLaneSpawner.transform.childCount <= 0) {
			return false;
		}
			
		//are the attackers ahead
		foreach (Transform attacker in myLaneSpawner.transform) {
			if (attacker.transform.position.x > transform.position.x) {
				return true;
			}
		}

		//attackers behind us
		return false;
	}
}

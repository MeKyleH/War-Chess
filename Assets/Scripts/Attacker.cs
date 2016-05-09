using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class Attacker : MonoBehaviour {

	[Tooltip ("Average number of seconds between appearances")]
	public float seenEverySeconds;

	private float currentSpeed;
	private GameObject currentTarget;
	private Animator animator;

	void Start() {
		animator = GetComponent<Animator> ();
	}

	void Update () {
		transform.Translate (Vector3.left * currentSpeed * Time.deltaTime); 
		if (!currentTarget) {
			animator.SetBool ("isAttacking", false);
		}
	}

	void OnTriggerEnter2D() {
		
	}

	public void SetSpeed(float speed) {
		currentSpeed = speed;
	}

	//called from animator at time of actual attacking.
	public void StrikeCurrentTarget(float damage) {
		if (currentTarget) {
			Health health = currentTarget.GetComponent<Health> ();
			if (health) {
				health.DealDamage (damage);
			}
		}
	}

	//requires the GameObject that is going to be attacked
	public void Attack(GameObject obj) {
		currentTarget = obj;
	}
}

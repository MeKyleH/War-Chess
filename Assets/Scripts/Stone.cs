using UnityEngine;
using System.Collections;

public class Stone : MonoBehaviour {

	private Animator animator;

	void Start() {
		animator = GetComponent<Animator> ();
	}

	void OnTriggerStay2S (Collider2D collider) {
		Attacker attacker = collider.gameObject.GetComponent<Attacker> ();

		if (attacker) {
			animator.SetTrigger ("underAttack trigger");
		}
	}
}

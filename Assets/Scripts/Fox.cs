using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Attacker))]
public class Fox : MonoBehaviour {

	private Animator anim;
	private Attacker attacker;

	void Start () {
		anim = GetComponent<Animator> ();
		attacker = GetComponent<Attacker> ();
	}
	
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider) {
		//object being collided with
		GameObject obj = collider.gameObject;

		//Quit method if not colliding with a defender
		if (!obj.GetComponent<Defender>()) {
			return;
		}

		if (obj.GetComponent<Stone> ()) {
			anim.SetTrigger ("jump trigger");
		} else {
			anim.SetBool ("isAttacking", true);
			attacker.Attack (obj);
		}
	}
}

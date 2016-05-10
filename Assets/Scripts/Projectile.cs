using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float speed;
	public float damage;

	void Update() {
		transform.Translate (Vector3.right * speed * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		Attacker attacker = collider.gameObject.GetComponent<Attacker> ();
		Health health = collider.gameObject.GetComponent<Health> ();

		if (attacker && health) {
			health.DealDamage (damage);
			Destroy (gameObject);
		}
	}

}

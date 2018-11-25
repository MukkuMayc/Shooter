using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Mob") {
			return;
		}
		if (collision.collider.tag == "Player") {
			collision.collider.gameObject.GetComponent<Character>().Hit(10f);
		}
		Destroy(gameObject);
	}
}

using UnityEngine;
using System.Collections;

public class Skeleton : MonoBehaviour {

	public GameObject player;
	Animator anim;

	float nextAttack;

	float damage = 10f;

	void Start () {
		anim = GetComponent<Animator>();
		nextAttack = Time.time;
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
		Vector3 direction;

		Vector3 dirOriginal;
		if (player == null) {
			return;
		}
		dirOriginal = direction = player.transform.position - transform.position;
		if(player != null) {
			
			direction.y = 0;

			transform.rotation = Quaternion.Slerp(transform.rotation,
										Quaternion.LookRotation(direction), 0.1f);

			anim.SetBool("isIdle", false);
			if(dirOriginal.magnitude > 1) {
				transform.Translate(0,0,0.05f);
				anim.SetBool("isRunning", true);
				anim.SetBool("isAttacking", false);
				nextAttack = Time.time + 1f;
			}
			else {
				anim.SetBool("isAttacking", true);
				anim.SetBool("isRunning", false);
				if (player != null) {
					Attack();
				}
			}

		}
		else {
			nextAttack = Time.time + 1f;
			anim.SetBool("isIdle", true);
			anim.SetBool("isRunning", false);
			anim.SetBool("isAttacking", false);
		}
	}

	public void Attack () {
		if (nextAttack <= Time.time) {
			player.GetComponent<Character>().Hit(damage);
			nextAttack = Time.time + 2.76f;
		}
	}
}
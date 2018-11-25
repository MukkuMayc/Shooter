using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SciFiSoldier : MonoBehaviour {

	
	public Transform rightGunBone;
	public Transform leftGunBone;
	public Arsenal[] arsenal;

	public GameObject player;

	private Animator animator;

	private Actions actions;

	public GameObject bullet;

	private float nextAttack; 

	void Start () {
		animator = GetComponent<Animator> ();
		actions = GetComponent<Actions> ();
		if (arsenal.Length > 0) {
			SetArsenal(arsenal[0].name);
		}
		nextAttack = Time.time + 0.433f;

		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	public void SetArsenal (string name) {
		foreach (Arsenal hand in arsenal) {
			if (hand.name == name) {
				if (rightGunBone.childCount > 0) {
					Destroy(rightGunBone.GetChild(0).gameObject);
				}
				if (leftGunBone.childCount > 0) {
					Destroy(leftGunBone.GetChild(0).gameObject);
				}
				if (hand.rightGun != null) {
					GameObject newRightGun = (GameObject) Instantiate(hand.rightGun);
					newRightGun.transform.parent = rightGunBone;
					newRightGun.transform.localPosition = Vector3.zero;
					newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
				}
				if (hand.leftGun != null) {
					GameObject newLeftGun = (GameObject) Instantiate(hand.leftGun);
					newLeftGun.transform.parent = leftGunBone;
					newLeftGun.transform.localPosition = Vector3.zero;
					newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
				}
				animator.runtimeAnimatorController = hand.controller;
				return;
			}
		}
	}

	[System.Serializable]
	public struct Arsenal {
		public string name;
		public GameObject rightGun;
		public GameObject leftGun;
		public RuntimeAnimatorController controller;
	}

	Vector3 dir;
	void Update () {
		if (player == null) {
			return;
		}
		dir = player.transform.position - rightGunBone.transform.position;
		Vector3 direction = dir;
		direction.y = 0;
		float angle = Vector3.Angle(direction, transform.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation,
									Quaternion.LookRotation(direction), 0.1f);
		if (direction.magnitude > 5f) {
			nextAttack = Time.time + 1.2f;
			actions.Run();
		}
		else {
			actions.Attack();
		}
	}

	public void Attack () {
		if (nextAttack <= Time.time) {
			GameObject bulInst = Instantiate(bullet, rightGunBone.transform.position, Quaternion.identity);
			bulInst.GetComponent<Rigidbody>().AddForce(dir.normalized * 400f);
			nextAttack = Time.time + 0.433f;
			Destroy(bulInst, 10f);
		}
	}
}

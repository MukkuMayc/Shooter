using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {
	protected float hp;
	
	[SerializeField]
	protected GameObject mainParent;

	[SerializeField]
	protected float maxHp = 30f;

	[SerializeField]
	protected float damage = 5f;

	[SerializeField]
	protected Slider healthSlider;

	[SerializeField]
	protected Text healthText;

	[SerializeField]
	protected GameObject deathScreen;

	void Start () {
		hp = maxHp;
		if (healthSlider != null) {
			healthSlider.maxValue = maxHp;
			healthSlider.value = hp;
		}
		if (healthText != null) {
			healthText.text = "HP: " + hp;
		}
	}
	
	void Update () {
	}

	public void Hit (float damage) {
		hp -= damage;
		if (healthSlider != null) {
			healthSlider.value -= damage;
		}
		if (healthText != null) {
			healthText.text = "HP: " + hp;
		}
		//Debug.Log(gameObject.name + " took " + damage + "damage. " + hp + " hp left");
		if (hp <= 0f) {
			Die();
		}
	}

	public void Attack (Character character) {
		character.Hit(damage);
	}

	void Die () {
		if (tag == "Player") {
			Camera cam = transform.Find("Camera").gameObject.GetComponent<Camera>();
			Destroy(cam.transform.Find("Gun").gameObject);
			cam.gameObject.transform.parent = null;
			deathScreen.GetComponentInChildren<Text>().text = 
			"Game Over\nYour Score: " + cam.GetComponent<Score>().GetScore();
			deathScreen.SetActive(true);
			Cursor.visible = true;
		}
		if (tag == "Mob") {
			GameObject spawner = GameObject.Find("Spawner");
			Debug.Log(spawner);
			spawner.GetComponent<Spawner>().mobCount -= 1;
			GameObject player = GameObject.Find("Player");
			Debug.Log(player);
			player.GetComponentInChildren<Score>().AddScore(10);
		}
		Debug.Log(mainParent.name + " was killed!");
		Destroy(mainParent);

	}
}

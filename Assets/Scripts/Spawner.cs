using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

	[SerializeField]
	Vector3 spawnPlaceCorner1 = Vector3.zero;

	[SerializeField]
	Vector3 spawnPlaceCorner2 = Vector3.zero;

	Vector3 spawnRange;

	[SerializeField]
	GameObject[] mobs;

	public int mobCount = 0;

	int spawnCount = 5;

	[SerializeField]
	GameObject waveCounter;

	[SerializeField]
	Text waveCounterText;

	bool timerStarted;

	float timer;

	void Start () {
		if (mobs.Length == 0) {
			Debug.Log("There is no mobs!");
			Destroy(this);
		}

		for (int i = 0; i < 3; ++i) {
			if (spawnPlaceCorner2[i] < spawnPlaceCorner1[i]) {
				float temp = spawnPlaceCorner1[i];
				spawnPlaceCorner1[i] = spawnPlaceCorner2[i];
				spawnPlaceCorner2[i] = temp;
			}
		}

		spawnRange = spawnPlaceCorner2 - spawnPlaceCorner1;
	}

	void Update () {
		if (mobCount > 0)
			return;
		
		if (!timerStarted) {
			timerStarted = true;
			timer = 3.0f;
			waveCounter.SetActive(true);
		}
		else {
			if (timer > 0) {
				timer -= Time.deltaTime;
				waveCounterText.text = "Next wave in " + Mathf.Floor(10f * timer) / 10f + " sec.";
			}
			else {
				timerStarted = false;
				waveCounter.SetActive(false);
				Spawn(spawnCount);
				spawnCount *= 2;
			}
		}
	}

	void SpawnRand() {
		GameObject mobForSpawn = mobs[Random.Range(0, mobs.Length)];

		Vector3 spawnRange = spawnPlaceCorner2 - spawnPlaceCorner1;

		Vector3 spawnPoint = spawnPlaceCorner1 + 
		new Vector3(spawnRange.x * Random.value, spawnRange.y * Random.value, spawnRange.z * Random.value);

		Instantiate(mobForSpawn, spawnPoint, Quaternion.identity);

		++mobCount;
	}

	public void Spawn(int amount) {
		for (int i = 0; i < amount; ++i) {
			SpawnRand();
		}
	}
}

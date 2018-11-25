using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	Spawner spawner;

	int wayNum = 1;

	void Start () {
		spawner = GetComponent<Spawner>();
	}

	void Update () {
		if (spawner.mobCount > 0)
			return;

		spawner.Spawn(5);
	}
}

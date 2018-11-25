using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

	int score;
	// Use this for initialization
	void Start () {
		score = 0;
	}

	public void AddScore (int _score) {
		score += _score;
	}

	public int GetScore () {
		return score;
	}
}

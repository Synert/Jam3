using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveMeAllTheScrap : MonoBehaviour {

	public Recycle recycler;

	void AllTheScrap() {
		recycler.Money = 1337420;
	}

	void resetHighScore() {
		PlayerPrefs.SetInt("Highscore", 0);
	}
}

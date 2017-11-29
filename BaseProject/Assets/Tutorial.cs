using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	public float originalTimer;
	public float timer;
	public bool isNotTutorial;

	void Start() {
		originalTimer = timer;
	}

	void Update() {
		if (!isNotTutorial) {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				pressed ();
			}
		}
	}

	void pressed() {
		SpriteRenderer[] temp = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sr in temp) {
			sr.enabled = false;
			GetComponent<Text> ().enabled = false;
		}
	}

	public void reactivate() {
		timer = originalTimer;
		SpriteRenderer[] temp = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sr in temp) {
			sr.enabled = true;
			GetComponent<Text> ().enabled = true;
		}
	}

	void reactivateAgain() {
		Tutorial[] all = GameObject.FindObjectsOfType<Tutorial> ();
		foreach (Tutorial tut in all) {
			tut.reactivate ();
		}
		GenerateStartingScrap temp = GameObject.FindObjectOfType<GenerateStartingScrap> ();
		if (temp) {
			temp.reactivate ();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	public float originalTimer;
	public float timer;
	public bool isNotTutorial;
	public GameObject[] tutorials;
	public GameObject tutorialLink;

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
		if (GetComponent<Text> ()) {
			GetComponent<Text> ().enabled = false;
		}
	}

	public void reactivate() {
		gameObject.SetActive (true);
		timer = originalTimer;
		enabled = true;
		SpriteRenderer[] temp = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sr in temp) {
			sr.enabled = true;
			GetComponent<Text> ().enabled = true;
		}
		if (GetComponent<Text> ()) {
			GetComponent<Text> ().enabled = true;
		}
	}

	public void grabLinkTime() {
		timer = tutorialLink.GetComponent<Tutorial> ().timer;
		tutorialLink.SetActive (false);
	}

	void reactivateAgain() {
		foreach (GameObject obj in tutorials) {
			if (obj.GetComponent<Tutorial> ()) {
				obj.GetComponent<Tutorial> ().reactivate ();
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateStartingScrap : MonoBehaviour {

	public float startingScrap = 0;
	public GameObject scrapPrefab;
	public float delay = 0;
	public float timer;
	float originalTimer;
	public float currentDelay = 0;

	// Use this for initialization
	void Start () {
		currentDelay = delay;
		originalTimer = timer;
	}
	
	// Update is called once per frame
	void Update () {
		if (startingScrap > 0) {
			if (currentDelay >= 0) {
				currentDelay -= Time.deltaTime;
			} else {
				currentDelay = delay;
				if (startingScrap > 0) {

					startingScrap -= 1;
					GameObject.Instantiate (scrapPrefab, transform.position, transform.rotation);
					if (startingScrap == 0) {
						transform.parent.GetComponent<Text> ().enabled = false;
						SpriteRenderer[] temp = transform.parent.GetComponentsInChildren<SpriteRenderer> ();
						foreach (SpriteRenderer sr in temp) {
							sr.enabled = false;
						}
					}
				}
			}
		} else {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				transform.parent.GetComponent<Text> ().enabled = false;
				SpriteRenderer[] temp = transform.parent.GetComponentsInChildren<SpriteRenderer> ();
				foreach (SpriteRenderer sr in temp) {
					sr.enabled = false;
				}
			}
		}

	}

	public void reactivate() {
		transform.parent.GetComponent<Text> ().enabled = true;
		SpriteRenderer[] temp = transform.parent.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sr in temp) {
			sr.enabled = true;
		}
		timer = originalTimer;
	}
}

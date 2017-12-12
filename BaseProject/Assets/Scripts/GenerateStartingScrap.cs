using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateStartingScrap : MonoBehaviour {

	public float startingScrap = 0;
	public GameObject scrapPrefab;
	public float delay = 0;
	public float currentDelay = 0;

	// Use this for initialization
	void Start () {
		currentDelay = delay;
	}
	
	// Update is called once per frame
	void Update () {
		if (startingScrap > 0) {
			if (currentDelay >= 0) {
				currentDelay -= Time.deltaTime;
			} else {
				currentDelay = delay;
				startingScrap -= 1;
				GameObject.Instantiate (scrapPrefab, transform.position, transform.rotation);
				if (startingScrap == 0) {
					GameObject.Destroy (this.gameObject);
				}
			}
		}
	}
}

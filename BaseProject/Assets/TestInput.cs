using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour {

	public int controllerTest = 0;
	public GameObject[] controllerVisuals;
	public GameObject[] keyboardVisuals;
	public GameObject[] controllerTutorial;
	public GameObject[] keyboardTutorial;
	public bool previousController = false;
	public bool controller = false;
	bool firstUpdate = true;

	// Update is called once per frame
	void Update ()
	{
		if (firstUpdate) {
			if (GameObject.FindObjectOfType<ControllerPoll> ().Controllers [controllerTest]) {
				controller = true;
			} else {
				previousController = true;
			}
			firstUpdate = false;
		} else {
			if (controller) {
				foreach (GameObject obj in controllerVisuals) {
					obj.SetActive (true);
				}
				foreach (GameObject obj in keyboardVisuals) {
					obj.SetActive (false);
				}
				if (controller != previousController) {
					foreach (GameObject obj in controllerTutorial) {
						obj.GetComponent<Tutorial> ().grabLinkTime ();
						obj.SetActive (true);
					}
				}
			} else {
				foreach (GameObject obj in controllerVisuals) {
					obj.SetActive (false);
				}
				foreach (GameObject obj in keyboardVisuals) {
					obj.SetActive (true);
				}
				if (controller != previousController) {
					foreach (GameObject obj in keyboardTutorial) {
						obj.GetComponent<Tutorial> ().grabLinkTime ();
						obj.SetActive (true);
					}
				}
			}
			previousController = controller;
		}
	}
}

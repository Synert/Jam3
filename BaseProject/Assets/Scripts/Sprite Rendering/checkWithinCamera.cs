using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkWithinCamera : MonoBehaviour {

	public Camera cam;
	public GridBuilding building;
	public List<gridSection> sectionsInRender;
	public Vector2 pos;
	public Vector2 previousPos = -Vector2.one;
	public int renderSize;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		building = GameObject.FindObjectOfType<GridBuilding> ();
	}
	
	// Update is called once per frame
	void Update () {
		pos = cam.transform.position;
		if (previousPos != pos) {
			testSections ();
		}
		previousPos = pos;
	}

	public void testSections() {
		if (pos != previousPos) {
			clearSections ();
			for (int a = -renderSize; a < renderSize; a++) {
				for (int b = -renderSize; b < renderSize; b++) {
					int index = building.testRaw (pos + new Vector2 (a * building.size.x, b * building.size.y));
					if (index == -1) {
						continue;
					}
					gridSection temp = building.grid [index];
					addSections (temp);
				}
			}
		} else {
			for (int a = 0; a < sectionsInRender.Count; a++) {
				checkSection (sectionsInRender [a]);
			}
		}
	}

	void clearSections() {
		for (int a = 0; a < sectionsInRender.Count; a++) {
			if (sectionsInRender [a].obj) {
				sectionsInRender [a].obj.GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
		sectionsInRender.Clear ();
	}

	void addSections(gridSection section) {
		sectionsInRender.Add (section);
		if (section.obj) {
			section.obj.GetComponent<SpriteRenderer> ().enabled = true;
		}
	}

	void checkSection(gridSection section) {
		if (section.obj) {
			section.obj.GetComponent<SpriteRenderer> ().enabled = true;
		}
	}
}

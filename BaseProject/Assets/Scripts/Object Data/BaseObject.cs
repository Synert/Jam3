using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {

	public string name;
	public int hp;
	public int index;
	public GridBuilding building;
	public bool show = false;

	void Start() {
		building = GameObject.FindObjectOfType<GridBuilding> ();
	}

	public void Update() {
		if (testDistance()) {
			GetComponent<SpriteRenderer> ().enabled = true;
		} else {
			GetComponent<SpriteRenderer> ().enabled = false;
		}
	}

	bool testDistance() {
		Vector3 camPos = Camera.main.transform.position;
		float size = Camera.main.orthographicSize + 1;
		Vector2 relativePos = new Vector2 (transform.position.x - camPos.x, transform.position.y - camPos.y);

		if (show) {
			Debug.Log (camPos);
			Debug.Log (relativePos);
		}

		if ((relativePos.x > -size && relativePos.x < size) &&
			(relativePos.y > -size && relativePos.y < size)) {
			return true;
		}

		return false;
	}

	public void takeDamage(int damage) {
		hp -= damage;
		testHealth ();
	}

	void testHealth() {
		if (hp <= 0) {
			building.resetPosition (index);
			GameObject.Destroy (this.gameObject);
		}
	}
}
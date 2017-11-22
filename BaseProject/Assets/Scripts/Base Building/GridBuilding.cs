using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilding : MonoBehaviour {

	public Vector2 startPos;
	public Vector2 size;
	public Vector2 gridSections;
	public List<gridSection> grid = new List<gridSection> ();
	public GameObject obj;

	public Color col;

	// Use this for initialization
	void Start () {
		for (int a = 0; a < gridSections.x; a++) {
			for (int b = 0; b < gridSections.y; b++) {
				grid.Add (new gridSection (new Vector4 (startPos.x + (a * size.x), startPos.y + (b * size.y), startPos.x + ((a + 1) * size.x), startPos.y + ((b + 1) * size.y)), (a * (int)gridSections.y) + b));
			}
		}

		LineRenderer lr = GetComponent<LineRenderer> ();
		lr.positionCount = (5 * (int)gridSections.y) + (5 * (int)gridSections.x);

		int aCost = 0;
		for (int a = 0; a < gridSections.y * 5; a+= 5) {
			int aOffset = a / 5;
			lr.SetPosition(a, new Vector3(startPos.x - (size.x / 2), startPos.y + (aOffset * size.y) - (size.y / 2), 0));
			lr.SetPosition(a + 1, new Vector3(startPos.x + (size.x * gridSections.x) - (size.x / 2), startPos.y + (aOffset * size.y) - (size.y / 2), 0));
			lr.SetPosition(a + 2, new Vector3(startPos.x + (size.x * gridSections.x) - (size.x / 2), startPos.y + (aOffset * size.y) + size.y - (size.y / 2), 0));
			lr.SetPosition(a + 3, new Vector3(startPos.x - (size.x / 2), startPos.y + (aOffset * size.y) + size.y - (size.y / 2), 0));
			lr.SetPosition(a + 4, new Vector3(startPos.x - (size.x / 2), startPos.y + (aOffset * size.y) - (size.y / 2), 0));
			aCost += 5;
		}

		for (int a = 0; a < gridSections.x * 5; a+= 5) {
			int aOffset = a / 5;
			lr.SetPosition(a + aCost, new Vector3(startPos.x + (aOffset * size.x) - (size.x / 2), startPos.y - (size.y / 2), 0));
			lr.SetPosition(a + 1 + aCost, new Vector3(startPos.x + (aOffset * size.x) - (size.x / 2), startPos.y + (size.y * gridSections.y) - (size.y / 2), 0));
			lr.SetPosition(a + 2 + aCost, new Vector3(startPos.x + (aOffset * size.x) + size.x - (size.x / 2), startPos.y + (size.y * gridSections.y) - (size.y / 2), 0));
			lr.SetPosition(a + 3 + aCost, new Vector3(startPos.x + (aOffset * size.x) + size.x - (size.x / 2), startPos.y - (size.y / 2), 0));
			lr.SetPosition(a + 4 + aCost, new Vector3(startPos.x + (aOffset * size.x) - (size.x / 2), startPos.y - (size.y / 2), 0));
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			int temp = test ();
			if (temp != -1) {
				if (grid [temp].full == false) {
					grid [temp].full = true;
					grid [temp].obj = GameObject.Instantiate(obj, new Vector3(grid [temp].xy.x,grid [temp].xy.y,0),transform.rotation);
					grid [temp].obj.GetComponent<BaseObject> ().index = temp;
				}
			}
		}
		if (Input.GetMouseButton (1)) {
			int temp = test ();
			if (temp != -1) {
				if (grid [temp].full == true) {
					grid [temp].full = false;
					GameObject.Destroy(grid [temp].obj);
					grid [temp].obj = null;
				}
			}
		}
		if (Input.GetMouseButtonDown (2)) {
			paintCan ();
		}

	}

	/*
	void OnDrawGizmos()
	{
		if (grid.Count != 0) {
			for (int a = 0; a < gridSections.x; a++) {
				for (int b = 0; b < gridSections.y; b++) {
					Gizmos.color = grid[(a * (int)gridSections.y) + b].col;
					Gizmos.DrawCube (new Vector3 (startPos.x + (a * size.x) + (size.x / 2), startPos.y + (b * size.y) + (size.y / 2), 0), size);
					//Gizmos.color = Color.black;
					//Gizmos.DrawWireCube (new Vector3 (startPos.x + (a * size.x) + (size.x / 2), startPos.y + (b * size.y) + (size.y / 2), 0), size);
				}
			}
		}
	}
	*/

	int test() {
		Vector2 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		int index = (int)(((int)((pos.x - startPos.x) / size.x)) * gridSections.y) + ((int)((pos.y - startPos.y) / size.y));
		//Debug.Log ((int)(((int)((pos.x - startPos.x) / size.x)) * gridSections.y) + ((int)((pos.y - startPos.y) / size.y)));

		if (pos.x < startPos.x ||
			pos.x > startPos.x + (gridSections.x * size.x) ||
			pos.y < startPos.y ||
			pos.y > startPos.y + (gridSections.y * size.y)) {
			return -1;
		}

		return index;
	}

	List<gridSection> GetSegments(Vector2 minMax, string search = "") {
		List<gridSection> returnValue = new List<gridSection> ();
		for (int a = 0; a <= minMax.y - minMax.x; a++) {
			for (int b = 0; b < gridSections.y; b++) {
				if (search == "") {
					returnValue.Add (grid [((int)minMax.x + a) + (b * (int)gridSections.x)]);
				} else {
					if (grid [((int)minMax.x + a) + (b * (int)gridSections.x)].obj) {
						if (grid [((int)minMax.x + a) + (b * (int)gridSections.x)].obj.GetComponent<BaseObject>()) {
							if (grid [((int)minMax.x + a) + (b * (int)gridSections.x)].obj.GetComponent<BaseObject>().name == search) {
								returnValue.Add (grid [((int)minMax.x + a) + (b * (int)gridSections.x)]);
							}
						}
					}
				}
			}
		}
		for (int a = 0; a < returnValue.Count; a++) {
			returnValue [a].full = true;
		}
		return returnValue;
	}

	public int GetTurrets(Vector2 minMax) {
		return GetSegments (minMax, "Turret").Count;
	}

	public int GetArmour(Vector2 minMax) {
		return GetSegments (minMax, "Armour").Count;
	}

	public float GetSegmentHeight() {
		return 0;
	}





	//fun
	void paintCan() {
		List<gridSection> checkd = new List<gridSection>();
		List<gridSection> queue = new List<gridSection>();
		int temp = test ();
		GameObject origObj = null;
		if (temp != -1) {
			queue.Add (grid [temp]);
			origObj = grid [temp].obj;
		}


		while (queue.Count != 0) {
			if (!checkd.Contains (queue [0])) {

				if (queue [0].xy.x > startPos.x) {
					if (!checkd.Contains (grid [queue [0].index - (int)gridSections.y])) {
						if (testObject(queue [0].index - (int)gridSections.y,origObj)) {
							queue.Add (grid [queue [0].index - (int)gridSections.y]);
						}
					}
				}
				if (queue [0].xy.x < startPos.x + (gridSections.x - 1)) {
					if (!checkd.Contains (grid [queue [0].index + (int)gridSections.y])) {
						if (testObject(queue [0].index + (int)gridSections.y,origObj)) {
							queue.Add (grid [queue [0].index + (int)gridSections.y]);
						}
					}
				}
				if (queue [0].xy.y > startPos.y) {
					if (!checkd.Contains (grid [queue [0].index - 1])) {
						if (testObject(queue [0].index - 1,origObj)) {
							queue.Add (grid [queue [0].index - 1]);
						}
					}
				}
				if (queue [0].xy.y < startPos.x + (gridSections.y - 1)) {
					if (!checkd.Contains (grid [queue [0].index + 1])) {
						if (testObject(queue [0].index + 1, origObj)) {
							queue.Add (grid [queue [0].index + 1]);
						}
					}
				}
				if (queue [0].obj != null) {
					if (queue [0].obj.name != obj.name) {
						queue [0].obj.GetComponent<BaseObject>().takeDamage(int.MaxValue);
						queue [0].full = false;
					} else {
						queue [0].full = true;
					}
				}

				if (queue [0].full == false) {
					queue [0].obj = GameObject.Instantiate (obj, new Vector3 (queue [0].xy.x, queue [0].xy.y, 0), transform.rotation);
					queue [0].full = true;
				}

			}
			checkd.Add (queue [0]);
			queue.RemoveAt (0);
		}


	}

	bool testObject(int _index, GameObject test) {
		if (!grid [_index].obj && test == null) {
			return true;
		}
		if (grid [_index].obj) {
			if (test) {
				if (grid [_index].obj.name == test.name) {
					return true;
				}
			}
		}
		return false;
	}

	public void resetPosition(int _index) {
		grid [_index].full = false;
		grid [_index].obj = null;
	}


}

[System.Serializable]
public class gridSection 
{
	public Vector4 xy = Vector4.zero;
	public bool full = false;
	public Color col = Color.white;
	public GameObject obj = null;
	public int index;

	public gridSection(Vector4 _xy, int _index) {
		xy = _xy;
		index = _index;
	}
}
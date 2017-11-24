using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilding : MonoBehaviour {

	public Vector2 startPos;
	public Vector2 size;
	public Vector2I gridSections;
	public List<gridSection> grid = new List<gridSection> ();
	public GameObject obj;
	public bool enableLineRenderer = false;
	public int maxHeight = -int.MaxValue;

	// Use this for initialization
	void Start () {
		for (int a = 0; a < gridSections.x; a++) {
			for (int b = 0; b < gridSections.y; b++) {
				grid.Add (new gridSection (new Vector4 (startPos.x + (a * size.x), startPos.y + (b * size.y), startPos.x + ((a + 1) * size.x), startPos.y + ((b + 1) * size.y)), new Vector3 ((a * (int)gridSections.y) + b, a, b)));
			}
		}
			
		LineRenderer lr = GetComponent<LineRenderer> ();
		lr.enabled = enableLineRenderer;
		lr.positionCount = (5 * (int)gridSections.y) + (5 * (int)gridSections.x);

		int aCost = 0;
		for (int a = 0; a < gridSections.y * 5; a += 5) {
			int aOffset = a / 5;
			lr.SetPosition (a, new Vector3 (startPos.x - (size.x / 2), startPos.y + (aOffset * size.y) - (size.y / 2), 0));
			lr.SetPosition (a + 1, new Vector3 (startPos.x + (size.x * gridSections.x) - (size.x / 2), startPos.y + (aOffset * size.y) - (size.y / 2), 0));
			lr.SetPosition (a + 2, new Vector3 (startPos.x + (size.x * gridSections.x) - (size.x / 2), startPos.y + (aOffset * size.y) + size.y - (size.y / 2), 0));
			lr.SetPosition (a + 3, new Vector3 (startPos.x - (size.x / 2), startPos.y + (aOffset * size.y) + size.y - (size.y / 2), 0));
			lr.SetPosition (a + 4, new Vector3 (startPos.x - (size.x / 2), startPos.y + (aOffset * size.y) - (size.y / 2), 0));
			aCost += 5;
		}

		for (int a = 0; a < gridSections.x * 5; a += 5) {
			int aOffset = a / 5;
			lr.SetPosition (a + aCost, new Vector3 (startPos.x + (aOffset * size.x) - (size.x / 2), startPos.y - (size.y / 2), 0));
			lr.SetPosition (a + 1 + aCost, new Vector3 (startPos.x + (aOffset * size.x) - (size.x / 2), startPos.y + (size.y * gridSections.y) - (size.y / 2), 0));
			lr.SetPosition (a + 2 + aCost, new Vector3 (startPos.x + (aOffset * size.x) + size.x - (size.x / 2), startPos.y + (size.y * gridSections.y) - (size.y / 2), 0));
			lr.SetPosition (a + 3 + aCost, new Vector3 (startPos.x + (aOffset * size.x) + size.x - (size.x / 2), startPos.y - (size.y / 2), 0));
			lr.SetPosition (a + 4 + aCost, new Vector3 (startPos.x + (aOffset * size.x) - (size.x / 2), startPos.y - (size.y / 2), 0));
		}
	}

	// Update is called once per frame
	void Update () {
		GetComponent<LineRenderer> ().enabled = enableLineRenderer;
		if (Input.GetMouseButton (0)) {
			int temp = test ();
			if (temp != -1) {
				if (grid [temp].full == false) {
					createObj (temp, obj);
				}
			}
		}
		if (Input.GetMouseButton (1)) {
			int temp = test ();
			if (temp != -1) {
				if (grid [temp].indexAB.y >= maxHeight) {
					//maxHeight = findMaxHeight (temp);
				}
				if (grid [temp].full == true) {
					destroyObjsAbove (temp);
					//destroyObj(temp);
				}
			}
		}
		if (Input.GetMouseButtonDown (2)) {
			fillHole ();
		}
	}

	void findMaxHeight (int _index) {
		checkSidesData temp = checkSides (_index);

		if (temp.down) {

		}
	}

	void destroyObjsAbove(int _index) {

		checkSidesData temp = checkSides (_index);
		List<gridSection> tempSections = new List<gridSection> ();
		int highestReached = 0;

		destroyObj (_index);

		if (temp.left) {
			if (grid [temp.leftIndex].full) {
				if (!findGround (grid [temp.leftIndex])) {
					List<gridSection> above = gatherSpaces (new findOptionsOpenClosed (true), grid [temp.leftIndex]);
					for (int a = 0; a < above.Count; a++) {
						destroyObj (above [a].index);
					}
				}
			}
		}


		if (temp.right) {
			if (grid [temp.rightIndex].full) {
				if (!findGround (grid [temp.rightIndex])) {
					List<gridSection> above = gatherSpaces (new findOptionsOpenClosed (true), grid [temp.rightIndex]);
					for (int a = 0; a < above.Count; a++) {
						destroyObj (above [a].index);
					}
				}
			}
		}


		if (temp.up) {
			if (grid [temp.upIndex].full) {
				if (!findGround (grid [temp.upIndex])) {
					List<gridSection> above = gatherSpaces (new findOptionsOpenClosed (true), grid [temp.upIndex]);
					for (int a = 0; a < above.Count; a++) {
						destroyObj (above [a].index);
					}
				}
			}
		}
			
		if (temp.down) {
			if (grid [temp.downIndex].full) {
				if (!findGround (grid [temp.downIndex])) {
					List<gridSection> above = gatherSpaces (new findOptionsOpenClosed (true), grid [temp.downIndex]);
					for (int a = 0; a < above.Count; a++) {
						destroyObj (above [a].index);
					}
				}
			}
		}

	}

	bool checkBlockPlaceAvailable(int _index, bool testCase) {
		checkSidesData temp = checkSides(_index);

		if (temp.left) {
			if (grid [temp.leftIndex].full == testCase) {
				return true;
			}
		}

		if (temp.right) {
			if (grid [temp.rightIndex].full == testCase) {
				return true;
			}
		}

		if (temp.up) {
			if (grid [temp.upIndex].full == testCase) {
				return true;
			}
		}

		if (temp.down) {
			if (grid [temp.downIndex].full == testCase) {
				return true;
			}
		}

		return grid[_index].indexAB.y == 0;
	}

	void createObj(int _index, GameObject _obj) {
		if (checkBlockPlaceAvailable(_index, true)) {
			if (grid [_index].full) {
				destroyObj (_index);
			}
			if (grid [_index].indexAB.y > maxHeight) {
				maxHeight = grid [_index].indexAB.y;
			}
			grid [_index].full = true;
			grid [_index].obj = GameObject.Instantiate (_obj, new Vector3 (grid [_index].xy.x, grid [_index].xy.y, 0), transform.rotation);
			grid [_index].obj.GetComponent<SpriteRenderer> ().enabled = false;
			grid [_index].obj.GetComponent<BaseObject> ().index = _index;
		}
	}

	void destroyObj(int _index) {
		grid [_index].full = false;
		if (grid [_index].obj) {
			GameObject.Destroy (grid [_index].obj);
		}
		grid [_index].obj = null;
	}

	int test() {
		Vector2 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		int index = Mathf.FloorToInt(((pos.x + (size.x/2)) - startPos.x) / size.x) * gridSections.y + Mathf.FloorToInt(((pos.y + (size.y/2)) - startPos.y) / size.y);

		if (pos.x < startPos.x - (size.x) ||
			pos.x >= startPos.x + ((gridSections.x - 1) * size.x + (size.x / 2)) ||
			pos.y < startPos.y - (size.y) ||
			pos.y >= startPos.y + ((gridSections.y - 1) * size.y + (size.y / 2)) ||
			index < 0 || index > grid.Count) {
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
		return size.y;
	}

	public int GetMaxHeight() {
		return maxHeight;
	}

	bool findGround() {

		return false;
	}

	void fillHole() {
		GameObject objToChangeTo = null;
		int temp = test ();
		if (temp != -1) {
			objToChangeTo = grid [temp].obj;
			List<gridSection> sections = gatherSpaces (new findOptionsFill (objToChangeTo), grid[temp]);
			for (int a = 0; a < sections.Count; a++) {
				createObj (sections [a].index, obj);
			}
		}
	}

	//check each side of index
	checkSidesData checkSides(int _index) {
		checkSidesData val = new checkSidesData ();

		//left
		if (grid [_index].indexAB.x > 0) {
			if (grid [_index - gridSections.y].check) {
				val.left = true;
				val.leftIndex = grid [_index].index - gridSections.y;
			}
		}

		//right
		if (grid [_index].indexAB.x < (gridSections.x - 1)) {
			if (grid [_index + gridSections.y].check) {
				val.right = true;
				val.rightIndex = grid [_index].index + gridSections.y;
			}
		}

		//up
		if (grid [_index].indexAB.y < (gridSections.y - 1)) {
			if (grid [_index + 1].check) {
				val.up = true;
				val.upIndex = grid [_index].index + 1;
			}
		}

		//down
		if (grid [_index].indexAB.y > 0) {
			if (grid [_index - 1].check) {
				val.down = true;
				val.downIndex = grid [_index].index - 1;
			}
		}

		return val;
	}

	bool checkIndex(int _index, bool testCase) {
		//check grid spot open/full
		grid [_index].check = false;
		return (grid [_index].full == testCase);
	}

	void checkIndexAdd(int _index, List<gridSection> queue, List<gridSection> checkd, bool testCase) {
		//check grid spot open/full and add to queue if true
		if (checkIndex (_index, testCase)) {
			queue.Add (grid [_index]);
		} else {
			checkd.Add (grid [_index]);
		}
	}

	//check if sides are full/empty
	void checkSidesLoop(List<gridSection> queue, List<gridSection> checkd, checkSidesData _data, bool testCase) {

		//left
		if (_data.left) {
			checkIndexAdd (_data.leftIndex, queue, checkd, testCase);
		}

		//right
		if (_data.right) {
			checkIndexAdd (_data.rightIndex, queue, checkd, testCase);
		}

		//up
		if (_data.up) {
			checkIndexAdd (_data.upIndex, queue, checkd, testCase);
		}

		//down
		if (_data.down) {
			checkIndexAdd (_data.downIndex, queue, checkd, testCase);
		}
	}

	bool checkIndexObject(int _index, GameObject _obj) {
		grid [_index].check = false;
		//check if obj exists
		if (_obj) {
			//check if grid has obj
			if (grid [_index].obj) {
				//return if obj is same as grid's
				return _obj.name == grid [_index].obj.name;
			}
		}
		//return if both objs are null
		return (_obj == null && grid [_index].obj == null);
	}

	void checkIndexObjectAdd(int _index, List<gridSection> queue, List<gridSection> checkd, GameObject testCase) {
		//check if object is the same as index and add if true
		if (checkIndexObject (_index, testCase)) {
			queue.Add (grid [_index]);
		} else {
			checkd.Add (grid [_index]);
		}
	}

	//check if sides has obj similar to testCase
	void checkSidesLoop(List<gridSection> queue, List<gridSection> checkd, checkSidesData _data, GameObject testCase) {

		//left
		if (_data.left) {
			checkIndexObjectAdd (_data.leftIndex, queue, checkd, testCase);
		}

		//right
		if (_data.right) {
			checkIndexObjectAdd (_data.rightIndex, queue, checkd, testCase);
		}

		//up
		if (_data.up) {
			checkIndexObjectAdd (_data.upIndex, queue, checkd, testCase);
		}

		//down
		if (_data.down) {
			checkIndexObjectAdd (_data.downIndex, queue, checkd, testCase);
		}
	}
	 
	void resetChecked(List<gridSection> reset) {
		for (int a = 0; a < reset.Count; a++) {
			reset [a].check = true;
		}
	}

	void finishedCheckingObject(List<gridSection> queue, List<gridSection> checkd)
	{
		queue [0].check = false;
		checkd.Add (queue [0]);
		queue.RemoveAt (0);
	}
		
	List<gridSection> gatherSpaces(findOptions option, gridSection startSection) {
		List<gridSection> checkd = new List<gridSection> ();
		List<gridSection> queue = new List<gridSection> ();

		queue.Add (startSection);

		List<gridSection> returnVal = new List<gridSection> ();

		checkSidesData sideData = new checkSidesData();

		if (option.type == findOption.findOpenClosed) {
			bool testCase = ((findOptionsOpenClosed)option).open;
			while (queue.Count != 0) {
				sideData = checkSides (queue [0].index);
				checkSidesLoop (queue, checkd, sideData, testCase);
				returnVal.Add (queue [0]);
				finishedCheckingObject (queue, checkd);
			}
		} else if (option.type == findOption.findSimilar) {
			GameObject testObj = ((findOptionsFill)option).obj;
			while (queue.Count != 0) {
				sideData = checkSides (queue [0].index);
				checkSidesLoop (queue, checkd, sideData, testObj);
				returnVal.Add (queue [0]);
				finishedCheckingObject (queue, checkd);
			}
		} 

		resetChecked (checkd);

		return returnVal;

	}

	bool findGround(gridSection startSection) {
		List<gridSection> checkd = new List<gridSection> ();
		List<gridSection> queue = new List<gridSection> ();

		queue.Add (startSection);

		checkSidesData sideData = new checkSidesData();
		while (queue.Count != 0) {
				
			if (queue [0].indexAB.y == 0) {
				resetChecked (checkd);
				resetChecked (queue);
				return true;
			}

			sideData = checkSides (queue [0].index);
			checkSidesLoop (queue, checkd, sideData, true);
			finishedCheckingObject (queue, checkd);
		}

		resetChecked (checkd);

		return false;
	}

	int findHighest(List<gridSection> startSections, out int highestReached) {
		List<gridSection> checkd = new List<gridSection> ();
		List<gridSection> queue = new List<gridSection> ();

		for (int a = 0; a < startSections.Count; a++) {
			queue.Add (startSections [a]);
		}

		highestReached = -1;

		checkSidesData sideData = new checkSidesData();
		while (queue.Count != 0) {
			if (queue [0].indexAB.y > highestReached) {
				highestReached = queue [0].indexAB.y;
			}
			sideData = checkSides (queue [0].index);
			checkSidesLoop (queue, checkd, sideData, true);
			finishedCheckingObject (queue, checkd);
		}


		resetChecked (checkd);

		return -1;
	}

	public void resetPosition(int _index) {
		grid [_index].full = false;
		grid [_index].obj = null;
	}


}

/*
		else if (option.type == findOption.findGround) {
			bool testCase = ((findOptionsOpenClosed)option).open;
			while (queue.Count != 0) {
				if (queue [0].indexAB.y == 0) {
					resetChecked (checkd);
					//return list with 1 value to act as true
					List<gridSection> returnVal = new List<gridSection>();
					returnVal.Add(queue [0]);
					return returnVal;
				}
				sideData = checkSides (queue [0].index);
				checkSidesLoop (queue, sideData, testCase);
				finishedCheckingObject (queue, checkd);
			}
		}
		*/

[System.Serializable]
public class gridSection 
{
	public Vector4 xy = Vector4.zero;
	public bool full = false;
	public GameObject obj = null;
	public int index;
	public Vector2I indexAB;
	public bool check = true;

	public gridSection(Vector4 _xy, int _index) {
		xy = _xy;
		index = _index;
	}

	public gridSection(Vector4 _xy, Vector3 _index) {
		xy = _xy;
		index = (int)_index.x;
		indexAB = new Vector2I (_index.y, _index.z);
	}
}

[System.Serializable]
public class checkSidesData
{
	public bool left = false, right = false, up = false, down = false;
	public int leftIndex, rightIndex, upIndex, downIndex;
}
	
public class findOptions {
	public findOption type;

}

public class findOptionsOpenClosed : findOptions {
	public bool open;

	public findOptionsOpenClosed(bool _open) {
		open = _open;
		type = findOption.findOpenClosed;
	}
}

public class findOptionsFill : findOptions {
	public GameObject obj;

	public findOptionsFill(GameObject _obj) {
		obj = _obj;
		type = findOption.findSimilar;
	}
}

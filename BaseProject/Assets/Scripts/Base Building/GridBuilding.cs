using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilding : MonoBehaviour {

	public Vector2 startPos;
	public Vector2 size;
	public Vector2I gridSections;
	public List<gridSection> grid = new List<gridSection> ();
	public GameObject[] obj;
	public int currentObj = 0;
	public int currentRotation = 0;
	public bool enableLineRenderer = false;
	public int maxHeight = 0;
	public Vector2I ground;
	public int maxDropBeforeDeath = 10;
	public GameObject cursor;
	public GameObject overlay;
	public GameObject scrapPrefab;
	public Gameover gameover;
	public int state = 0;
    public GameObject audios;

	// Use this for initialization
	void Start()
	{
        audios = GameObject.Find("AudioObject");
		cursor = GameObject.FindObjectOfType<MoveCursor>().gameObject;
		gameover = GetComponent<Gameover>();
		for (int a = 0; a < gridSections.x; a++)
		{
			for (int b = 0; b < gridSections.y; b++)
			{
				grid.Add(new gridSection(new Vector4(startPos.x + (a * size.x), startPos.y + (b * size.y), startPos.x + ((a + 1) * size.x), startPos.y + ((b + 1) * size.y)), new Vector3((a * (int)gridSections.y) + b, a, b)));
			}
		}

		LineRenderer lr = GetComponent<LineRenderer>();
		lr.enabled = enableLineRenderer;
		lr.positionCount = (5 * (int)gridSections.y) + (5 * (int)gridSections.x);

		int aCost = 0;
		for (int a = 0; a < gridSections.y * 5; a += 5)
		{
			int aOffset = a / 5;
			lr.SetPosition(a, new Vector3(startPos.x - (size.x / 2), startPos.y + (aOffset * size.y) - (size.y / 2), 0));
			lr.SetPosition(a + 1, new Vector3(startPos.x + (size.x * gridSections.x) - (size.x / 2), startPos.y + (aOffset * size.y) - (size.y / 2), 0));
			lr.SetPosition(a + 2, new Vector3(startPos.x + (size.x * gridSections.x) - (size.x / 2), startPos.y + (aOffset * size.y) + size.y - (size.y / 2), 0));
			lr.SetPosition(a + 3, new Vector3(startPos.x - (size.x / 2), startPos.y + (aOffset * size.y) + size.y - (size.y / 2), 0));
			lr.SetPosition(a + 4, new Vector3(startPos.x - (size.x / 2), startPos.y + (aOffset * size.y) - (size.y / 2), 0));
			aCost += 5;
		}

		for (int a = 0; a < gridSections.x * 5; a += 5)
		{
			int aOffset = a / 5;
			lr.SetPosition(a + aCost, new Vector3(startPos.x + (aOffset * size.x) - (size.x / 2), startPos.y - (size.y / 2), 0));
			lr.SetPosition(a + 1 + aCost, new Vector3(startPos.x + (aOffset * size.x) - (size.x / 2), startPos.y + (size.y * gridSections.y) - (size.y / 2), 0));
			lr.SetPosition(a + 2 + aCost, new Vector3(startPos.x + (aOffset * size.x) + size.x - (size.x / 2), startPos.y + (size.y * gridSections.y) - (size.y / 2), 0));
			lr.SetPosition(a + 3 + aCost, new Vector3(startPos.x + (aOffset * size.x) + size.x - (size.x / 2), startPos.y - (size.y / 2), 0));
			lr.SetPosition(a + 4 + aCost, new Vector3(startPos.x + (aOffset * size.x) - (size.x / 2), startPos.y - (size.y / 2), 0));
		}
	}

	void addRotation() {
		currentRotation += 1;
		if (currentRotation > 3) {
			currentRotation = 0;
		}
	}

	void removeRotation() {
		currentRotation -= 1;
		if (currentRotation < 0) {
			currentRotation = 3;
		}
	}

	void controllerOptions()
	{
		int cost = 0;
		if (currentObj < obj.Length) {
			if (obj [currentObj].GetComponent<BaseObject> ()) {
				cost = obj [currentObj].GetComponent<BaseObject> ().cost;
			}
		}
		if (GameObject.FindObjectOfType<Recycle> ().Money >= cost)
		{
			int temp = test(cursor.transform.position);
			if (temp != -1)
			{
				if (grid[temp].full == false)
				{
					if (currentObj == 0)
					{
						BuildBlock();
                     
                    }
					if (currentObj == 1)
					{
						BuildArmour ();
                      
                    }
					if (currentObj == 2) 
					{
						BuildTurret();
                       
                    }
				}
			}
			if (currentObj == 3)
			{
				if (temp != -1)
				{
					if (grid[temp].full == true)
					{
						DestroyBlock();
						GameObject.FindObjectOfType<Recycle>().Money += Mathf.CeilToInt(grid[temp].obj.GetComponent<BaseObject>().cost/2);
					}
				}
			}
		}
	}

	void changeObjectSelection(variableData data) {
		currentObj = data.i;
	}

	void BuildFakeBlock(GameObject _obj, bool _isSolid, bool _breakable, bool canPlaceOnGround, int needsRotation)
	{
		int temp = test(cursor.transform.position);
		if (temp != -1)
		{
			if (grid[temp].full == false)
			{
				createObj(temp, _obj, _isSolid, _breakable, canPlaceOnGround, needsRotation, false);
				GameObject.FindObjectOfType<checkWithinCamera>().testSections();
			}
		}
	}

	void BuildBlock()
	{
		BuildFakeBlock(obj[0], true, true, true, 0);
        audios.GetComponent<AudioStuff>().playSound(1);
    }

	void BuildArmour()
	{
		BuildFakeBlock(obj[1], true, true, true, 0);
        audios.GetComponent<AudioStuff>().playSound(1);
    }

	void BuildTurret()
	{
		BuildFakeBlock(obj[2], false, false, false, 1);
        audios.GetComponent<AudioStuff>().playSound(1);
    }

	void DestroyBlock()
	{
		int temp = test(cursor.transform.position);
		if (temp != -1)
		{
			if (grid[temp].full == true)
			{
				bool doesntHaveGroundAnySide = false;
				checkSidesData data = checkSides (temp);
				if (data.left) {
					grid [temp].check = false;
					if (!findGround (grid [data.leftIndex])) {
						doesntHaveGroundAnySide = true;
					}
				}
				if (data.right) {
					grid [temp].check = false;
					if (!findGround (grid [data.rightIndex])) {
						doesntHaveGroundAnySide = true;
					}
				}
				if (data.down) {
					grid [temp].check = false;
					if (!findGround (grid [data.downIndex])) {
						doesntHaveGroundAnySide = true;
					}
				}
				if (data.up) {
					grid [temp].check = false;
					if (!findGround (grid [data.upIndex])) {
						doesntHaveGroundAnySide = true;
					}
				}

				if (!doesntHaveGroundAnySide) {
					destroyObjsAbove (temp);
					GameObject.FindObjectOfType<checkWithinCamera> ().testSections ();
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		GetComponent<LineRenderer>().enabled = enableLineRenderer;
		if (Input.GetMouseButton(0))
		{
			int temp = test(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (temp != -1)
			{
				if (grid[temp].full == false)
				{
					createObj(temp, obj[0], true, true, true, 0, true);
                    
                    GameObject.FindObjectOfType<checkWithinCamera>().testSections();
                    audios.GetComponent<AudioStuff>().playSound(1, 0.8f, 0.04f);
                }
            }
		}
		if (Input.GetMouseButton(1))
		{
			int temp = test(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (temp != -1)
			{
				if (grid[temp].full == true)
				{
					destroyObjsAbove(temp);
					GameObject.FindObjectOfType<checkWithinCamera>().testSections();
				}
			}
		}
		if (Input.GetMouseButtonDown(2))
		{

			/*
            fillHole ();
            GameObject.FindObjectOfType<checkWithinCamera> ().testSections ();
			*/

			int temp = test(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (temp != -1)
			{
				if (grid[temp].full == false)
				{
					createObj(temp, obj[1], false, false, false, 1, true);
					GameObject.FindObjectOfType<checkWithinCamera>().testSections();
				}
			}

		}

		overlayScrap ();
	}

	void overlayScrap() {
		int temp = test(cursor.transform.position);
		if (temp != -1) {
			if ((grid [temp].full && currentObj == 3) ||
				(grid [temp].full != true && currentObj != 3)) {

				if (currentObj == 3) {
					overlay.GetComponent<SpriteRenderer> ().sprite = scrapPrefab.GetComponent<SpriteRenderer> ().sprite;
					overlay.SetActive (true);
				} else {
					overlay.GetComponent<SpriteRenderer> ().sprite = obj [currentObj].GetComponent<SpriteRenderer> ().sprite;
					if (currentObj != 2) {
						if (checkBlockPlaceAvailable (temp, true, true, 0, true)) {
							overlay.SetActive (true);
							overlay.GetComponent<SpriteRenderer> ().color = Color.green;
						} else {
							overlay.SetActive (false);
						}
					} else {
						if (checkBlockPlaceAvailable (temp, true, true, 1, false)) {
							overlay.SetActive (true);
							overlay.GetComponent<SpriteRenderer> ().color = Color.green;
						} else {
							overlay.SetActive (true);
							overlay.GetComponent<SpriteRenderer> ().color = Color.red;
						}
					}
				}

				Vector3 rot = transform.rotation.eulerAngles;
				int rotation = currentRotation;
				rot.z = rotation * 90;
				overlay.transform.rotation = Quaternion.Euler (rot);
				overlay.transform.position = new Vector3(grid[temp].xy.x, grid[temp].xy.y, 0);

				if (obj[currentObj].GetComponent<ObjectSpawningOffsets>())
				{
					//canBuildApon
					Vector3 offset = obj[currentObj].GetComponent<ObjectSpawningOffsets>().offset;
					float offsetTemp = 0;
					switch (currentRotation) {
					case 1:
						offsetTemp = offset.y;
						offset.y = -offset.x;
						offset.x = -offsetTemp;
						break;
					case 2:
						offset.y *= -1;
						break;
					case 3:
						offsetTemp = offset.y;
						offset.y = offset.x;
						offset.x = offsetTemp;
						break;
					}
					overlay.transform.position += offset;
				}
			} else {
				overlay.SetActive (false);
			}
		} else {
			overlay.SetActive (true);
		}
	}

	void findMaxHeight()
	{
		List<gridSection> floor = new List<gridSection>();
		for (int a = 0; a < gridSections.x; a++)
		{
			floor.Add(grid[a * gridSections.y]);
		}

		maxHeight = 0;

		if (floor.Count != 0)
		{
			maxHeight = findHighest(floor);
		}
	}

	void destroyObjsAbove(int _index)
	{

		checkSidesData temp = checkSides(_index);
		List<gridSection> tempSections = new List<gridSection>();
		int needTestHeight = 0;

		destroyObj(_index);

		if (temp.left)
		{
			if (grid[temp.leftIndex].full)
			{
				if (!findGround(grid[temp.leftIndex]))
				{
					List<gridSection> above = gatherSpaces(new findOptionsOpenClosedIncludeSolid(true), grid[temp.leftIndex]);
					if (above.Count >= maxDropBeforeDeath)
					{
						gameover.setScore(maxHeight);
						Debug.Log("Death");
					}
					for (int a = 0; a < above.Count; a++)
					{
						needTestHeight += destroyObj(above[a].index);
					}
				}
			}
		}


		if (temp.right)
		{
			if (grid[temp.rightIndex].full)
			{
				if (!findGround(grid[temp.rightIndex]))
				{
					List<gridSection> above = gatherSpaces(new findOptionsOpenClosedIncludeSolid(true), grid[temp.rightIndex]);
					if (above.Count >= maxDropBeforeDeath)
					{
						gameover.setScore(maxHeight);
						Debug.Log("Death");
					}
					for (int a = 0; a < above.Count; a++)
					{
						needTestHeight += destroyObj(above[a].index);
					}
				}
			}
		}


		if (temp.up)
		{
			if (grid[temp.upIndex].full)
			{
				if (!findGround(grid[temp.upIndex]))
				{
					List<gridSection> above = gatherSpaces(new findOptionsOpenClosedIncludeSolid(true), grid[temp.upIndex]);
					if (above.Count >= maxDropBeforeDeath)
					{
						gameover.setScore(maxHeight);
						Debug.Log("Death");
					}
					for (int a = 0; a < above.Count; a++)
					{
						needTestHeight += destroyObj(above[a].index);
					}
				}
			}
		}

		if (temp.down)
		{
			if (grid[temp.downIndex].full)
			{
				if (!findGround(grid[temp.downIndex]))
				{
					List<gridSection> above = gatherSpaces(new findOptionsOpenClosedIncludeSolid(true), grid[temp.downIndex]);
					if (above.Count >= maxDropBeforeDeath)
					{
						gameover.setScore(maxHeight);
						Debug.Log("Death");
					}
					for (int a = 0; a < above.Count; a++)
					{
						needTestHeight += destroyObj(above[a].index);
					}
				}
			}
		}

		if (needTestHeight > 0 || grid[_index].indexAB.y >= maxHeight)
		{
			findMaxHeight();
		}

	}

	bool checkBlockPlaceAvailable(int _index, bool testCase, bool solidTestCase, int needsRotation, bool canPlaceOnGround)
	{
		checkSidesData temp = checkSides(_index);

		if (temp.left)
		{
			if (grid[temp.leftIndex].full == testCase && grid[temp.leftIndex].isSolid == solidTestCase)
			{
				if (grid [temp.leftIndex].canBuildApon) {
					if (needsRotation == 0 || needsRotation == -1) {
						return true;
					} else {
						if (currentRotation == 3) {
							return true;
						}
					}
				}
			}
		}

		if (temp.right)
		{
			if (grid[temp.rightIndex].full == testCase && grid[temp.rightIndex].isSolid == solidTestCase)
			{
				if (grid [temp.rightIndex].canBuildApon) {
					if (needsRotation == 0 || needsRotation == -1) {
						return true;
					} else {
						if (currentRotation == 1) {
							return true;
						}
					}
				}
			}
		}

		if (temp.up)
		{
			if (grid[temp.upIndex].full == testCase && grid[temp.upIndex].isSolid == solidTestCase)
			{
				if (grid [temp.upIndex].canBuildApon) {
					if (needsRotation == 0 || needsRotation == -1) {
						return true;
					} else {
						if (currentRotation == 2) {
							return true;
						}
					}
				}
			}
		}

		if (temp.down)
		{
			if (grid[temp.downIndex].full == testCase && grid[temp.downIndex].isSolid == solidTestCase)
			{
				if (grid [temp.downIndex].canBuildApon) {
					if (needsRotation == 0 || needsRotation == -1) {
						return true;
					} else {
						if (currentRotation == 0) {
							return true;
						}
					}
				}
			}
		}

		if (canPlaceOnGround)
		{
			return grid[_index].indexAB.y == 0;
		}

		return false;
	}

	void createObj(int _index, GameObject _obj, bool _isSolid, bool _breakable, bool canPlaceOnGround, int needsRotation, bool canBuildApon, bool force = false) {
		if (checkBlockPlaceAvailable(_index, true, true, needsRotation, canPlaceOnGround) || force) {
			if (grid[_index].indexAB.y > maxHeight)
			{
				maxHeight = grid[_index].indexAB.y;
			}
			grid[_index].breakable = _breakable;
			grid[_index].isSolid = _isSolid;
			grid[_index].full = true;
			grid [_index].canBuildApon = canBuildApon;
			Vector3 rot = transform.rotation.eulerAngles;
			int rotation = currentRotation;
			if (needsRotation == 0) {
				rotation = Random.Range (0, 3);
				Debug.Log (rotation);
			}
			rot.z = rotation * 90;
			grid [_index].rotation = currentRotation;
			grid[_index].obj = GameObject.Instantiate(_obj, new Vector3(grid[_index].xy.x, grid[_index].xy.y, 0), Quaternion.Euler(rot));
			grid[_index].obj.GetComponent<SpriteRenderer>().enabled = false;
			if (grid[_index].obj.GetComponent<BaseObject>())
			{
				grid[_index].obj.GetComponent<BaseObject>().index = _index;
			}
			if (grid[_index].obj.GetComponent<ObjectSpawningOffsets>())
			{
				//canBuildApon
				Vector3 offset = grid[_index].obj.GetComponent<ObjectSpawningOffsets>().offset;
				float offsetTemp = 0;
				switch (currentRotation) {
				case 1:
					offsetTemp = offset.y;
					offset.y = -offset.x;
					offset.x = -offsetTemp;
					break;
				case 2:
					offset.y *= -1;
					break;
				case 3:
					offsetTemp = offset.y;
					offset.y = offset.x;
					offset.x = offsetTemp;
					break;
				}
				grid[_index].obj.transform.position += offset;
			}

			GameObject.FindObjectOfType<Recycle>().Money -= obj[currentObj].GetComponent<BaseObject>().cost;
		}
	}

	int destroyObj(int _index)
	{
		grid[_index].full = false;
		if (grid[_index].obj)
		{
			GameObject.Destroy(grid[_index].obj);
		}
		grid[_index].obj = null;
		if (grid[_index].indexAB.y >= maxHeight)
		{
			return 1;
		}
		return 0;
	}

	public int test(Vector2 pos)
	{

		int index = Mathf.FloorToInt(((pos.x + (size.x / 2)) - startPos.x) / size.x) * gridSections.y + Mathf.FloorToInt(((pos.y + (size.y / 2)) - startPos.y) / size.y);

		if (pos.x <= startPos.x - (size.x) ||
			pos.x >= startPos.x + ((gridSections.x - 1) * size.x + (size.x / 2)) ||
			pos.y <= startPos.y - (size.y) ||
			pos.y >= startPos.y + ((gridSections.y - 1) * size.y + (size.y / 2)) ||
			index < 0 || index >= grid.Count)
		{
			return -1;
		}

		return index;
	}

	public int testRaw(Vector2 pos)
	{

		int index = Mathf.FloorToInt((pos.x - startPos.x) / size.x) * gridSections.y + Mathf.FloorToInt((pos.y - startPos.y) / size.y);

		if (pos.x < startPos.x ||
			pos.x >= startPos.x + (gridSections.x * size.x) ||
			pos.y < startPos.y ||
			pos.y >= startPos.y + (gridSections.y * size.y) ||
			index < 0 || index >= grid.Count)
		{
			return -1;
		}

		return index;
	}

	List<gridSection> GetSegments(Vector2 minMax, string search = "")
	{
		List<gridSection> returnValue = new List<gridSection>();
		for (int a = 0; a <= minMax.y - minMax.x; a++)
		{
			for (int b = 0; b < gridSections.y; b++)
			{
				int index = ((int)minMax.x + a) + (b * (int)gridSections.x);
				if (index > 0 && index < grid.Count)
				{
					if (search == "")
					{
						returnValue.Add(grid[index]);
					}
					else
					{
						if (grid[index].obj)
						{
							if (grid[index].obj.GetComponent<BaseObject>())
							{
								if (grid[index].obj.GetComponent<BaseObject>().name == search)
								{
									returnValue.Add(grid[index]);
								}
							}
						}
					}
				}
			}
		}
		for (int a = 0; a < returnValue.Count; a++)
		{
			returnValue[a].full = true;
		}
		return returnValue;
	}

	public int GetTurrets(Vector2 minMax)
	{
		return GetSegments(minMax, "Turret").Count;
	}

	public int GetArmour(Vector2 minMax)
	{
		return GetSegments(minMax, "Armour").Count;
	}

	public int GetBase(Vector2 minMax)
	{
		return GetSegments(minMax, "Base").Count;
	}

	public float GetSegmentHeight()
	{
		return size.y;
	}

	public int GetMaxHeight()
	{
		return maxHeight;
	}

	void fillHole()
	{
		GameObject objToChangeTo = null;
		int temp = test(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		if (temp != -1)
		{
			objToChangeTo = grid[temp].obj;
			List<gridSection> sections = gatherSpaces(new findOptionsFill(objToChangeTo), grid[temp]);
			for (int a = 0; a < sections.Count; a++)
			{
				createObj(sections[a].index, obj[currentObj], true, true, true, 0, true);
			}
		}
	}

	//check each side of index
	checkSidesData checkSides(int _index, bool force = false)
	{
		checkSidesData val = new checkSidesData();

		//left
		if (grid[_index].indexAB.x > 0)
		{
			if (grid[_index - gridSections.y].check || force)
			{
				val.left = true;
				val.leftIndex = grid[_index].index - gridSections.y;
			}
		}

		//right
		if (grid[_index].indexAB.x < (gridSections.x - 1))
		{
			if (grid[_index + gridSections.y].check || force)
			{
				val.right = true;
				val.rightIndex = grid[_index].index + gridSections.y;
			}
		}

		//up
		if (grid[_index].indexAB.y < (gridSections.y - 1))
		{
			if (grid[_index + 1].check || force)
			{
				val.up = true;
				val.upIndex = grid[_index].index + 1;
			}
		}

		//down
		if (grid[_index].indexAB.y > 0)
		{
			if (grid[_index - 1].check || force)
			{
				val.down = true;
				val.downIndex = grid[_index].index - 1;
			}
		}

		return val;
	}

	//check how many sides of no-solid block exist
	int sidesOccupied(checkSidesData sides)
	{
		int returnVal = 0;
		if (sides.left)
		{
			if (grid[sides.leftIndex].isSolid && grid[sides.leftIndex].full)
			{
				returnVal++;
			}
		}
		if (sides.right)
		{
			if (grid[sides.rightIndex].isSolid && grid[sides.rightIndex].full)
			{
				returnVal++;
			}
		}
		if (sides.up)
		{
			if (grid[sides.upIndex].isSolid && grid[sides.upIndex].full)
			{
				returnVal++;
			}
		}
		if (sides.down)
		{
			if (grid[sides.downIndex].isSolid && grid[sides.downIndex].full)
			{
				returnVal++;
			}
		}
		return returnVal;
	}

	bool checkIndex(int _index, bool testCase)
	{
		//check grid spot open/full
		grid[_index].check = false;
		return (grid[_index].full == testCase);
	}

	bool checkIndexSolid(int _index, bool testCase)
	{
		//check grid spot open/full
		grid[_index].check = false;
		return (grid[_index].isSolid == testCase);
	}

	void checkIndexAdd(int _index, List<gridSection> queue, List<gridSection> checkd, bool testCase)
	{
		//check grid spot open/full and add to queue if true
		if (checkIndex(_index, testCase))
		{
			queue.Add(grid[_index]);
		}
		else
		{
			checkd.Add(grid[_index]);
		}
	}

	void checkIndexAdd(int _index, List<gridSection> queue, List<gridSection> checkd, bool testCase, bool solidTestCase)
	{
		//check grid spot open/full and add to queue if true
		if (checkIndex(_index, testCase))
		{
			if (checkIndexSolid(_index, solidTestCase))
			{
				queue.Add(grid[_index]);
				return;
			}
		}
		checkd.Add(grid[_index]);
	}

	//check if sides are full/empty
	void checkSidesLoop(List<gridSection> queue, List<gridSection> checkd, checkSidesData _data, bool testCase)
	{

		//left
		if (_data.left)
		{
			checkIndexAdd(_data.leftIndex, queue, checkd, testCase);
		}

		//right
		if (_data.right)
		{
			checkIndexAdd(_data.rightIndex, queue, checkd, testCase);
		}

		//up
		if (_data.up)
		{
			checkIndexAdd(_data.upIndex, queue, checkd, testCase);
		}

		//down
		if (_data.down)
		{
			checkIndexAdd(_data.downIndex, queue, checkd, testCase);
		}
	}

	//check if sides are full/empty and solid/not
	void checkSidesLoop(List<gridSection> queue, List<gridSection> checkd, checkSidesData _data, bool testCase, bool solidTestCase)
	{

		//left
		if (_data.left)
		{
			checkIndexAdd(_data.leftIndex, queue, checkd, testCase, solidTestCase);
		}

		//right
		if (_data.right)
		{
			checkIndexAdd(_data.rightIndex, queue, checkd, testCase, solidTestCase);
		}

		//up
		if (_data.up)
		{
			checkIndexAdd(_data.upIndex, queue, checkd, testCase, solidTestCase);
		}

		//down
		if (_data.down)
		{
			checkIndexAdd(_data.downIndex, queue, checkd, testCase, solidTestCase);
		}
	}

	//check if sides are full/empty and solid/not and object in right orientation
	void checkSidesLoop(List<gridSection> queue, List<gridSection> checkd, checkSidesData _data, bool testCase, bool solidTestCase, int rotation)
	{
		//left
		if (_data.left && rotation == 3)
		{
			if (grid [_data.leftIndex].isSolid) {
				checkIndexAdd (_data.leftIndex, queue, checkd, testCase, solidTestCase);
			}
		}

		//right
		if (_data.right && rotation == 1)
		{
			if (grid [_data.rightIndex].isSolid) {
				checkIndexAdd (_data.rightIndex, queue, checkd, testCase, solidTestCase);
			}
		}

		//up
		if (_data.up && rotation == 2)
		{
			if (grid [_data.upIndex].isSolid) {
				checkIndexAdd (_data.upIndex, queue, checkd, testCase, solidTestCase);
			}
		}

		//down
		if (_data.down && rotation == 0)
		{
			if (grid [_data.downIndex].isSolid) {
				checkIndexAdd (_data.downIndex, queue, checkd, testCase, solidTestCase);
			}
		}
	}

	bool checkIndexObject(int _index, GameObject _obj)
	{
		grid[_index].check = false;
		//check if obj exists
		if (_obj)
		{
			//check if grid has obj
			if (grid[_index].obj)
			{
				//return if obj is same as grid's
				return _obj.name == grid[_index].obj.name;
			}
		}
		//return if both objs are null
		return (_obj == null && grid[_index].obj == null);
	}

	void checkIndexObjectAdd(int _index, List<gridSection> queue, List<gridSection> checkd, GameObject testCase)
	{
		//check if object is the same as index and add if true
		if (checkIndexObject(_index, testCase))
		{
			queue.Add(grid[_index]);
		}
		else
		{
			checkd.Add(grid[_index]);
		}
	}

	//check if sides has obj similar to testCase
	void checkSidesLoop(List<gridSection> queue, List<gridSection> checkd, checkSidesData _data, GameObject testCase)
	{

		//left
		if (_data.left)
		{
			checkIndexObjectAdd(_data.leftIndex, queue, checkd, testCase);
		}

		//right
		if (_data.right)
		{
			checkIndexObjectAdd(_data.rightIndex, queue, checkd, testCase);
		}

		//up
		if (_data.up)
		{
			checkIndexObjectAdd(_data.upIndex, queue, checkd, testCase);
		}

		//down
		if (_data.down)
		{
			checkIndexObjectAdd(_data.downIndex, queue, checkd, testCase);
		}
	}

	void resetChecked(List<gridSection> reset)
	{
		for (int a = 0; a < reset.Count; a++)
		{
			reset[a].check = true;
		}
	}

	void finishedCheckingObject(List<gridSection> queue, List<gridSection> checkd)
	{
		queue[0].check = false;
		checkd.Add(queue[0]);
		queue.RemoveAt(0);
	}

	List<gridSection> gatherSpaces(findOptions option, gridSection startSection)
	{
		List<gridSection> temp = new List<gridSection>(1);
		temp.Add(startSection);
		return gatherSpaces(option, temp);
	}

	List<gridSection> gatherSpaces(findOptions option, List<gridSection> startSection)
	{
		List<gridSection> checkd = new List<gridSection>();
		List<gridSection> queue = new List<gridSection>();

		for (int a = 0; a < startSection.Count; a++)
		{
			queue.Add(startSection[a]);
		}

		List<gridSection> returnVal = new List<gridSection>();
		List<gridSection> turrets = new List<gridSection>();

		checkSidesData sideData = new checkSidesData();

		if (option.type == findOption.findOpenClosed)
		{
			bool testCase = ((findOptionsOpenClosed)option).open;
			while (queue.Count != 0)
			{
				sideData = checkSides(queue[0].index);
				checkSidesLoop(queue, checkd, sideData, testCase);
				returnVal.Add(queue[0]);
				finishedCheckingObject(queue, checkd);
			}
		}
		else if (option.type == findOption.findOpenClosedSolid)
		{
			bool testCase = ((findOptionsOpenClosed)option).open;
			bool testSolid = ((findOptionsOpenClosedSolid)option).isSolid;
			while (queue.Count != 0)
			{
				sideData = checkSides(queue[0].index);
				checkSidesLoop(queue, checkd, sideData, testCase, testSolid);
				returnVal.Add(queue[0]);
				finishedCheckingObject(queue, checkd);
			}
		}
		else if (option.type == findOption.findOpenClosedSolidIncludeSolidEdges)
		{
			bool testCase = ((findOptionsOpenClosedIncludeSolid)option).open;
			while (queue.Count != 0)
			{
				if (queue[0].isSolid)
				{
					sideData = checkSides(queue[0].index);
					checkSidesLoop(queue, checkd, sideData, testCase);
					returnVal.Add(queue[0]);
				}
				else
				{
					sideData = checkSides(queue[0].index, true);
					if (sidesOccupied(sideData) == 1)
					{
						returnVal.Add(queue[0]);
					}
					else
					{
						turrets.Add(queue[0]);
					}
				}
				finishedCheckingObject(queue, checkd);
			}
		}
		else if (option.type == findOption.findSimilar)
		{
			GameObject testObj = ((findOptionsFill)option).obj;
			while (queue.Count != 0)
			{
				sideData = checkSides(queue[0].index);
				checkSidesLoop(queue, checkd, sideData, testObj);
				returnVal.Add(queue[0]);
				finishedCheckingObject(queue, checkd);
			}
		}

		resetChecked(checkd);

		for (int a = 0; a < turrets.Count; a++)
		{
			if (!findGround(turrets[a]))
			{
				returnVal.Add(turrets[a]);
			}
		}

		return returnVal;

	}

	//ASDASJDLWAHF:OSAFH:OUSAHGFIUSAGF:IUSAGF:IAGSF:OUAHSF:OASHGF:IUSAHGF:IUSAGFFSA
	//**********)£($*)£"*%()£"*%()£% THIS NEEDS FIXING (£*$()£"*%$()£"*%************
	//ASDASJDLWAHF:OSAFH:OUSAHGFIUSAGF:IUSAGF:IAGSF:OUAHSF:OASHGF:IUSAHGF:IUSAGFFSA
	bool findGround(gridSection startSection)
	{
		List<gridSection> checkd = new List<gridSection>();
		List<gridSection> queue = new List<gridSection>();

		queue.Add(startSection);

		checkSidesData sideData = new checkSidesData();
		while (queue.Count != 0)
		{

			if (queue[0].indexAB.y == 0 && queue[0].isSolid)
			{
				resetChecked(checkd);
				resetChecked(queue);
				return true;
			}

			sideData = checkSides(queue[0].index);
			if (startSection.isSolid) {
				checkSidesLoop (queue, checkd, sideData, true, true);
			} else {
				checkSidesLoop(queue, checkd, sideData, true, true, queue[0].rotation);
			}
			finishedCheckingObject(queue, checkd);
		}

		resetChecked(checkd);

		return false;
	}

	int findHighest(List<gridSection> startSections)
	{
		List<gridSection> checkd = new List<gridSection>();
		List<gridSection> queue = new List<gridSection>();

		for (int a = 0; a < startSections.Count; a++)
		{
			if (startSections[a].full)
			{
				queue.Add(startSections[a]);
			}
		}

		int highestReached = 0;

		checkSidesData sideData = new checkSidesData();
		while (queue.Count != 0)
		{
			if (queue[0].indexAB.y >= highestReached)
			{
				highestReached = queue[0].indexAB.y;
			}
			sideData = checkSides(queue[0].index);
			checkSidesLoop(queue, checkd, sideData, true, true);
			finishedCheckingObject(queue, checkd);
		}

		resetChecked(checkd);

		return highestReached;
	}

	public void resetPosition(int _index)
	{
		destroyObjsAbove (_index);
		grid[_index].full = false;
		grid[_index].obj = null;
	}


}

[System.Serializable]
public class gridSection
{
	public Vector4 xy = Vector4.zero;
	public bool full = false;
	public bool isSolid = false;
	public bool breakable = false;
	public bool canBuildApon = false;
	public int rotation = 0;
	public GameObject obj = null;
	public int index;
	public Vector2I indexAB;
	public bool check = true;

	public gridSection(Vector4 _xy, int _index)
	{
		xy = _xy;
		index = _index;
	}

	public gridSection(Vector4 _xy, Vector3 _index)
	{
		xy = _xy;
		index = (int)_index.x;
		indexAB = new Vector2I(_index.y, _index.z);
	}
}

[System.Serializable]
public class checkSidesData
{
	public bool left = false, right = false, up = false, down = false;
	public int leftIndex, rightIndex, upIndex, downIndex;
}

public class findOptions
{
	public findOption type;

}

public class findOptionsOpenClosed : findOptions
{
	public bool open;

	public findOptionsOpenClosed(bool _open)
	{
		open = _open;
		type = findOption.findOpenClosed;
	}
}

public class findOptionsOpenClosedIncludeSolid : findOptions
{
	public bool open;

	public findOptionsOpenClosedIncludeSolid(bool _open)
	{
		open = _open;
		type = findOption.findOpenClosedSolidIncludeSolidEdges;
	}
}

public class findOptionsOpenClosedSolid : findOptionsOpenClosed
{
	public bool isSolid;

	public findOptionsOpenClosedSolid(bool _open, bool _isSolid) : base(_open)
	{
		isSolid = _isSolid;
		type = findOption.findOpenClosedSolid;
	}
}

public class findOptionsFill : findOptions
{
	public GameObject obj;

	public findOptionsFill(GameObject _obj)
	{
		obj = _obj;
		type = findOption.findSimilar;
	}
}
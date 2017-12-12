using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	
	public GridBuilding GridManager;
	public float speed = 5.0f;
	public float lowestPoint = 0;
	public GameObject cursor;
	public bool goToPos = false;
	public Vector3 aimPos;
	public float currentAimDist = 0;
	public Vector2 maxCursorVal;
	public float minDist = 0;
	public bool lerpVsSnap;
    
	// Use this for initialization
	void Start () {
		cursor = GameObject.FindObjectOfType<MoveCursor> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//to-do: lerp towards new position? mostly useful for the snapping
		//map controls in multi-input
		if (lerpVsSnap) {
			if (goToPos) {
				float yVal = Camera.main.transform.position.y - cursor.transform.position.y;
				if (Vector2.Distance (transform.position, aimPos) < (currentAimDist / 10)) {
					goToPos = false;
					transform.position = aimPos;
				} else {
					transform.position = Vector3.Lerp (transform.position, aimPos, Time.deltaTime * 2);
				}
				cursor.transform.position = new Vector3 (cursor.transform.position.x, Camera.main.transform.position.y - yVal, 0);
			}
		}

		speed = GridManager.GetMaxHeight () + 5;

		Vector3 tempPos = transform.position;
		if (transform.position.y - cursor.transform.position.y > maxCursorVal.x) {
			tempPos.y -= (transform.position.y - cursor.transform.position.y) - maxCursorVal.x;
		} else if (transform.position.y - cursor.transform.position.y < maxCursorVal.y) {
			tempPos.y -= (transform.position.y - cursor.transform.position.y) - maxCursorVal.y;
		}
			
		transform.position = checkPos(tempPos);

    }

	Vector3 checkPos(Vector3 pos) {
		if(pos.y < lowestPoint)
		{
			pos.y = lowestPoint;
		}
		if(pos.y > GridManager.GetMaxHeight() * GridManager.GetSegmentHeight())
		{
			pos.y = GridManager.GetMaxHeight() * GridManager.GetSegmentHeight();
		}
		return pos;
	}
	
	void TriggerInput(variableData _var)
	{
		float yVal = Camera.main.transform.position.y - cursor.transform.position.y;
		Vector3 tempPos = transform.position;
		
		tempPos.y += _var.state.TriggerRight.input * speed * Time.deltaTime;
		tempPos.y -= _var.state.TriggerLeft.input * speed * Time.deltaTime;
		
		transform.position = tempPos;

		if(tempPos.y < lowestPoint)
		{
			tempPos.y = lowestPoint;
		}
		if(tempPos.y > GridManager.GetMaxHeight() * GridManager.GetSegmentHeight())
		{
			tempPos.y = GridManager.GetMaxHeight() * GridManager.GetSegmentHeight();
		}

		transform.position = tempPos;

		cursor.transform.position = new Vector3 (cursor.transform.position.x, Camera.main.transform.position.y - yVal, 0);
    }

	void KeyboardInput(variableData _var)
	{
		float yVal = Camera.main.transform.position.y - cursor.transform.position.y;
		Vector3 tempPos = transform.position;

		tempPos.y += _var.f * speed * Time.deltaTime;

		transform.position = tempPos;

		if(tempPos.y < lowestPoint)
		{
			tempPos.y = lowestPoint;
		}
		if(tempPos.y > GridManager.GetMaxHeight() * GridManager.GetSegmentHeight())
		{
			tempPos.y = GridManager.GetMaxHeight() * GridManager.GetSegmentHeight();
		}

		transform.position = tempPos;

		cursor.transform.position = new Vector3 (cursor.transform.position.x, Camera.main.transform.position.y - yVal, 0);
	}
	
	//right bumper
	void SnapTop()
	{
		if (GridManager.GetMaxHeight () * GridManager.GetSegmentHeight () - transform.position.y > minDist) {
			float yVal = Camera.main.transform.position.y - cursor.transform.position.y;
			Vector3 tempPos = transform.position;
			tempPos.y = GridManager.GetMaxHeight () * GridManager.GetSegmentHeight ();
			if (lerpVsSnap) {
				currentAimDist = Vector3.Distance (transform.position, tempPos);
				aimPos = tempPos;
				goToPos = true;
			} else {
				transform.position = tempPos;
				cursor.transform.position = new Vector3 (cursor.transform.position.x, Camera.main.transform.position.y - yVal, 0);
			}
		}
	}
	
	//left bumper
	void SnapBottom()
	{
		if (transform.position.y > minDist) {
			float yVal = Camera.main.transform.position.y - cursor.transform.position.y;
			Vector3 tempPos = transform.position;
			tempPos.y = lowestPoint;
			if (lerpVsSnap) {
				currentAimDist = Vector3.Distance (transform.position, tempPos);
				aimPos = tempPos;
				goToPos = true;
			} else {
				transform.position = tempPos;
				cursor.transform.position = new Vector3 (cursor.transform.position.x, Camera.main.transform.position.y - yVal, 0);
			}
		}
	}
	
}
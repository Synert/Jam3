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
				if (Vector2.Distance (transform.position, aimPos) < 1) {
					goToPos = false;
				} else {
					float yVal = Camera.main.transform.position.y - cursor.transform.position.y;
					transform.position = Vector3.Lerp (transform.position, aimPos, Time.deltaTime * 2);
					cursor.transform.position = new Vector3 (cursor.transform.position.x, Camera.main.transform.position.y - yVal, 0);
				}
			}
		}

		speed = GridManager.GetMaxHeight () + 5;
		
		Vector3 tempPos = transform.position;

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
			yVal = 0;
		}
		if(tempPos.y > GridManager.GetMaxHeight() * GridManager.GetSegmentHeight())
		{
			tempPos.y = GridManager.GetMaxHeight() * GridManager.GetSegmentHeight();
			yVal = 0;
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
				aimPos = tempPos;
				goToPos = true;
			} else {
				transform.position = tempPos;
				cursor.transform.position = new Vector3 (cursor.transform.position.x, Camera.main.transform.position.y - yVal, 0);
			}
		}
	}
	
}
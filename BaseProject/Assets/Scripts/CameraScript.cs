using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	
	public GridBuilding GridManager;
	public float speed = 5.0f;
    public GameObject floor;
    public GameObject RecycleBin;
	public GameObject cursor;
    int n = 0;
    
	// Use this for initialization
	void Start () {
		cursor = GameObject.FindObjectOfType<MoveCursor> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//to-do: lerp towards new position? mostly useful for the snapping
		//map controls in multi-input

		speed = GridManager.GetMaxHeight () + 1;
		
		Vector3 tempPos = transform.position;
		
		if(tempPos.y < -5)
		{
			tempPos.y = -5;
		}
		if(tempPos.y > GridManager.GetMaxHeight() * GridManager.GetSegmentHeight())
		{
			tempPos.y = GridManager.GetMaxHeight() * GridManager.GetSegmentHeight();
		}

		//floor.transform.position = tempPos - new Vector3(0, Camera.main.orthographicSize, 0);
		transform.position = tempPos;
       if(n == 0)
        {
            //RecycleBin = (GameObject)Instantiate(RecycleBin, tempPos, Quaternion.identity);
            n++;
        }
       
     	//recycle bin code
		//RecycleBin.transform.position = tempPos - new Vector3(-GridManager.startPos.x + 4, Camera.main.orthographicSize-4, -10);
        transform.position = tempPos;
    }
	
	void TriggerInput(variableData _var)
	{
		float yVal = Camera.main.transform.position.y - cursor.transform.position.y;
		Vector3 tempPos = transform.position;
		
		tempPos.y += _var.state.TriggerRight.input * speed * Time.deltaTime;
		tempPos.y -= _var.state.TriggerLeft.input * speed * Time.deltaTime;
		
		transform.position = tempPos;
		cursor.transform.position = new Vector3 (cursor.transform.position.x, Camera.main.transform.position.y - yVal, 0);
    }
	
	//right bumper
	void SnapTop()
	{
		float yVal = Camera.main.transform.position.y - cursor.transform.position.y;
		Vector3 tempPos = transform.position;
		tempPos.y = GridManager.GetMaxHeight() * GridManager.GetSegmentHeight();
		transform.position = tempPos;
		cursor.transform.position = new Vector3 (cursor.transform.position.x, Camera.main.transform.position.y - yVal, 0);
	}
	
	//left bumper
	void SnapBottom()
	{
		float yVal = Camera.main.transform.position.y - cursor.transform.position.y;
		Vector3 tempPos = transform.position;
		tempPos.y = 0;
		transform.position = tempPos;
		cursor.transform.position = new Vector3 (cursor.transform.position.x, Camera.main.transform.position.y - yVal, 0);

	}
	
}
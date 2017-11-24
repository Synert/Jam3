using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	
	public GridBuilding GridManager;
	public float speed = 5.0f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//to-do: lerp towards new position? mostly useful for the snapping
		//map controls in multi-input

		speed = GridManager.GetMaxHeight ();
		
		Vector3 tempPos = transform.position;
		
		if(tempPos.y < 0)
		{
			tempPos.y = 0;
		}
		if(tempPos.y > GridManager.GetMaxHeight() * GridManager.GetSegmentHeight())
		{
			tempPos.y = GridManager.GetMaxHeight() * GridManager.GetSegmentHeight();
		}
		
		transform.position = tempPos;
		
	}
	
	void TriggerInput(variableData _var)
    {
		Vector3 tempPos = transform.position;
		
		tempPos.y += _var.state.TriggerRight.input * speed * Time.deltaTime;
		tempPos.y -= _var.state.TriggerLeft.input * speed * Time.deltaTime;
		
		transform.position = tempPos;
    }
	
	//right bumper
	void SnapTop()
	{
		Vector3 tempPos = transform.position;
		tempPos.y = GridManager.GetMaxHeight() * GridManager.GetSegmentHeight();
		transform.position = tempPos;
	}
	
	//left bumper
	void SnapBottom()
	{
		Vector3 tempPos = transform.position;
		tempPos.y = 0;
		transform.position = tempPos;
	}
	
}
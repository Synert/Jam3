using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	
	public GridBuilding GridManager;
	float r_trigger, l_trigger;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//to-do: lerp towards new position? mostly useful for the snapping
		//map controls in multi-input
		
		Vector3 tempPos = transform.position;
		
		if(tempPos.y < 0)
		{
			tempPos.y = 0;
		}
		if(tempPos.y > GridManager.GetMaxHeight() * GridManager.SegmentHeight())
		{
			tempPos.y = GridManager.GetMaxHeight() * GridManager.SegmentHeight();
		}
		
		transform.position = tempPos;
		
	}
	
	void TriggerInput(variableData _var)
    {
		Vector3 tempPos = transform.position;
		float speed = 5.0f;
		
        r_trigger = _var.state.TriggerRight.input;
		l_trigger = _var.state.TriggerLeft.input;
		
		tempPos.y += r_trigger * speed * Time.deltaTime;
		tempPos.y -= l_trigger * speed * Time.deltaTime;
		
		transform.position = tempPos;
    }
	
	//right bumper
	void SnapTop()
	{
		Vector3 tempPos = transform.position;
		tempPos.y = GridManager.GetMaxHeight() * GridManager.SegmentHeight();
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
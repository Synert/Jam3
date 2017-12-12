using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursor : MonoBehaviour {

	public List<Scrap> scrap = new List<Scrap>();
	public CameraScript cam;
	public GridBuilding grid;
	public float maxDistSide = 0;
	public float maxYDown = 0;
	public float speed = 5;

    void takeInputs(variableData data)
    {
		if (Time.timeScale == 1) {
			GridBuilding builder = GameObject.FindObjectOfType<GridBuilding> ();
			Vector3 transOffset = transform.position + (new Vector3 (data.state.ThumbStickLeft.inputs.x, data.state.ThumbStickLeft.inputs.y, 0) * speed);
			GameObject.FindObjectOfType<TestInput> ().controller = true;
			Debug.Log (data.state.ThumbStickLeft.inputs.x);
			if (transOffset == transform.position) {
				if (data.multiAxisValue.x != 0 || data.multiAxisValue.y != 0) {
					transOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					transOffset.z = 0;
					GameObject.FindObjectOfType<TestInput> ().controller = false;
				}
			}
			if (transOffset.x < builder.startPos.x - maxDistSide) {
				transOffset.x = builder.startPos.x - maxDistSide;
			}

			if (transOffset.x > builder.startPos.x + (builder.gridSections.x - 1) * builder.size.x + maxDistSide) {
				transOffset.x = builder.startPos.x + (builder.gridSections.x - 1) * builder.size.x + maxDistSide;
			}

			if (transOffset.y > cam.transform.position.y - cam.maxCursorVal.y + 1) {
				transOffset.y = cam.transform.position.y - cam.maxCursorVal.y + 1;
			}

			if (transOffset.y < -maxYDown) {
				transOffset.y = -maxYDown;
			}

			if (transOffset.y > builder.startPos.y + (builder.gridSections.y - 1) * builder.size.y) {
				transOffset.y = builder.startPos.y + (builder.gridSections.y - 1) * builder.size.y;
			}

			transform.position = transOffset;
		}
    }

	void selectScrap() {
		GetComponent<CircleCollider2D> ().enabled = true;
	}

	void deselectScrap() {
		GetComponent<CircleCollider2D> ().enabled = false;
		for (int a = 0; a < scrap.Count; a++) {
			if (scrap [a] != null) {
				scrap [a].currentlySelected = false;
				scrap [a].resetRigidbody ();
			}
		}
		scrap.Clear ();
	}
}

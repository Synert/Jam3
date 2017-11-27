using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursor : MonoBehaviour {

	public List<Scrap> scrap = new List<Scrap>();
	public float maxDistSide = 0;

    void takeInputs(variableData data)
    {
		GridBuilding builder = GameObject.FindObjectOfType<GridBuilding>();
		Vector3 transOffset = transform.position + (new Vector3(data.state.ThumbStickLeft.inputs.x, data.state.ThumbStickLeft.inputs.y, 0) / 5);
		if (transOffset.x < builder.startPos.x - maxDistSide) {
			transOffset.x = builder.startPos.x - maxDistSide;
		}

		if (transOffset.x > builder.startPos.x + (builder.gridSections.x - 1) * builder.size.x + maxDistSide) {
			transOffset.x = builder.startPos.x + (builder.gridSections.x - 1) * builder.size.x + maxDistSide;
		}

		if (transOffset.y < builder.startPos.y) {
			transOffset.y = builder.startPos.y;
		}

		if (transOffset.y > builder.startPos.y + (builder.gridSections.y - 1) * builder.size.y) {
			transOffset.y = builder.startPos.y + (builder.gridSections.y - 1) * builder.size.y;
		}

		transform.position = transOffset;
    }

	void selectScrap() {
		GetComponent<CircleCollider2D> ().enabled = true;
	}

	void deselectScrap() {
		GetComponent<CircleCollider2D> ().enabled = false;
		for (int a = 0; a < scrap.Count; a++) {
			scrap [a].currentlySelected = false;
			scrap [a].resetRigidbody ();
		}
		scrap.Clear ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{

    public bool selected = false;
	public bool currentlySelected = false;
	public Vector3 difference;
	public GameObject cursor;

    // Use this for initialization
    void Start()
    {
		cursor = GameObject.FindObjectOfType<MoveCursor> ().gameObject;
    }
    // Update is called once per frame
    void Update()
    {

		if (currentlySelected) {
			transform.position = cursor.transform.position + difference;
		}

    }
	private void OnTriggerStay2D(Collider2D col)
    {
		if (col.gameObject.tag == "Cursor") {
			if (!currentlySelected) {
				col.gameObject.GetComponent<MoveCursor> ().scrap.Add (this);
				currentlySelected = true;
				selected = true;
				difference = transform.position - col.gameObject.transform.position;
				GetComponent<Rigidbody2D> ().simulated = false;
			}
		}
    }

	public void resetRigidbody() {
		GetComponent<Rigidbody2D> ().simulated = true;
	}

    public bool GetScrapSelected()
    {
        return selected;
    }
    

}

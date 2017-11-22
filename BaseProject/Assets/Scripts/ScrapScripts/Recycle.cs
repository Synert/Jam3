using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycle : MonoBehaviour {

    GameObject m_stuff;
    bool select = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Work");
        if (col.gameObject.tag == "Scrap")
        {

            Scrap select = col.gameObject.GetComponent<Scrap>();
            if(select.selected == true)
            {
                Destroy(col.gameObject);
            }
            
        }
    }
}

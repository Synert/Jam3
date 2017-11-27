using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recycle : MonoBehaviour {

    GameObject m_stuff;
    public int Money = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		string temp = "Money: " + Money;
		temp += ":: Height ";
		temp += GameObject.FindObjectOfType<GridBuilding> ().maxHeight;
		GameObject.FindObjectOfType<Text> ().text = temp;
	}

	private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Scrap")
        {

            Scrap select = col.gameObject.GetComponent<Scrap>();
            if(select.selected == true)
            {
                Destroy(col.gameObject);
                Money++;
            }
            
        }
    }
}

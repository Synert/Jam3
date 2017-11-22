using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleBoxScript : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
       
        
	}
	
	// Update is called once per frame
	void Update () {
       
	}
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Scrap")
        {
            GameObject scraps = GameObject.Find("ScrapPrefab");
            Scrap scrapScript = scraps.GetComponent<Scrap>();
            Debug.Log("Collis");
            if (scrapScript.selected == true)
            {

                Destroy(col.gameObject);
            }


        }
    }
}

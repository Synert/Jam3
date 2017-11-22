using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapGenerator : MonoBehaviour {

    public GameObject scrapPrefab;
    public Rigidbody2D Rigidbody2D;
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upforce = 1.0f;
    bool dead = false;
    int Num = 0;
    int n;
    // Use this for initialization
    void Start()
    {
        dead = true;
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true && Num == 0)
        {
            CreateScrap();


            Destroy(gameObject);
        }
       
    }
    void CreateScrap()
    {
        GameObject scrap = (GameObject)Instantiate(scrapPrefab, transform.position, Quaternion.identity);
        scrap.GetComponent<Rigidbody2D>().AddForce(transform.up * power);
        n = Random.Range(0, 10);
        if(n > 5)
        {
            scrap.GetComponent<Rigidbody2D>().AddForce(transform.right * power);
        }
        else
        {
            scrap.GetComponent<Rigidbody2D>().AddForce(transform.right * -power);
        }
        

        Num = 1;
       
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public Transform[] clouds;
    public Transform bg1, bg2, camera;
    public int cloudCount = 20;

    SpriteRenderer bg1Sprite, bg2Sprite;

    // Use this for initialization
    void Start() {
        bg1Sprite = bg1.GetComponentInChildren<SpriteRenderer>();
        bg2Sprite = bg2.GetComponentInChildren<SpriteRenderer>();

        //place the clouds
        for (int i = 0; i < cloudCount; i++)
        {
            Transform new_cloud = Instantiate(clouds[Random.Range(0, clouds.Length)], bg1);
            new_cloud.position = new Vector3(Random.Range(-30.0f, 30.0f), 5.0f + i * (Random.Range(1.5f, 2.0f) + 1.0f));

            Transform new_cloud2 = Instantiate(clouds[Random.Range(0, clouds.Length)], bg2);
            new_cloud2.position = new Vector3(Random.Range(-30.0f, 30.0f), 5.0f + i * (Random.Range(1.5f, 2.0f) + 1.0f));
        }
	}
	
	// Update is called once per frame
	void Update () {
        //please don't change any of these numbers

        //this is the height of the background bits
        float x = 69.66f;

        //and then everything else i honestly have no idea at this point
        float dist = camera.position.y / x;
        int y = Mathf.FloorToInt(dist - 0.1f);

        float offset = 20.0f;

        if (Mathf.Abs(y) % 2 == 1)
        {
            //odd
            bg1.position = new Vector3(camera.position.x, (y + 1) * x + offset);
            bg2.position = new Vector3(camera.position.x, (y + 0) * x + offset);
        }
        else
        {
            bg1.position = new Vector3(camera.position.x, (y + 0) * x + offset);
            bg2.position = new Vector3(camera.position.x, (y + 1) * x + offset);
        }

        //a kinda hacky fix for the 'floor cloud' problem
        if(y == -1)
        {
            bg2.gameObject.SetActive(false);
        }
        else
        {
            bg2.gameObject.SetActive(true);
        }

        float col = Mathf.Max(dist, 0) / 3.0f;
        float div = 1.0f + col;

        bg1Sprite.color = new Color(1.0f / div, 1.0f / div, 1.0f / (div / 1.55f), 1.0f);
        bg2Sprite.color = new Color(1.0f / div, 1.0f / div, 1.0f / (div / 1.55f), 1.0f);
    }
}

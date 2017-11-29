using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
	public bool isFriendly = false;
    float lifetime = 5.0f;

    void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
		if (other.gameObject.tag == "Enemy") {
			DestroyObject (gameObject);
		}
			
    }
}

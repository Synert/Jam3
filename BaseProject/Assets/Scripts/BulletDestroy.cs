using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
	public bool isFriendly = false;
    void OnCollisionEnter2D(Collision2D other)
    {
		if (other.gameObject.tag == "Enemy") {
			DestroyObject (gameObject);
		}
			
    }
}

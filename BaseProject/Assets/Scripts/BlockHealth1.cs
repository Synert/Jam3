using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHealth1 : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
	public bool isFriendly = false;

    void Start()
    {

        curHealth = maxHealth;

    }
    void Update()
    {
        if (curHealth < 1)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bullet")
        {
			if (col.GetComponent<BulletDestroy> ().isFriendly != isFriendly) {
				curHealth -= 1;
				Destroy (col.gameObject);
			}
        }
    }
}
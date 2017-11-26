using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public float shootInterval;
    public float bulletSpeed = 100;
    public float bulletTimer;

    public bool awake = false;
    public bool lookingRight = true;

    public GameObject bullet;
    public GameObject target;

	public Transform rightSpawnLocation;
	public Transform leftSpawnLocation;

	public float viewDistance = 0;
	float prevViewDistance = 0;
	public Vector3 blockOffset;
	public float distanceToTarget;

    void Update()
    {
		if (prevViewDistance != viewDistance) {
			GetComponent<CircleCollider2D> ().radius = viewDistance;
			prevViewDistance = viewDistance;
		}
		if (target) {
			if (target.transform.position.x > transform.position.x) {
				lookingRight = true;
			}
			if (target.transform.position.x < transform.position.x) {
				lookingRight = false;
			}
		}
    }

	void OnTriggerStay2D(Collider2D col)
	{
		float currentDistanceToTarget = Vector2.Distance (col.transform.position, transform.position);
		RaycastHit2D hit = Physics2D.Raycast (transform.position, col.transform.position - transform.position, currentDistanceToTarget);
		if (hit.collider == col) {
			if (target) {
				if (col.CompareTag ("Enemy") && (currentDistanceToTarget < distanceToTarget || target == col.gameObject)) {
					target = col.gameObject;
					distanceToTarget = currentDistanceToTarget;
					if (col.transform.position.x < transform.position.x) {
						Attack (false);
					} else {
						Attack (true);
					}
				}
			} else {
				if (col.CompareTag ("Enemy")) {
					target = col.gameObject;
					distanceToTarget = currentDistanceToTarget;
					if (col.transform.position.x < transform.position.x) {
						Attack (false);
					} else {
						Attack (true);
					}
				}
			}
		}
	}

    public void Attack(bool attackingRight)
    {
        bulletTimer += Time.deltaTime;

        if (bulletTimer >= shootInterval)
        {
			Vector2 direction = Vector2.zero;
			if (attackingRight) {
				direction = target.transform.position - (transform.position + blockOffset);
			} else {
				direction = target.transform.position - (transform.position - blockOffset);
			}
            direction.Normalize();

            if(!attackingRight)
            {
                GameObject bulletClone;
				bulletClone = Instantiate(bullet, transform.position - blockOffset, transform.rotation) as GameObject;
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                bulletTimer = 0;
            }

            if(attackingRight)
            {
                GameObject bulletClone;
				bulletClone = Instantiate(bullet, transform.position + blockOffset, transform.rotation) as GameObject;
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                bulletTimer = 0;
            }
        }
    }
}

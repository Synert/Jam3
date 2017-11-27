using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public float shootInterval;
    public float bulletTimer;

    public GameObject target;

	public float viewDistance = 0;
	float prevViewDistance = 0;
	public float distanceToTarget;

	LineRenderer lr;
	SpriteRenderer sr;
	public float shotDisplay = 0;
	public float currentShotDisplay = 0;

	void Start() {
		lr = GetComponent<LineRenderer> ();
		sr = GetComponent<SpriteRenderer> ();
	}

    void Update()
    {
		if (prevViewDistance != viewDistance) {
			GetComponent<CircleCollider2D> ().radius = viewDistance;
			prevViewDistance = viewDistance;
		}

		if (currentShotDisplay >= 0) {
			currentShotDisplay -= Time.deltaTime;
		} else {
			lr.enabled = false;
		}

		if (target == null) {
			bulletTimer = 0;
		}
    }

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.tag == "Enemy") {
			float currentDistanceToTarget = Vector2.Distance (col.transform.position, transform.position);
			RaycastHit2D hit = Physics2D.Raycast (transform.position, col.transform.position - transform.position);
			if (hit.collider == col) {
				if (target) {
					if ((currentDistanceToTarget < distanceToTarget || target == col.gameObject)) {
						target = col.gameObject;
						distanceToTarget = currentDistanceToTarget;
						if (col.transform.position.x < transform.position.x) {
							Attack (false);
						} else {
							Attack (true);
						}
					}
				} else {
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
			if (sr.enabled) {
				lr.enabled = true;
				currentShotDisplay = shotDisplay;
				lr.positionCount = 2;
				lr.SetPosition (0, transform.position);
				lr.SetPosition (1, target.transform.position);
			}
			if (target) {
				target.GetComponent<enemyBehaviour> ().takeDamage (1);
			}

			bulletTimer = 0;
        }
    }
}
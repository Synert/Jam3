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
    public GameObject audios;

	void Start() {
		lr = GetComponent<LineRenderer> ();
		sr = GetComponent<SpriteRenderer> ();
        audios = GameObject.Find("AudioObject");
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
            if (audios)
            {
                audios.GetComponent<AudioStuff>().playSound(3, 1.5f, 0.05f);
            }
            if (sr.enabled) {
				lr.enabled = true;
				currentShotDisplay = shotDisplay;
				lr.positionCount = 2;
                Vector3 temp = transform.position;
                temp.z = 0;
                Vector3 temp2 = target.transform.position;
                temp2.z = 0;
				lr.SetPosition (0, temp);
				lr.SetPosition (1, temp2);
			}
			if (target) {
				target.GetComponent<enemyBehaviour> ().takeDamage (1);
			}

			bulletTimer = 0;
        }
    }
}
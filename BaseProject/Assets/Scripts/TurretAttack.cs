using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    public TurretAI turretAI;

    public bool isLeft = false;

    void Awake()
    {
        turretAI = gameObject.GetComponentInParent<TurretAI>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
		if (turretAI.target) {
			float currentDistanceToTarget = Vector2.Distance (col.transform.position, transform.position);
			if (col.CompareTag ("Enemy") && (currentDistanceToTarget < turretAI.distanceToTarget || turretAI.target == col.gameObject)) {
				turretAI.target = col.gameObject;
				turretAI.distanceToTarget = currentDistanceToTarget;
				if (isLeft) {
					turretAI.Attack (false);
				} else {
					turretAI.Attack (true);
				}
			}
		} else {
			if(col.CompareTag("Enemy"))
			{
				turretAI.target = col.gameObject;
				turretAI.distanceToTarget = Vector2.Distance (col.transform.position, transform.position);
				if(isLeft)
				{
					turretAI.Attack(false);
				}
				else
				{
					turretAI.Attack(true);
				}
			}
		}

    }
}

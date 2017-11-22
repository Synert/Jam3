using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehaviour : MonoBehaviour
{
    Transform myGun;
    Transform targetTransform;
    Vector2 m_destination;

    int m_direction = -1; 
    int towerMask = 1 << 8;

    bool targetAcquired = false;
    float speed = 2.0f;
    float step = 0.0f;

    // Use this for initialization
    void Start ()
    {  
        foreach (Transform child in transform)
        {
            if (child.name == "Gun")
            {
                myGun = child;
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(targetAcquired == false)
        {
            getTarget();
        }
       
        Movement();
    }

    void getTarget()
    {
        Debug.DrawLine(myGun.transform.position, myGun.transform.position + (myGun.transform.up * -m_direction));

        if (targetAcquired == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(myGun.transform.position, myGun.transform.up * -m_direction, 20.0f, towerMask);
            if (hit == true)
            {
                Debug.Log("targetAcquired");
                targetAcquired = true;
                targetTransform = hit.transform;
            }
        }
    }

    void Movement()
    {
        if (targetAcquired)
        {
            RaycastHit2D hit = Physics2D.Raycast(myGun.transform.position, myGun.transform.up * -m_direction, 3.0f, towerMask);
            if(hit == false)
            {
                if (!(targetTransform.position.y == myGun.transform.position.y))
                {
                    step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, step);
                }
            }

            if (hit == true)
            {
                if (!(targetTransform.position.y == myGun.transform.position.y))
                {
                    step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x, targetTransform.position.y), step);
                }
            }
            
            if(hit == true && Mathf.Abs(myGun.transform.position.y - targetTransform.position.y) < 0.75)
            {
                Debug.Log("Shooting");
                shoot();
            }

        } 
       
        if(targetAcquired == false)
        {
            
            step = speed * Time.deltaTime;
            m_destination = new Vector2(transform.position.x + (20 * m_direction), transform.position.y);
            transform.position = Vector3.MoveTowards(transform.position, m_destination, step);
        }
    }

    void shoot()
    {
        Debug.Log("Bang");
    }

    public void SetDirection(int direction)
    {
        m_direction = direction;
    }

    public int GetDirection()
    {
        return m_direction;
    }
}

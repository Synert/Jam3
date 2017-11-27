using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehaviour : MonoBehaviour
{

    public GameObject bulletPrefab;
    public int m_direction = -1;
	
	EnemyManager m_manager;
	int m_seg;

    Transform myGun;
    Transform targetTransform;
    Vector2 m_destination;
    Vector3 theScale;
 
    int towerMask = 1 << 8;
    int enemyMask = 1 << 9;
    public int health = 3;

    bool isAlive = true;
    bool targetAcquired = false;
    float speed = 2.0f;
    float step = 0.0f;

    float bulletSpeed = 5.0f;
    float counter = 1;

    bool friendlyAbove = false;
    bool friendlyBelow = false;

    bool firstPass = true;

    public int maxHealth;
    bool isFriendly = false;

	/*
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bullet")
        {
            if (col.GetComponent<BulletDestroy>().isFriendly != isFriendly)
            {
                health -= 1;
                Destroy(col.gameObject);
                takeDamage();
            }
        }
    }
    */

    // Use this for initialization
    void Start ()
    {
        health = maxHealth;
        foreach (Transform child in transform)
        {
            if (child.name == "Gun")
            {
                myGun = child;
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            transform.localScale = new Vector3(-m_direction, transform.localScale.y, transform.localScale.z);

            if (targetAcquired == false)
            {
                getTarget();
            }

            Movement();
        }
    }

    void getTarget()
    {
        Debug.DrawLine(myGun.transform.position, myGun.transform.position + (myGun.transform.up * 20.0f));

        if (targetAcquired == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(myGun.transform.position, myGun.transform.up, 20.0f, towerMask);
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
        counter -= Time.deltaTime;

        if(targetAcquired && targetTransform == null)
        {
            targetAcquired = false;
        }

        if (targetAcquired)
        {
            RaycastHit2D hit = Physics2D.Raycast(myGun.transform.position, myGun.transform.up, 5.0f, towerMask);
    
            RaycastHit2D hitAbove = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 3f), transform.up, 3.0f, enemyMask);
            RaycastHit2D hitBelow = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 3f), transform.up * -1, 3.0f, enemyMask);
            RaycastHit2D hitFront = Physics2D.Raycast(new Vector2(transform.position.x + 4f * m_direction, transform.position.y), transform.right * m_direction, 1.0f, enemyMask);

            Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + 3f), transform.position + (transform.up * 5));
            Debug.DrawLine(new Vector2(transform.position.x, transform.position.y - 3f), transform.position + (transform.up * -5));
            Debug.DrawLine(new Vector2(transform.position.x + 4f * m_direction, transform.position.y), transform.position + (transform.right * m_direction * 2.0f));


            if (hitAbove == true)
            {
                step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y - 0.5f), step);
            }

            if(hitBelow == true)
            {
                step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 0.5f), step);
            }

            if(hitFront == true)
            {
                step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 0.5f), step);
            }


            if (hit == false)
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
            
            if(hit == true)
            {
                Debug.Log("Shooting");
                if (counter <= 0)
                {
                    shoot();
                    counter = 1;
                }
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
        var bullet = (GameObject)Instantiate(bulletPrefab, myGun.position, myGun.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f * m_direction, 0);
        Destroy(bullet, 2.0f);
    }

    public void SetDirection(int direction)
    {
        m_direction = direction;
    }

    public int GetDirection()
    {
        return m_direction;
    }

	public void takeDamage(int damage)
    {
        //Damage recieved from turret removes HP
		health -= damage;

        if (health <= 0 && isAlive)
        {
            isAlive = false;
			GetComponent<ScrapGenerator> ().CreateScrap ();
            Death();
        }
    }

    void Death()
    {
        //Instantiate(ScrapType1, transform.position, transform.rotation);
        //Instantiate(ScrapType2, transform.position, transform.rotation);
        //Instantiate(ScrapType3, transform.position, transform.rotation);

		if (m_manager) {
			m_manager.RegisterDeath (m_seg);
		}
		
        Destroy(gameObject);
    }
	
	public void SetManager(EnemyManager set)
	{
		m_manager = set;
	}
	
	public void SetSegment(int set)
	{
		m_seg = set;
	}
}

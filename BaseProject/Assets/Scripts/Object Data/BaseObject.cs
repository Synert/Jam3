using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {

	public string name;
	public int hp;
	public int index;
	public GridBuilding building;
	public bool show = false;
    public bool isFriendly = true;
    public float buildTime = 1.0f;
    public bool build = true;

	void Start() {
		building = GameObject.FindObjectOfType<GridBuilding> ();
	}

    void Update()
    {
        if(build)
        {
            buildTime -= Time.deltaTime;
            if(buildTime <= 0.0f)
            {
                build = false;
                //building.setPositionSolid(index);
            }
        }
    }

    public void takeDamage(int damage) {
		hp -= damage;
		testHealth ();
	}

	void testHealth() {
		if (hp <= 0) {
			building.resetPosition (index);
			GameObject.Destroy (this.gameObject);
		}
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bullet")
        {
            if (col.GetComponent<BulletDestroy>().isFriendly != isFriendly)
            {
                takeDamage(1);
                Destroy(col.gameObject);
            }
        }
    }
}
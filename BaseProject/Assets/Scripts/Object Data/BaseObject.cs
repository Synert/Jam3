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
	public int cost = 0;
    public bool build = true;

    public LineRenderer m_line;
    Vector3 bottom;
    float initBuild;
    public GameObject audios;

    bool doOnce = false;

	void Start() {
        audios = GameObject.Find("AudioObject");
		building = GameObject.FindObjectOfType<GridBuilding> ();

        //m_line = gameObject.AddComponent<LineRenderer>();
        m_line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if(!doOnce)
        {
            bottom = transform.position;
            bottom.y -= (transform.lossyScale.y) * 4.7f;
            bottom.z = -5.0f;

            m_line.SetPosition(0, bottom);
            m_line.SetPosition(1, bottom);
            initBuild = buildTime;

            m_line.positionCount = 2;
            m_line.SetColors(Color.red, Color.red);
            m_line.SetWidth(3.0f, 3.0f);

            doOnce = true;
        }

        if(build)
        {
            buildTime -= Time.deltaTime;
            if(buildTime <= 0.0f)
            {
                build = false;
				building.grid[index].canBuildApon = true;
                m_line.enabled = false;
                m_line.SetWidth(0.25f, 0.25f);
                m_line.SetColors(Color.yellow, Color.red);
            }
            else
            {
                float percentage = buildTime / initBuild;
                Debug.Log(percentage);
                m_line.SetPosition(1, new Vector3(bottom.x, bottom.y + percentage * 3.0f, -5.0f));
                Color newCol = new Color(percentage, 1.0f - percentage, 0.0f, 0.7f);
                m_line.SetColors(newCol, newCol);
            }
        }
    }

    public void takeDamage(int damage) {
		hp -= damage;
		testHealth ();
	}

	void testHealth() {
		if (hp <= 0) {
            audios.GetComponent<AudioStuff>().playSound(4);
			building.resetPosition (index);
		}
	}

    void OnTriggerEnter2D(Collider2D col)
    {
		if (name != "Turret") {
			if (col.tag == "Bullet") {
				if (col.GetComponent<BulletDestroy> ().isFriendly != isFriendly) {
					takeDamage (1);
					Destroy (col.gameObject);
				}
			}
		}
    }
}
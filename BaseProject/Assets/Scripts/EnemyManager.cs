using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public GridBuilding GridManager;
	public GameObject[] enemyList;
    public float maxMult = 1.5f;
    public float minMult = 0.75f;
    public float turretScale = 1.25f;
    public float armourScale = 1.0f;
	public float breather = 7.5f;
	public float breatherMax = 10.0f;
	public int segSize = 10;
	public int baseEnemies = 2;
    public float iHateYourMagicNumbersSoHeresOneOfMine = 0;

    int[] enemiesAlive;
	int[] currentWave;
	bool[] waveEnded;
	float[] breatherTime;

	// Use this for initialization
	void Start () {
		enemiesAlive = new int[512];
		currentWave = new int[512];
		waveEnded = new bool[512];
		breatherTime = new float[512];
		
		for(int i = 0; i < 512; i++)
		{
			enemiesAlive[i] = 0;
			waveEnded[i] = true;
			currentWave[i] = 0;
			breatherTime[i] = 1.0f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < GridManager.GetMaxHeight(); i+=segSize)
		{
			if(enemiesAlive[i] <= 0)
			{
				if (!waveEnded[i])
				{
					//wave has just ended
					waveEnded[i] = true;
					breatherTime[i] = Mathf.Min(breatherMax, breather + currentWave[i] * 0.75f);
					currentWave[i]++;
				}
				else if (breatherTime[i] >= 0.0f)
				{
					breatherTime[i] -= Time.deltaTime;
					if (breatherTime[i] <= 0.0f)
					{
						//new wave
						SpawnEnemies(i);
						waveEnded[i] = false;
					}
				}
			}
		}
	}

	void SpawnEnemies(int seg)
	{
		float turrets = GridManager.GetTurrets(new Vector2(seg, seg + segSize));
		float armour = GridManager.GetArmour(new Vector2(seg, seg + segSize));

        turrets *= turretScale;
        armour *= armourScale;

        //number of segments between here and the top
		int heightDiff = Mathf.FloorToInt(GridManager.GetMaxHeight() / segSize) - seg;

        //number of segments between here and the top
		float heightScalar = maxMult + (float)heightDiff / 10.0f;
			
		if(heightScalar < minMult)
		{
			heightScalar = minMult;
		}

		float enemies = baseEnemies + turrets + armour;
		enemies *= heightScalar;

        float segHeight = GridManager.GetSegmentHeight();
        //segHeight /= 10.0f;

        Vector3 origin = GridManager.startPos;

		for(int i = 0; i < (int)enemies; i++)
		{
			int randomEnemy = Random.Range(0, enemyList.Length);
			Transform newEnemy = Instantiate(enemyList[randomEnemy]).transform;
				
			int direction = 1 - (Random.Range(0, 2) * 2);
				
			newEnemy.GetComponent<enemyBehaviour>().SetManager(this);
			newEnemy.GetComponent<enemyBehaviour>().SetSegment(seg);
			newEnemy.GetComponent<enemyBehaviour>().SetDirection(-direction);

            float maxHeight = GridManager.GetMaxHeight() * segHeight;

            float newY = seg * segHeight + Random.Range(0.0f, segHeight * segSize);
            newY = Mathf.Min(newY, maxHeight);

            newEnemy.transform.position = new Vector3(0.0f, newY) + origin;
            newEnemy.transform.position = new Vector3(((45.0f + i * 6) - iHateYourMagicNumbersSoHeresOneOfMine) * direction, newEnemy.transform.position.y);

            enemiesAlive[seg]++;
		}
	}

	public void RegisterDeath(int segment)
	{
		enemiesAlive[segment]--;
        Debug.Log(segment + " has died. Total left: " + enemiesAlive[segment]);
	}
}

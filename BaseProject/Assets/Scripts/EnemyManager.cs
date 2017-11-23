using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public GridBuilding GridManager;
	public GameObject[] enemyList;
	public float maxMult, minMult, turretScale, armourScale;
	public int segSize = 10;
	public int baseEnemies = 2;

	int enemiesAlive;
	int currentWave;
	bool waveEnded;
	float breatherTime;

	// Use this for initialization
	void Start () {
		enemiesAlive = 0;
		waveEnded = true;
		currentWave = 1;
		breatherTime = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(enemiesAlive <= 0)
		{
			if (!waveEnded)
			{
				//wave has just ended
				waveEnded = true;
				breatherTime = 5 + 2.5f * currentWave;
				currentWave++;
			}
			else if (breatherTime >= 0.0f)
			{
				breatherTime -= Time.deltaTime;
				if (breatherTime <= 0.0f)
				{
					//new wave
					SpawnEnemies();
					waveEnded = false;
				}
			}
		}
	}

	void SpawnEnemies()
	{
		for(int i = 0; i < GridManager.GetMaxHeight(); i+=segSize)
		{
			int turrets = GridManager.GetTurrets(new Vector2(i * segSize, (i * segSize) + segSize));
			int armour = GridManager.GetArmour(new Vector2(i * segSize, (i * segSize) + segSize));
			int heightDiff = GridManager.GetMaxHeight() - i;
			float heightScalar = maxMult - heightDiff / 10.0f;
			
			if(heightScalar < minMult)
			{
				heightScalar = minMult;
			}

			float enemies = baseEnemies + turrets * turretScale + armour * armourScale;
			enemies *= heightScalar;

			float segHeight = GridManager.GetSegmentHeight() * segSize;

			for(int j = 0; j < (int)enemies; j++)
			{
				int randomEnemy = Random.Range(0, enemyList.Length);
				Transform newEnemy = Instantiate(enemyList[randomEnemy]);
				
				int direction = 1 - Random.Range(0, 1) * 2;
				
				newEnemy.GetComponent<enemyBehaviour>().SetManager(this);
				newEnemy.GetComponent<enemyBehaviour>().SetDirection(direction);
				
				newEnemy.transform.position = new Vector3(50.0f * direction, i * seg + Random.Range(0.0f, segHeight));
				
				enemiesAlive++;
			}

		}
	}

	public void RegisterDeath()
	{
		enemiesAlive--;
	}
}

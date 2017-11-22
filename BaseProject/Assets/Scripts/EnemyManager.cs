using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	//none of this exists yet, so it won't compile-
	//should be fine once the other stuff's added
	//public GridBuilding GridManager;
	public GameObject[] enemyList;
	public float maxMult, minMult;

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
					//SpawnEnemies();
					waveEnded = false;
				}
			}
		}
	}

	//void SpawnEnemies()
	//{
	//	for(int i = 0; i < GridManager.GetSegments(); i++)
	//	{
	//		int turrets = GridManager.GetTurrets(i);
	//		int armour = GridManager.GetArmour(i);
	//		int heightDiff = GridManager.GetSegments() - i;
	//		float heightScalar = maxMult - heightDiff / 10.0f;
	//		if(heightScalar < minMult)
	//		{
	//			heightScalar = minMult;
	//		}

	//		float enemies = 2 + turrets * 1.5f + armour * 1.0f;
	//		enemies *= heightScalar;

	//		float segmentHeight = GridManager.GetSegmentHeight();

	//		for(int j = 0; j < (int)enemies; j++)
	//		{
	//			int randomEnemy = Random.Range(0, enemyList.Length);
	//			Transform newEnemy = Instantiate(enemyList[randomEnemy]);

	//			newEnemy.GetComponent<Enemy>().SetManager(this);
	//			newEnemy.transform.position = new Vector3(-50.0f + Random.Range(0, 1) * 100.0f, segment * segmentHeight + Random.Range(0.0f, segmentHeight));
	//			enemiesAlive++;
	//		}

	//	}
	//}

	public void RegisterDeath()
	{
		enemiesAlive--;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public Transform[] enemySpawner;

    private float Timer;
    private float timeToSpawn;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Timer += Time.deltaTime;
        timeToSpawn += Time.deltaTime;

		if (Timer < 30f)
        {
            if (timeToSpawn >= 8f)
            {
                SpawnEnemy();
                timeToSpawn = 0f;
            }
        }

        if (Timer > 30f)
        {
            if (timeToSpawn >= 5f)
            {
                SpawnEnemy();
                timeToSpawn = 0f;
            }
        }
	}

    void SpawnEnemy()
    {
        Instantiate(enemy, enemySpawner[Random.Range(0, 3)].transform.position, transform.rotation);
    }
}

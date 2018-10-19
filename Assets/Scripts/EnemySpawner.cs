using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemy;
    public Transform[] enemySpawner;

    private float Timer;
    private float timeToSpawn;
    private float bigSpawn;

    public Text timerText;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Timer += Time.deltaTime;
        timeToSpawn += Time.deltaTime;

        timerText.text = Mathf.Round(Timer).ToString();

        if(Timer >= 45f)
        {
            bigSpawn += Time.deltaTime;
        }

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

        if (Timer > 45f)
        {
            if(timeToSpawn >= 3f)
            {
                SpawnEnemy();
                timeToSpawn = 0f;
            }

            if (bigSpawn >= 8f)
            {
                SpawnBigEnemy();
                bigSpawn = 0f;
            }
        }
	}

    void SpawnEnemy()
    {
        Instantiate(enemy[0], enemySpawner[Random.Range(0, 3)].transform.position, transform.rotation);
    }

    void SpawnBigEnemy()
    {
        Instantiate(enemy[1], enemySpawner[Random.Range(0, 3)].transform.position, transform.rotation);
    }
}

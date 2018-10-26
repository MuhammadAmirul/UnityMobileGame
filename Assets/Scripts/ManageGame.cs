using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageGame : MonoBehaviour
{
    public GameObject[] enemy;
    public Transform[] enemySpawner;

    private float Timer;
    private float timeToSpawn;
    private float bigSpawn;

    public int addEnemies;
    public static int killCount;

    public Text timerText;
    public Text enemies;
    public Text killCountText;

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
        enemies.text = addEnemies.ToString();
        killCountText.text = killCount.ToString();

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
                addEnemies += 1;
            }
        }

        if (Timer > 30f)
        {
            if (timeToSpawn >= 5f)
            {
                SpawnEnemy();
                timeToSpawn = 0f;
                addEnemies += 1;
            }
        }

        if (Timer > 45f)
        {
            if(timeToSpawn >= 3f)
            {
                SpawnEnemy();
                timeToSpawn = 0f;
                addEnemies += 1;
            }

            if (bigSpawn >= 8f)
            {
                SpawnBigEnemy();
                bigSpawn = 0f;
                addEnemies += 1;
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

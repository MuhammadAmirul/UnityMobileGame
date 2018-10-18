using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGame : MonoBehaviour {

    [Header("Game Related Vars")]
    private int score;
    private bool isSpawning = true;
    private float timeSpawn = 5.0f;

    [Space]
    [Header("Enemy Related Vars")]
    public EnemyControl prefabEnemy;
    public Transform[] enemySpawnPoints;
    private List<EnemyControl> listGarbageEnemies;


    [Space]
    [Header("UI Related Vars")]
    public Canvas canvasMenus;
    public Canvas canvas3D;


    private void Awake()
    {
        listGarbageEnemies = new List<EnemyControl>();
    }

    private void Start()
    {

        if (isSpawning)
        {
            InvokeRepeating("SpawnEnemy", timeSpawn, timeSpawn);
        }
    }

    public void SpawnEnemy()
    {
        EnemyControl newEnemy = CheckForAvailableGarbageEnemy();

        Transform spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];

        if(newEnemy == null)
        {
            //Instantiate.
            newEnemy = Instantiate(prefabEnemy, spawnPoint.position, spawnPoint.rotation);

            listGarbageEnemies.Add(newEnemy);
        }
        else
        {
            newEnemy.transform.position = spawnPoint.position;
        }

        newEnemy.gameObject.SetActive(true);
        newEnemy.Init();
    }

    private EnemyControl CheckForAvailableGarbageEnemy()
    {
        EnemyControl returnEnemy = null;

        //Go through list and return any inactive spawned enemy.
        if(listGarbageEnemies.Count > 0)
        {
            foreach(EnemyControl ec in listGarbageEnemies)
            {
                if (!ec.isActiveAndEnabled)
                {
                    returnEnemy = ec;
                }
            }
        }

        return returnEnemy;
    }
}

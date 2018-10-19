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
    [Header("Player/Enemy Related Vars")]
    public PlayerControl player;
    public UnitStatsUI prefabEnemyStatUI;
    public EnemyControl prefabEnemy;
    public Transform[] enemySpawnPoints;
    private List<EnemyControl> listGarbageEnemies;
    private List<UnitStatsUI> listGarbageEnemyUIs;


    [Space]
    [Header("UI Related Vars")]
    public Canvas canvasMenus;
    public Canvas canvas3D;


    private void Awake()
    {
        listGarbageEnemies = new List<EnemyControl>();
        listGarbageEnemyUIs = new List<UnitStatsUI>();
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
        UnitStatsUI newEnemyUI = CheckForAvailableGarbageEnemyUI();

        Transform spawnPoint = FindFurthestSpawnPoint();

        if(newEnemy == null)
        {
            //Instantiate.
            newEnemy = Instantiate(prefabEnemy, spawnPoint.position, spawnPoint.rotation);

            newEnemyUI = Instantiate(prefabEnemyStatUI, Vector3.zero, Quaternion.identity);
            newEnemyUI.transform.SetParent(prefabEnemyStatUI.transform.parent, true);
            newEnemyUI.transform.localScale = Vector3.one;
            RectTransform rtUI = newEnemyUI.transform as RectTransform;

            rtUI.SetAsFirstSibling();
            listGarbageEnemies.Add(newEnemy);
            listGarbageEnemyUIs.Add(newEnemyUI);
        }
        else
        {
            newEnemy.transform.position = spawnPoint.position;
        }

        newEnemy.gameObject.SetActive(true);

        //Pair UI and unit.
        newEnemyUI.tUnit = newEnemy.transform;
        newEnemy.statsUI = newEnemyUI;
        newEnemyUI.gameObject.SetActive(true);

        newEnemy.Init();
    }

    private UnitStatsUI CheckForAvailableGarbageEnemyUI()
    {
        UnitStatsUI ui = null;

        if(listGarbageEnemyUIs.Count > 0)
        {
            //Check which one is active.
            foreach(UnitStatsUI u in listGarbageEnemyUIs)
            {
                if (!u.isActiveAndEnabled)
                {
                    ui = u;
                }
            }
        }

        return ui;
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

    private Transform FindFurthestSpawnPoint()
    {
        Transform spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];

        float distanceFromPlayer = 0f;

        for(int i = 0; i < enemySpawnPoints.Length; i++)
        {
            float pointDistance = Vector3.Distance(player.transform.position, enemySpawnPoints[i].position);
            if (pointDistance > distanceFromPlayer) {
                distanceFromPlayer = pointDistance;
                spawnPoint = enemySpawnPoints[i];
            }
        }

        return spawnPoint;
    }

    public void GamePause(bool b)
    {
        if (b)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}

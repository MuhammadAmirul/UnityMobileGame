using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGameV2 : MonoBehaviour
{
    [Header("Game Related Vars")]
    public int score;
    //private bool isSpawning = true;
    //private float timeSpawn = 5.0f;
    private float Timer;
    private float timeToSpawn;
    private float bigSpawn;
    public int addEnemies;
    public static int killCount;

    [Space]
    [Header("Player/Enemy Related Vars")]
    public PlayerStatus player;
    public EnemyHealthBarPosition prefabEnemyStatUI;
    public EnemyPenguinStatus prefabEnemy;
    public Transform[] enemySpawnPoints;
    private List<EnemyPenguinStatus> listGarbageEnemies;
    private List<EnemyHealthBarPosition> listGarbageEnemyUIs;


    [Space]
    [Header("UI Related Vars")]
    public Canvas canvasMenus;
    public Canvas canvas3D;
    public Text timerText;
    public Text enemies;
    public Text killCountText;


    private void Awake()
    {
        listGarbageEnemies = new List<EnemyPenguinStatus>();
        listGarbageEnemyUIs = new List<EnemyHealthBarPosition>();
    }

    private void Start()
    {
        /*if (isSpawning)
        {
            InvokeRepeating("SpawnEnemy", timeSpawn, timeSpawn);
        }*/
    }

    void FixedUpdate()
    {
        if (PlayerStatus.dead == false)
        {
            Timer += Time.deltaTime;
            timeToSpawn += Time.deltaTime;

            timerText.text = Mathf.Round(Timer).ToString();
            enemies.text = addEnemies.ToString();
            killCountText.text = killCount.ToString();

            if (Timer < 30f)
            {
                if (timeToSpawn >= 8f)
                {
                    SpawnEnemy();
                    timeToSpawn = 0f;
                    addEnemies += 1;
                }
            }

            if (Timer >= 30f)
            {
                if (timeToSpawn >= 5f)
                {
                    SpawnEnemy();
                    timeToSpawn = 0f;
                    addEnemies += 1;
                }
            }

            if (Timer >= 45f)
            {
                bigSpawn += Time.deltaTime;

                if (timeToSpawn >= 3f)
                {
                    SpawnEnemy();
                    timeToSpawn = 0f;
                    addEnemies += 1;
                }

                if (bigSpawn >= 8f)
                {
                    SpawnEnemy();
                    bigSpawn = 0f;
                    addEnemies += 1;
                }
            }
        }

        /*if (GetComponent<EnemyPenguinStatus>().dead == true)
        {
            score += 1;
            addEnemies -= 1;
        }*/
    }

    public void SpawnEnemy()
    {
        EnemyPenguinStatus newEnemyPenguin = CheckForAvailableGarbageEnemy();
        EnemyHealthBarPosition newEnemyUI = CheckForAvailableGarbageEnemyUI();

        Transform spawnPoint = FindFurthestSpawnPoint();

        if (newEnemyPenguin == null)
        {
            //Debug.Log("Instantiating new enemy");
            //Instantiate.
            newEnemyPenguin = Instantiate(prefabEnemy, spawnPoint.position, spawnPoint.rotation);

            listGarbageEnemies.Add(newEnemyPenguin);
        }
        else
        {
            //Debug.Log("Recycling new enemy");
            newEnemyPenguin.transform.position = spawnPoint.position;
        }

        if(newEnemyUI == null)
        {
            newEnemyUI = Instantiate(prefabEnemyStatUI, Vector3.zero, Quaternion.identity);
            newEnemyUI.transform.SetParent(prefabEnemyStatUI.transform.parent, true);
            newEnemyUI.transform.localScale = Vector3.one;
            RectTransform rtUI = newEnemyUI.transform as RectTransform;

            rtUI.SetAsFirstSibling();
            listGarbageEnemyUIs.Add(newEnemyUI);
        }
        else
        {

        }

        newEnemyPenguin.transform.position = spawnPoint.position;
        newEnemyPenguin.gameObject.SetActive(true);
        newEnemyUI.gameObject.SetActive(true);

        //Pair UI and unit.
        newEnemyUI.target = newEnemyPenguin.transform;
        newEnemyPenguin.statsUI = newEnemyUI;

        newEnemyPenguin.Init();
    }

    private EnemyHealthBarPosition CheckForAvailableGarbageEnemyUI()
    {
        EnemyHealthBarPosition ui = null;

        if (listGarbageEnemyUIs.Count > 0)
        {
            //Check which one is active.
            foreach (EnemyHealthBarPosition u in listGarbageEnemyUIs)
            {
                if (!u.isActiveAndEnabled)
                {
                    ui = u;
                }
            }
        }

        return ui;
    }

    private EnemyPenguinStatus CheckForAvailableGarbageEnemy()
    {
        EnemyPenguinStatus returnEnemy = null;

        //Go through list and return any inactive spawned enemy.
        if (listGarbageEnemies.Count > 0)
        {
            foreach (EnemyPenguinStatus ec in listGarbageEnemies)
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

        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            float pointDistance = Vector3.Distance(player.transform.position, enemySpawnPoints[i].position);
            if (pointDistance > distanceFromPlayer)
            {
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

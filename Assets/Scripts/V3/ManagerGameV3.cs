using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public enum UNITFACTION
{
    NONE,
    PLAYER,
    ENEMY
}

public enum UNITTYPE
{
    A,
    B,
    C
}

public class ManagerGameV3 : MonoBehaviour
{
    public CinemachineVirtualCamera camOrtho;
    public Transform[] tSpawnPoints;
    public Unit[] prefabUnits;
    public Projectile[] prefabProjectiles;
    public FixedJoystick[] controlJoysticks;    //0 = move, 1 = shoot
    public StatsUI prefabStatsUI;

    public TextMeshProUGUI tmTimer;
    public TextMeshProUGUI tmKills;


    private float timeSurvived;
    private int countKills;
    private int countSpawns;

    private float spawnRate = 5f;

    private List<Unit> listGarbageUnits;
    private List<StatsUI> listGarbageStatsUI;
    private List<Projectile> listGarbageProjectiles;

    [HideInInspector] public Unit unitPlayer;

    private void Awake()
    {
        listGarbageUnits = new List<Unit>();
        listGarbageStatsUI = new List<StatsUI>();
        listGarbageProjectiles = new List<Projectile>();

        InitGame();
    }

    private void LateUpdate()
    {
        // Update timer
        timeSurvived += Time.deltaTime;

        float roundTime = Mathf.Round(timeSurvived * 100f) / 100f;
        tmTimer.text = "TIME: " + roundTime;
    }

    public void UpdateKills(int i)
    {
        countKills += i;

        tmKills.text = "KILLS: " + countKills;
    }

    public void InitGame()
    {
        // For now, a zero condition game init call from previous screen that starts the game.

        // 1st - Set all existing garbage collected to deactivate, so it can be ready for new game.
        RecycleAllGarbage();

        // 2nd - Reset scores and time
        timeSurvived = 0f;
        countKills = 0;
        UpdateGameUI();

        // 3rd - Init Player.
        SpawnUnit(UNITFACTION.PLAYER, UNITTYPE.A);

        // 4th - Spawn Enemies.
        InvokeRepeating("SpawnEnemy", 3f, spawnRate);
    }

    private void UpdateGameUI()
    {

    }

    public void SpawnEnemy()
    {
        SpawnUnit(UNITFACTION.ENEMY, UNITTYPE.B);
        countSpawns++;
        AdjustSpawnRate();
    }

    private void AdjustSpawnRate()
    {
        int spawnFactor = countSpawns / 10;

        float newSpawnRate = 5.0f - (0.5f * spawnFactor);
        newSpawnRate = Mathf.Clamp(spawnRate, 2.0f, 5.0f);

        if(newSpawnRate < spawnRate)
        {
            CancelInvoke("SpawnEnemy");
            InvokeRepeating("SpawnEnemy", newSpawnRate, newSpawnRate);
            spawnRate = newSpawnRate;
        }
    }

    private void SpawnUnit(UNITFACTION faction, UNITTYPE type)
    {
        // Spawn unit gameobject first.
        Unit unitToSpawn = SpawnOrRecycleUnit(type);

        // Attach a stats UI to it as well.
        unitToSpawn.statsUI = SpawnOrRecycleStatsUI();

        if(faction == UNITFACTION.PLAYER)
        {
            // Set camera focus on unit.
            camOrtho.Follow = unitToSpawn.transform;
            unitPlayer = unitToSpawn;
            unitToSpawn.transform.position = new Vector3(0f, 0f, 1f);
        }
        else if (faction == UNITFACTION.ENEMY)
        {
            // Find the furthest spawn point from player.
            unitToSpawn.transform.position = GetFurthestSpawnPoint();
        }

        unitToSpawn.gameObject.SetActive(true);
        unitToSpawn.Init(faction);
    }

    private Vector3 GetFurthestSpawnPoint()
    {
        Vector3 pos = Vector3.zero;
        float dist = 0f;

        foreach (Transform pt in tSpawnPoints)
        {
            float ptDist = Vector3.Distance(unitPlayer.transform.position, pt.position);
            if(ptDist > dist)
            {
                pos = pt.position;
                dist = ptDist;
            }
        }

        return pos;
    }

    private Unit SpawnOrRecycleUnit(UNITTYPE type)
    {
        Unit unitToReturn = null;

        // Iterate through existing garbage list first.
        foreach(Unit u in listGarbageUnits)
        {
            switch (type)
            {
                case UNITTYPE.A:
                    //Check for 2 conditions: Unit Type and availability.
                    if(u.unitType == UNITTYPE.A && !u.isActiveAndEnabled)
                    {
                        unitToReturn = u;
                    }
                    break;

                case UNITTYPE.B:
                    if (u.unitType == UNITTYPE.B && !u.isActiveAndEnabled)
                    {
                        unitToReturn = u;
                    }
                    break;

                case UNITTYPE.C:
                    if (u.unitType == UNITTYPE.C && !u.isActiveAndEnabled)
                    {
                        unitToReturn = u;
                    }
                    break;
            }
        }

        // Now, if it's still null, then we instantiate and return.

        if(unitToReturn == null)
        {
            // Iterate through unit prefabs and inst.
            foreach(Unit prefabUnit in prefabUnits)
            {
                if(prefabUnit.unitType == type)
                {
                    GameObject inst = Instantiate(prefabUnit.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
                    unitToReturn = inst.GetComponent<Unit>();

                    // Add to garbage list.
                    listGarbageUnits.Add(unitToReturn);
                }
            }
        }
        return unitToReturn;
    } 

    private StatsUI SpawnOrRecycleStatsUI()
    {
        StatsUI statsUIToReturn = null;

        foreach(StatsUI ui in listGarbageStatsUI)
        {
            if (!ui.isActiveAndEnabled)
            {
                statsUIToReturn = ui;
            }
        }

        if(statsUIToReturn == null)
        {
            GameObject inst = Instantiate(prefabStatsUI.gameObject, prefabStatsUI.transform.parent) as GameObject;
            statsUIToReturn = inst.GetComponent<StatsUI>();
            statsUIToReturn.transform.SetSiblingIndex(1);

            listGarbageStatsUI.Add(statsUIToReturn);
        }

        return statsUIToReturn;
    }

    public Projectile SpawnOrRecycleProjectile()
    {
        Projectile projectileToReturn = null;

        foreach(Projectile p in listGarbageProjectiles)
        {
            if (!p.isActiveAndEnabled)
            {
                projectileToReturn = p;
            }
        }

        if(projectileToReturn == null)
        {
            GameObject inst = Instantiate(prefabProjectiles[0].gameObject) as GameObject;
            projectileToReturn = inst.GetComponent<Projectile>();

            listGarbageProjectiles.Add(projectileToReturn);
        }

        return projectileToReturn;
    }

    private void RecycleAllGarbage()
    {
        foreach(Unit u in listGarbageUnits)
        {
            u.Recycle();
        }
        foreach(StatsUI s in listGarbageStatsUI)
        {
            s.Recycle();
        }
        foreach(Projectile p in listGarbageProjectiles)
        {
            p.Recycle();
        }
    }
}

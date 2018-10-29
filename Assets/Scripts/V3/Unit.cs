using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Unit : MonoBehaviour {

    public UNITTYPE unitType;
    public Transform[] projectileSpawnPoints;
    public float projectileSpeed = 200.0f;
    public Animator animator;

    [HideInInspector] public StatsUI statsUI;
    [HideInInspector] public UNITFACTION faction;
    private CharacterController controller;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private ManagerGameV3 managerGame;

    private FixedJoystick[] joysticks;

    private float unitSpeed = 3f;
    private float baseUnitSpeed = 3f;
    private Vector3 unitMovement;
    private bool isActive;

    private bool isFiring;
    public float bulletSpeed = 5f;
    public float counterPauseBetweenShots = 0.25f;
    private float counterShots;

    public float hpCurrent;
    public float hpBase;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        managerGame = GameObject.Find("GAME MANAGER").GetComponent<ManagerGameV3>();
    }

    public void Recycle()
    {
        gameObject.SetActive(false);
    }

    public void Init(UNITFACTION f)
    {
        faction = f;

        switch (faction)
        {
            case UNITFACTION.PLAYER:
                joysticks = managerGame.controlJoysticks;
                break;
            case UNITFACTION.ENEMY:
                agent.isStopped = false;
                break;
        }

        controller.detectCollisions = rb.detectCollisions = true;

        //Set statsUI behaviour.
        statsUI.tTarget = transform;
        statsUI.Init(faction);

        //Stats
        hpCurrent = hpBase;
        UpdateUI_HP();

        isActive = true;
    }

    void FixedUpdate()
    {
        if(!isActive)
        {
            return;
        }

        switch (faction)
        {
            case UNITFACTION.PLAYER:
                
                float h = 0f;
                float v = 0f;
                float hs = 0f;
                float vs = 0f;

                if (Application.platform == RuntimePlatform.Android)
                {
                    h = joysticks[0].Horizontal;
                    v = joysticks[0].Vertical;

                    hs = joysticks[1].Horizontal;
                    vs = joysticks[1].Vertical;
                }
                else
                {
                    h = Input.GetAxisRaw("Horizontal");
                    v = Input.GetAxisRaw("Vertical");

                    hs = Input.GetAxisRaw("Horizontal Shoot");
                    vs = Input.GetAxisRaw("Vertical Shoot");
                }

                Move(h, v);
                Aim(hs, vs);
                Shoot();
                break;

            case UNITFACTION.ENEMY:
                agent.destination = managerGame.unitPlayer.transform.position;
                break;
        }
        
    }

    void Move(float h, float v)
    {
        unitMovement.Set(h, 0f, v);

        unitMovement = unitMovement.normalized * unitSpeed * Time.deltaTime;

        if (unitMovement.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(unitMovement);
            animator.SetBool("boolWalking", true);
        }
        else
        {
            animator.SetBool("boolWalking", false);
        }

        controller.Move(unitMovement);
        
        rb.MovePosition(transform.position + unitMovement);
    }

    void Aim(float hs, float vs)
    {
        //Aim first.
        Vector3 playerDirection = Vector3.right * hs + Vector3.forward * vs;

        if (playerDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);

            //Also enable shoot.
            isFiring = true;
            animator.SetBool("boolShooting", true);
            animator.SetBool("boolWalking", false);
        }
        else
        {
            isFiring = false;
            animator.SetBool("boolShooting", false);
        }
    }

    // To refactor if considering shooting profiles (e.g. different weapons etc).
    private void Shoot()
    {
        if (!isFiring)
        {
            counterShots = 0f;
            return;
        }
        else
        {
            counterShots -= Time.deltaTime;
            if (counterShots <= 0f)
            {
                foreach(Transform pt in projectileSpawnPoints)
                {
                    Projectile p = managerGame.SpawnOrRecycleProjectile();
                    p.transform.position = pt.position;
                    p.transform.eulerAngles = pt.eulerAngles;
                    p.projectileOwner = faction;
                    p.gameObject.SetActive(true);

                    p.rb.AddForce(p.transform.forward * projectileSpeed);
                }
                counterShots = counterPauseBetweenShots;
            }
        }
    }

    // To be fixed by twaeking enemies to use colliders, not controllers.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.name);
        if(hit.gameObject.tag == "UNIT")
        {
            // Get colliding unit's faction
            UNITFACTION colliderFaction = hit.gameObject.GetComponent<Unit>().faction;
            
            switch (faction)
            {
                case UNITFACTION.PLAYER:
                    if(colliderFaction == UNITFACTION.ENEMY)
                    {
                        NavMeshAgent a = hit.gameObject.GetComponent<NavMeshAgent>();
                        Knockback(a.velocity);
                    }
                    break;

                case UNITFACTION.ENEMY:
                    if(colliderFaction == UNITFACTION.PLAYER)
                    {
                        Knockback(hit.rigidbody.velocity);

                    }
                    break;
            }
        }
    }

    public void ResetUnitSpeed()
    {
        unitSpeed = baseUnitSpeed;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (faction == UNITFACTION.PLAYER) {
            switch (col.tag)
            {
                case "Speed":
                    Debug.Log("SPEED");
                    unitSpeed = baseUnitSpeed * 2f;
                    Invoke("ResetUnitSpeed", 5f);
                    break;

                case "Health":
                    ChangeHP(3.0f);
                    col.SendMessageUpwards("RespawnModifier", SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }
    }

    private void Knockback(Vector3 vel)
    {
        Debug.Log("KNOCKBACK: " + vel);
        rb.AddForce(vel * 100f, ForceMode.Impulse);
    }

    public void ChangeHP(float amt)
    {
        hpCurrent += amt;
        hpCurrent = Mathf.Clamp(hpCurrent, 0f, hpBase);
        UpdateUI_HP();

        if(hpCurrent == 0f)
        {
            StartCoroutine(Death());
        }

    }

    private IEnumerator Death()
    {
        switch (faction)
        {
            case UNITFACTION.ENEMY:
                agent.isStopped = true;
                controller.detectCollisions = rb.detectCollisions = false;

                //Add Scire
                managerGame.UpdateKills(1);
                break;

            case UNITFACTION.PLAYER:

                break;
        }

        statsUI.Recycle();

        animator.SetBool("boolDead", true);
        yield return new WaitForSeconds(1.0f);

        Recycle();
        yield return null;
    }

    private void UpdateUI_HP()
    {
        statsUI.UpdateHPSlider(hpCurrent / hpBase);
    }
}

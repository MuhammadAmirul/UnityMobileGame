
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyV3 : MonoBehaviour
{
    public Transform player;
    public Camera cam;
    public NavMeshAgent agent;
    public EnemyHealthBarPosition statsUI;
    private bool isMoving;
    private bool isShooting;
    private bool dead;
    public bool playerDead;

    private float delay;
    private float dying;

    public float health;
    public float maxHealth = 5f;

    public Animator anim;

    public EnemyBullet enemyBullet;
    public Transform[] spawnBullet;

    public Image healthBar;
    public GameObject HealthBar;

    public BoxCollider enemycol;

    //private float speed = 3.0f;

    // Use this for initialization
    void Start ()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //transform.LookAt(player);
        //transform.position += transform.forward * speed * Time.deltaTime;

        //if (PlayerStatus.dead == false)
        //{
        if (!dead)
        {
            agent.SetDestination(player.position);

            if (!isMoving && isShooting)
            {
                StopAndShoot();
            }
            else if (isMoving && !isShooting)
            {
                Walk();
            }
        }
	}

    public void Init()
    {
        GetComponent<NavMeshAgent>().speed = 2.0f;
        isMoving = true;
        isShooting = false;
        health = maxHealth;
        dead = false;
        //enemycol.enabled = true;
        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        float val = health / maxHealth;
        statsUI.UpdateHPValue(val);
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            isMoving = false;
            isShooting = true;
        }
    }

    /*void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Bullet"))
        {
            Debug.Log("Shot!");
            health -= 1f;
            
            if(health <= 0f)
            {
                health = 0f;
                Die();
                //enemycol.enabled = false;
                dead = true;
            }

            UpdateStatsUI();

            col.gameObject.SendMessage("Despawn", SendMessageOptions.DontRequireReceiver);
        }
    }*/

    public void Damage()
    {
        //Debug.Log("Shot!");
        health -= 1f;

        if (health <= 0f)
        {
            health = 0f;
            Die();
            //enemycol.enabled = false;
            
        }

        UpdateStatsUI();
    }

    void OnTriggerExit(Collider col)
    {
        if (!col.gameObject.tag.Equals("Player"))
        {
            isMoving = true;
            isShooting = false;
        }
    }

    void SpawnBullet()
    {
        EnemyBullet leftBullet = Instantiate(enemyBullet, spawnBullet[0].position, spawnBullet[0].rotation) as EnemyBullet;
        EnemyBullet rightBullet = Instantiate(enemyBullet, spawnBullet[1].position, spawnBullet[1].rotation) as EnemyBullet;
        leftBullet.speed = 8f;
        leftBullet.gameObject.SetActive(true);
        rightBullet.speed = 8f;
        rightBullet.gameObject.SetActive(true);
    }

    void StopAndShoot()
    {
        transform.LookAt(player);
        GetComponent<NavMeshAgent>().speed = 0.0f;
        delay += Time.deltaTime;
        anim.Play("Shooting");
        if (delay >= 0.4f)
        {
            delay = 0.0f;
            SpawnBullet();
        }
    }

    void Walk()
    {
        anim.Play("Walk");
        GetComponent<NavMeshAgent>().speed = 2.0f;
    }

    void Die()
    {
        if (agent.isStopped == false)
        {
            dead = true;
            isMoving = false;
            isShooting = false;

            
            //GetComponent<NavMeshAgent>().speed = 0.0f;
            //dying += Time.deltaTime;
            anim.SetTrigger("Death");
            //HealthBar.gameObject.SetActive(false);
            statsUI.Despawn();
            ManagerGameV2.killCount += 1;

            Invoke("Despawn", 0.75f);
            agent.isStopped = true;
        }
    }

    public void Despawn()
    {
        statsUI = null;
        gameObject.SetActive(false);
        
    }
}

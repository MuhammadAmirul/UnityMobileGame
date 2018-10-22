
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public Camera cam;
    public NavMeshAgent agent;
    private bool isMoving;
    private bool isShooting;
    private bool dead;

    private float delay;
    private float dying;

    public float health;
    public float maxHealth = 10f;

    public Animator anim;

    public GameObject enemyBullet;
    public Transform[] spawnBullet;

    public Image healthBar;
    public GameObject HealthBar;

    Collider enemycol;

    //private float speed = 3.0f;

    // Use this for initialization
    void Start ()
    {
        GetComponent<NavMeshAgent>().speed = 2.0f;
        isMoving = true;
        isShooting = false;
        health = maxHealth;
        enemycol = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / maxHealth;

        //transform.LookAt(player);
        //transform.position += transform.forward * speed * Time.deltaTime;

        Ray ray = cam.ScreenPointToRay(player.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(player.position);
        }

        if (health == 10f)
        {
            HealthBar.gameObject.SetActive(false);
            dead = false;
        }
        else
        {
            HealthBar.gameObject.SetActive(true);
        }

        if (health <= 0f)
        {
            dead = true;
            Die();
            enemycol.enabled = false;
        }

        if (!isMoving && isShooting)
        {
            StopAndShoot();
        }
        else if (isMoving && !isShooting)
        {
            Walk();
        }
	}

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            isMoving = false;
            isShooting = true;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Bullet"))
        {
            health -= 2f;
        }
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
        Instantiate(enemyBullet, spawnBullet[0].position, spawnBullet[0].rotation);
        Instantiate(enemyBullet, spawnBullet[1].position, spawnBullet[1].rotation);
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
        if (dead)
        {
            isMoving = false;
            isShooting = false;
            GetComponent<NavMeshAgent>().speed = 0.0f;
            Destroy(gameObject, 1.5f);
            anim.Play("Death");
        }
    }
}

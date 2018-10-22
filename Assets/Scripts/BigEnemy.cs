using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BigEnemy : MonoBehaviour
{
    public Transform player;
    public Camera cam;
    public NavMeshAgent agent;
    private bool isMoving;

    private float delay;
    private float dying;

    public float health;
    public float maxHealth = 14f;

    public Animator anim;

    public Image healthBar;
    public GameObject HealthBar;

    // Use this for initialization
    void Start ()
    {
        GetComponent<NavMeshAgent>().speed = 1.5f;
        isMoving = true;
        health = maxHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {
        healthBar.fillAmount = health / maxHealth;

        Ray ray = cam.ScreenPointToRay(player.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(player.position);
        }

        if (health == 14f)
        {
            HealthBar.gameObject.SetActive(false);
        }
        else
        {
            HealthBar.gameObject.SetActive(true);
        }

        if (health <= 0)
        {
            dying += Time.deltaTime;
            EnemySpawner.killCount += 1;
            anim.Play("Death");

            if (dying >= 1.5f)
            {
                Destroy(gameObject);
                dying = 0.0f;
            }
        }

        if (!isMoving)
        {
            GetComponent<NavMeshAgent>().speed = 0.0f;
            delay += Time.deltaTime;
            
            if (delay >= 0.4f)
            {
                anim.Play("Melee");
                delay = 0.0f;
            }
        }
        else
        {
            anim.Play("Walk");
            GetComponent<NavMeshAgent>().speed = 1.5f;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            isMoving = false;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Bullet"))
        {
            health -= 2;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (!col.gameObject.tag.Equals("Player"))
        {
            isMoving = true;
        }
    }
}

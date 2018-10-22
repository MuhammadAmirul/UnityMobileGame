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
    private bool isMelee;
    private bool dead;

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
        isMelee = false;
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
            dead = false;
        }
        else
        {
            HealthBar.gameObject.SetActive(true);
        }

        if (health <= 0)
        {
            dead = true;
            Die();
        }

        if (!isMoving && isMelee)
        {
            StopAndHit();
        }
        else if (isMoving && !isMelee)
        {
            Walk();
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            isMoving = false;
            isMelee = true;
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
            isMelee = false;
        }
    }

    void StopAndHit()
    {
        GetComponent<NavMeshAgent>().speed = 0.0f;
        delay += Time.deltaTime;
        
        if (delay >= 0.5f)
        {
            anim.Play("Melee");
            delay = 0.0f;
        }
    }

    void Walk()
    {
        anim.Play("Walk");
        GetComponent<NavMeshAgent>().speed = 1.5f;
    }

    void Die()
    {
        if (dead)
        {
            isMoving = false;
            GetComponent<NavMeshAgent>().speed = 0.0f;
            Destroy(gameObject, 1.5f);
            anim.Play("Death");
        }
    }
}

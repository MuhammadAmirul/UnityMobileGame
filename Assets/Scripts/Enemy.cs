
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public Camera cam;
    public NavMeshAgent agent;

    public int health = 10;

    //private float speed = 3.0f;

	// Use this for initialization
	void Start ()
    {
        
	}

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        //transform.position += transform.forward * speed * Time.deltaTime;

        Ray ray = cam.ScreenPointToRay(player.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(player.position);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
	}

<<<<<<< HEAD
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Bullet"))
        {
            health -= 2;
            print("health" + health);
=======
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            GetComponent<NavMeshAgent>().speed = 0.0f;
        }

        if (col.gameObject.tag.Equals("Bullet"))
        {
            health -= 2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player"))
        {
            GetComponent<NavMeshAgent>().speed = 2.0f;
>>>>>>> 897770bbfae3dadb61fbe41db85d14a9e18c884e
        }
    }
}

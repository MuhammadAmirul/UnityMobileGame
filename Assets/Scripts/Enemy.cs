using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public Camera cam;
    public NavMeshAgent agent;

    //private float speed = 3.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(player);

        //transform.position += transform.forward * speed * Time.deltaTime;

        Ray ray = cam.ScreenPointToRay(player.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(player.position);
        }
	}
}

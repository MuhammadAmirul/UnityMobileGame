using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (Rigidbody))]
public class EnemyControl : MonoBehaviour {

    public Transform player;
    public Camera cam;

    public int baseHP = 3;
    public int currentHP;
    private NavMeshAgent agent;
    private Rigidbody rb;

    public float speed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }


    // Use this for initialization
    void Start()
    {

    }

    public void Init()
    {
        if (agent)
        {
            agent.speed = speed;
        }

        currentHP = baseHP;

        UpdateUI();
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
    }

    private void UpdateUI()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Damaged Player!");

            //Trigger knockback.
            collision.rigidbody.AddForce(agent.velocity, ForceMode.Impulse);
            rb.AddForce(agent.velocity * -1f, ForceMode.Impulse);
        }
    }

    public void Damage(int dmg)
    {
        currentHP -= dmg;

        if(currentHP <= 0)
        {
            currentHP = 0;
            Death();
        }

        UpdateUI();
    }

    private void Death()
    {
        gameObject.SetActive(false);
    }
}

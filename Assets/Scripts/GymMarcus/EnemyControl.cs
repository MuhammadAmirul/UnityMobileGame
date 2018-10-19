using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (Rigidbody))]
public class EnemyControl : MonoBehaviour {

    public UnitStatsUI statsUI;

    public Transform player;
    public Camera cam;

    public float baseHP = 3f;
    public float currentHP;
    private NavMeshAgent agent;
    private Rigidbody rb;

    private bool isTracking = false;
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

        isTracking = true;
    }


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);

        if (isTracking)
        {
            agent.SetDestination(player.position);

            Vector3 forward = transform.TransformDirection(Vector3.forward) * 4;
            Debug.DrawRay(transform.position, forward, Color.green);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, forward, out hit, 4))
            {
                if(hit.collider.gameObject.tag == "Player")
                {
                    StartCoroutine("StopAndShoot");
                    isTracking = false;
                }
            }
        }
    }

    private IEnumerator StopAndShoot()
    {
        agent.isStopped = true;

        yield return new WaitForSeconds(2.0f);
        isTracking = true;
        agent.isStopped = false;

        yield return null; 
    }

    private void UpdateUI()
    {
        float val = currentHP / baseHP;

        statsUI.UpdateSliderHP(val);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Debug.Log("Damaged Player!");
            collision.gameObject.SendMessage("Damage", 0.5f, SendMessageOptions.DontRequireReceiver);


            Debug.Log(agent.velocity.sqrMagnitude + " | " + collision.rigidbody.velocity.sqrMagnitude);
            //Trigger knockback.
            if (agent.velocity.sqrMagnitude > 0f)
            {
                collision.rigidbody.AddForce(agent.velocity, ForceMode.Impulse);
                rb.AddForce(agent.velocity * -1f, ForceMode.Impulse);
            }
            else
            {
                //Player knockback it's own velocity.
                collision.rigidbody.AddForce(collision.rigidbody.velocity * -1f, ForceMode.Impulse);
                rb.AddForce(collision.rigidbody.velocity, ForceMode.Impulse);
            }
        }
    }

    public void Damage(float dmg)
    {
        currentHP -= dmg;

        if(currentHP <= 0f)
        {
            currentHP = 0f;
            Death();
        }

        UpdateUI();
    }

    private void Death()
    {
        statsUI.Deactivate();
        gameObject.SetActive(false);
    }
}

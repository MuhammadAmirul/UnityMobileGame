using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPenguinControl : MonoBehaviour
{
    public UnitStatsUI statsUI;

    public Transform player;
    public Camera cam;

    public EnemyBullet bullet;
    public Transform[] tTarget;

    public Animator anim;

    public float bulletSpeed = 5f;
    private NavMeshAgent agent;

    private bool isTracking = false;
    public float speed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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

        //currentHP = baseHP;

        isTracking = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (isTracking)
        {
            agent.SetDestination(player.position);
            anim.Play("Walk");

            Vector3 forward = transform.TransformDirection(Vector3.forward) * 4;
            Debug.DrawRay(transform.position, forward, Color.green);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, forward, out hit, 4))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    transform.LookAt(player);
                    anim.Play("Shooting");
                    StartCoroutine("StopAndShoot");
                    isTracking = false;
                }
            }
        }
    }

    private IEnumerator StopAndShoot()
    {
        agent.isStopped = true;

        EnemyBullet leftBullet = Instantiate(bullet, tTarget[0].position, tTarget[0].rotation) as EnemyBullet;
        EnemyBullet rightBullet = Instantiate(bullet, tTarget[1].position, tTarget[1].rotation) as EnemyBullet;
        leftBullet.speed = bulletSpeed;
        leftBullet.gameObject.SetActive(true);
        rightBullet.speed = bulletSpeed;
        rightBullet.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.4f);
        isTracking = true;
        agent.isStopped = false;

        yield return null;
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
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
    }*/
}

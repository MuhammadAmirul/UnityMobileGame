using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Related Vars")]
    public float speed = 8f;
    private Vector3 movement;
    private Rigidbody rb;

    [Space]
    [Header("Shooting Related Vars")]
    public Bullet bullet;
    public Transform tTarget;
    private bool isFiring = false;
    public float bulletSpeed = 5f;
    public float counterPauseBetweenShots = 0.25f;
    private float counterShots;
    
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float hs = Input.GetAxisRaw("Horizontal Shoot");
        float vs = Input.GetAxisRaw("Vertical Shoot");

        Move(h, v);
        Aim(hs, vs);
        Shoot();
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);

        movement = movement.normalized * speed * Time.deltaTime;

        if(movement.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(movement);

        }

        rb.MovePosition(transform.position + movement);
    }

    void Aim(float hs, float vs)
    {
        //Aim first.
        Vector3 playerDirection = Vector3.right * hs + Vector3.forward * vs;

        if(playerDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);

            //Also enable shoot.
            isFiring = true;
        }
        else
        {
            isFiring = false;
        }
    }

    void Shoot()
    {
        if (!isFiring)
        {
            counterShots = 0f;
            return;
        }
        else
        {
            counterShots -= Time.deltaTime;
            if(counterShots <= 0f)
            {
                counterShots = counterPauseBetweenShots;

                //Create bullet.
                Bullet newBullet = Instantiate(bullet, tTarget.position, tTarget.rotation) as Bullet;
                newBullet.speed = bulletSpeed;
                newBullet.gameObject.SetActive(true);
            }
        }
    }
    
}

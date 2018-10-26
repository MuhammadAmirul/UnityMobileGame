using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerV2 : MonoBehaviour
{
    [Header("Movement Related Vars")]
    public Joystick joystickMove;
    public Joystick joystickShoot;
    public UnitStatsUI statsUI;

    private Animator controller;

    public float speed = 3f;
    public bool boosted = false;
    private float decay = 5f;
    private Vector3 movement;
    private Rigidbody rb;

    [Space]
    [Header("Shooting Related Vars")]
    public PlayerBullet bullet;
    public Transform tTarget;
    public Image overheatBar;
    private bool isFiring = false;
    private bool overheated = false;
    public float bulletSpeed = 5f;
    public float counterPauseBetweenShots = 0.05f;
    private float counterShots;
    private float overheating;
    private float maxOverheating = 2f;
    private float coolingDown = 2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<Animator>();
    }

    void Start()
    {
        overheating = 0f;
    }

    void FixedUpdate()
    {
        overheatBar.fillAmount = overheating / maxOverheating;

        float h = 0f;
        float v = 0f;

        float hs = 0f;
        float vs = 0f;

        if (Application.platform == RuntimePlatform.Android)
        {
            h = joystickMove.Horizontal;
            v = joystickMove.Vertical;

            hs = joystickShoot.Horizontal;
            vs = joystickShoot.Vertical;
        }
        else
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            hs = Input.GetAxisRaw("Horizontal Shoot");
            vs = Input.GetAxisRaw("Vertical Shoot");
        }

        if (PlayerStatus.dead == true)
        {
            speed = 0f;
            isFiring = false;
        }

        Move(h, v);
        Aim(hs, vs);
        Shoot();
        Overheat();
    }

    void Update()
    {
        if (boosted)
        {
            decay -= Time.deltaTime;
            speed = 6f;

            if (decay <= 0f)
            {
                decay = 5f;
                boosted = false;
            }
        }

        if (!boosted)
        {
            speed = 3f;
        }
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);

        movement = movement.normalized * speed * Time.deltaTime;

        if (movement.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(movement);
            controller.SetBool("boolWalking", true);
        }
        else
        {
            controller.SetBool("boolWalking", false);
        }

        rb.MovePosition(transform.position + movement);
    }

    void Aim(float hs, float vs)
    {
        //Aim first.
        Vector3 playerDirection = Vector3.right * hs + Vector3.forward * vs;

        if (playerDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);

            //Also enable shoot.
            isFiring = true;
            controller.SetBool("boolShooting", true);

        }
        else
        {
            isFiring = false;
            controller.SetBool("boolShooting", false);
        }
    }

    void Shoot()
    {
        if (!isFiring && overheated)
        {
            counterShots = 0f;
            return;
        }
        else if (isFiring && !overheated)
        {
            counterShots -= Time.deltaTime;
            if (counterShots <= 0f)
            {
                counterShots = counterPauseBetweenShots;

                //Create bullet.
                PlayerBullet newBullet = Instantiate(bullet, tTarget.position, tTarget.rotation) as PlayerBullet;
                newBullet.speed = bulletSpeed;
                newBullet.gameObject.SetActive(true);
                overheating += 0.4f;
            }
        }
    }

    void Overheat()
    {
        if (overheating >= maxOverheating)
        {
            overheated = true;
            overheating = maxOverheating;
        }
        else
        {
            overheated = false;
        }

        if(overheating < 0f)
        {
            overheating = 0f;
        }

        if (!overheated)
        {
            overheating -= Time.deltaTime;
        }

        if (overheated)
        {
            coolingDown -= Time.deltaTime;
            isFiring = false;

            if (coolingDown <= 0f)
            {
                overheated = false;
                overheating = 0f;
                coolingDown = 2f;
            }
        }
    }
}

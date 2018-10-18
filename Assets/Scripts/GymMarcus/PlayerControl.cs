using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Related Vars")]
    public Joystick joystickMove;
    public Joystick joystickShoot;
    public UnitStatsUI statsUI;

    public float speed = 8f;
    private Vector3 movement;
    private Rigidbody rb;

    [Space]
    [Header("Shooting Related Vars")]
    public Bulletv2 bullet;
    public Transform tTarget;
    private bool isFiring = false;
    public float bulletSpeed = 5f;
    public float counterPauseBetweenShots = 0.25f;
    private float counterShots;

    public float baseHP = 20.0f;
    public float currentHP;

    public float baseSP = 10.0f;
    public float currentSP;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentHP = baseHP;

    }

    private void UpdateUI()
    {
        float valHP = currentHP / baseHP;
        statsUI.UpdateSliderHP(valHP);

        float valSP = currentSP / baseSP;
        statsUI.UpdateSliderSP(valSP);
    }

    void FixedUpdate()
    {
        float h = 0f;
        float v = 0f;
        float hs = 0f;
        float vs = 0f;

        if(Application.platform == RuntimePlatform.Android)
        {
            h = joystickMove.Horizontal;
            v = joystickMove.Vertical;

            hs = joystickShoot.Horizontal;
            vs = joystickShoot.Vertical;
        }
        else {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            hs = Input.GetAxisRaw("Horizontal Shoot");
            vs = Input.GetAxisRaw("Vertical Shoot");
        }

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
                Bulletv2 newBullet = Instantiate(bullet, tTarget.position, tTarget.rotation) as Bulletv2;
                newBullet.speed = bulletSpeed;
                newBullet.gameObject.SetActive(true);
            }
        }
    }

    public void Damage(float dmg)
    {
        currentHP -= dmg;
        if(currentHP <= 0f)
        {
            currentHP = 0f;
            Debug.Log("GAME OVER");
        }

        UpdateUI();
    }


}

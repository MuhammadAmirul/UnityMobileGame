using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoyStickMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;

    private bool walking;
    private bool idle;
    private Vector3 movement;
    private Rigidbody rb;

    protected FixedJoystick joystick;

    public Animator anim;

    public Joystick joystickMove;

    // Use this for initialization
    void Start ()
    {
        joystick = FindObjectOfType<FixedJoystick>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //var rigidbody = GetComponent<Rigidbody>();

        //rigidbody.velocity = new Vector3(joystick.Horizontal * 5f, rigidbody.velocity.y, joystick.Vertical * 5f);

        // Move translation along the object's x and z axis
        // transform.position += new Vector3(joystick.Horizontal * speed * Time.deltaTime, 0, joystick.Vertical * speed * Time.deltaTime);

        // Joystick not pressed, play player's Idle animation
        /*if (!joystick.Pressed)
        {
            walking = false;
            idle = true;
        }
        if (joystick.Pressed) // Joystick pressed, play player's Walking animation
        {
            walking = true;
            idle = false;
        }*/

        if (!walking && idle)
        {
            anim.SetBool("walk", false);
            anim.SetBool("idle", true);
        }

        if (walking && !idle)
        {
            anim.SetBool("walk", true);
            anim.SetBool("idle", false);
        }

        if (PlayerStatus.health <= 0f)
        {
            joystick.Pressed = false;
            GetComponent<Enemy>().agent.speed = 0.0f;
            GetComponent<BigEnemy>().agent.speed = 0.0f;
        }
    }

    void FixedUpdate()
    {
        float h = 0f;
        float v = 0f;

        if (Application.platform == RuntimePlatform.Android)
        {
            h = joystickMove.Horizontal;
            v = joystickMove.Vertical;
        }
        else
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }

        Move(h, v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);

        movement = movement.normalized * speed * Time.deltaTime;

        if (movement.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(movement);
            walking = true;
            idle = false;
        }
        else
        {
            walking = false;
            idle = false;
        }

        rb.MovePosition(transform.position + movement);
    }
}

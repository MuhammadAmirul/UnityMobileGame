using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoyStickMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;

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
        // Joystick not pressed, play player's Idle animation
        /*if (!joystick.Pressed)
        {
            anim.Play("idle");
        }
        else if (joystick.Pressed) // Joystick pressed, play player's Walking animation
        {
            anim.Play("walk");
        }*/

        if (PlayerStatus.health <= 0f)
        {
            joystick.Pressed = false;
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
            //anim.SetTrigger("isWalking");
            //anim.ResetTrigger("isIdle");
            anim.Play("walk");
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
            //anim.SetTrigger("isWalking");
            //anim.ResetTrigger("isIdle");
            anim.Play("walk");
        }
        else
        {
            //anim.SetTrigger("isIdle");
            //anim.ResetTrigger("isWalking");
            anim.Play("idle");
        }

        rb.MovePosition(transform.position + movement);
    }
}

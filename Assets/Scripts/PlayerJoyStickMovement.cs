using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoyStickMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;

    private bool walking;
    private bool idle;

    protected FixedJoystick joystick;

    public Animator anim;

	// Use this for initialization
	void Start ()
    {
        joystick = FindObjectOfType<FixedJoystick>();
	}

    // Update is called once per frame
    void Update()
    {
        //var rigidbody = GetComponent<Rigidbody>();

        //rigidbody.velocity = new Vector3(joystick.Horizontal * 5f, rigidbody.velocity.y, joystick.Vertical * 5f);

        // Move translation along the object's x and z axis
        transform.position += new Vector3(joystick.Horizontal * speed * Time.deltaTime, 0, joystick.Vertical * speed * Time.deltaTime);

        //Joystick not pressed, play player's Idle animation
        if (!joystick.Pressed)
        {
            walking = false;
            idle = true;
        }
        if (joystick.Pressed) //Joystick pressed, play player's Walking animation
        {
            walking = true;
            idle = false;
        }

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
    }
}

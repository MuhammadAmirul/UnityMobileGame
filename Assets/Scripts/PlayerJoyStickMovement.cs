using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoyStickMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;

    protected Joystick joystick;

	// Use this for initialization
	void Start ()
    {
        joystick = FindObjectOfType<Joystick>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //var rigidbody = GetComponent<Rigidbody>();

        //rigidbody.velocity = new Vector3(joystick.Horizontal * 5f, rigidbody.velocity.y, joystick.Vertical * 5f);

        // Move translation along the object's z-axis
        transform.position += new Vector3(joystick.Horizontal * speed * Time.deltaTime, 0, joystick.Vertical * speed * Time.deltaTime);

        // Rotate around our y-axis
    }
}

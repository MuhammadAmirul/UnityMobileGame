using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float drag = 0.5f;
    public float terminalRotationSpeed = 25.0f;

    private Rigidbody controller;

	// Use this for initialization
	void Start ()
    {
        controller = GetComponent<Rigidbody>();
        controller.maxAngularVelocity = terminalRotationSpeed;
        controller.drag = drag;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 dir = Vector3.zero;

        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        if(dir.magnitude > 1)
        {
            dir.Normalize();
        }

        controller.AddForce(dir * moveSpeed);
	}
}

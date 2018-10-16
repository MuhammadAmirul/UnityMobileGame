﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public Vector2 panLimit;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 pos = target.position;
        pos.y = transform.position.y;
        //  pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        //  pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        if (pos.x > 6.0f)
        {
            pos.x = 6.0f;
        }
        else if (pos.x < -6.0f)
        {
            pos.x = -6.0f;
        }

        if (pos.z > 1.0f)
        {
            pos.z = 1.0f;
        }
        else if (pos.z < -15f)
        {
            pos.z = -15.0f;
        }

        transform.position = pos + offset;
    }
}
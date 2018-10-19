using System.Collections;
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
        //  pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        //  pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        if (pos.x > 8.7f)
        {
            pos.x = 8.7f;
        }
        else if (pos.x < -6.7f)
        {
            pos.x = -6.7f;
        }

        if (pos.z > 18.0f)
        {
            pos.z = 18.0f;
        }
        else if (pos.z < -2.0f)
        {
            pos.z = -2.0f;
        }

        transform.position = pos + offset;
    }
}

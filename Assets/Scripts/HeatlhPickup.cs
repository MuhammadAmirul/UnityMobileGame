using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatlhPickup : MonoBehaviour
{
    // User inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private float respawn = 8f;
    public bool pickedUp;

    public Renderer rend;
    Collider healthcol;

    // Position storage variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        pickedUp = false;
        healthcol = GetComponent<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;

        if (pickedUp)
        {
            rend.enabled = false;
            healthcol.enabled = false;
            respawn -= Time.deltaTime;
            
            if(respawn <= 0f)
            {
                pickedUp = false;
                respawn = 8f;
            }
        }
        else
        {
            rend.enabled = true;
            healthcol.enabled = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && !pickedUp)
        {
            pickedUp = true;
        }
    }
}

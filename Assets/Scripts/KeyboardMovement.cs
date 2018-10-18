using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;

    public GameObject bullet;
    public Transform bulletSpawn;

    private float release;
    private float overheating;
    private bool overheat;

    public VirtualJoystick moveJoystick;

    // Use this for initialization
    void Start ()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float movement_z = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, movement_z);

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);

        overheating -= Time.deltaTime;

<<<<<<< HEAD
        if(moveJoystick.InputDirection != Vector3.zero)
        {
            
        }

=======
>>>>>>> 897770bbfae3dadb61fbe41db85d14a9e18c884e
        if (Input.GetKey(KeyCode.Space) && overheat == false)
        {
            release += Time.deltaTime;
            
            if (release >= 0.1f)
            {
                SpawnBullet();
                release = 0.0f;
                overheating += 0.4f;
            }
        }

        if (overheating < 0.0f)
        {
            overheating = 0.0f;
        }

        if (overheating >= 2.0f)
        {
            overheat = true;
        }

        if (overheating < 2.0f)
        {
            overheat = false;
        }
    }

    void SpawnBullet()
    {
        Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
    }
}

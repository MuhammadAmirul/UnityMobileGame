using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRotation : MonoBehaviour
{
    protected AttackRotationJoystick joystick;

    public GameObject bullet;
    public Transform bulletSpawn;

    private float release;
    private float overheating;
    private bool overheat;

    // Use this for initialization
    void Start ()
    {
        joystick = FindObjectOfType<AttackRotationJoystick>();
        overheat = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        overheating -= Time.deltaTime;

        if (joystick.Pressed && overheat == false)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackRotation : MonoBehaviour
{
    protected AttackRotationJoystick joystick;

    public GameObject bullet;
    public Transform bulletSpawn;

    private float release;
    private float overheating;
    private float maxOverheating = 2.0f;
    private bool overheat;

    public Animator animShoot;

    public Image overheatBar;

    // Use this for initialization
    void Start ()
    {
        joystick = FindObjectOfType<AttackRotationJoystick>();
        overheat = false;
        overheating = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        overheatBar.fillAmount = overheating / maxOverheating;

        overheating -= Time.deltaTime;

        if (joystick.Pressed && overheat == false)
        {
            release += Time.deltaTime;
            animShoot.Play("shooting");

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

        if (overheating >= maxOverheating)
        {
            overheat = true;
        }

        if (overheating < maxOverheating)
        {
            overheat = false;
        }
    }

    void SpawnBullet()
    {
        Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
    }
}

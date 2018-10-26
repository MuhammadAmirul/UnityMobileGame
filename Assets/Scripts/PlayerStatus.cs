using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public Image healthBar;
    public GameObject loseGame;

    public float health;
    public float maxHealth = 20f;
    private float dying;
    public static bool fullHealth;
    public static bool dead;

    public Animator anim;

    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start ()
    {
        health = maxHealth;
        loseGame.gameObject.SetActive(false);
        dead = false;
        dying = 0.0f;
    }

    void Update()
    {
        if (health == maxHealth)
        {
            fullHealth = true;
        }
        else if (health < maxHealth)
        {
            fullHealth = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        healthBar.fillAmount = health / maxHealth;

        if (health <= 0f)
        {
            dead = true;
            dying += Time.deltaTime;
            anim.Play("death");
            if (dying >= 1.2f)
            {
                gameObject.SetActive(false);
                loseGame.gameObject.SetActive(true);
            }
        }

        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Enemy Bullet") || col.gameObject.tag.Equals("Melee"))
        {
            health -= 1f;
            anim.SetTrigger("isHit");
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Health" && health < maxHealth)
        {
            health += 5f;
        }

        if (col.gameObject.tag == "Speed")
        {
            GetComponent<PlayerControllerV2>().boosted = true;
        }
    }
}

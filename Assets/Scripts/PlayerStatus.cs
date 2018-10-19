using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public Image healthbar;

    public float health;
    public float maxHealth = 100f;

    private float dying;

    public Animator anim;

	// Use this for initialization
	void Start ()
    {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        healthbar.fillAmount = health / maxHealth;

        if (health <= 0)
        {
            dying += Time.deltaTime;
            anim.Play("death");
            if(dying >= 0.8f)
            {
                Destroy(gameObject);
            }
            
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Enemy Bullet"))
        {
            health -= 5f;
        }
    }
}

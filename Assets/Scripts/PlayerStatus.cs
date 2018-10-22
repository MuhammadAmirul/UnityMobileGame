using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public Image healthbar;
    //public GameObject loseGame;

    public static float health;
    public float maxHealth = 100f;

    private float dying;

    public Animator anim;

    // Use this for initialization
    void Start ()
    {
        health = maxHealth;
        //loseGame.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        healthbar.fillAmount = health / maxHealth;

        if (health <= 0f)
        {
            dying += Time.deltaTime;
            anim.Play("death");
            if(dying >= 1.2f)
            {
                Destroy(gameObject);
                //loseGame.gameObject.SetActive(true);
            }
            
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Enemy Bullet"))
        {
            health -= 5f;
        }
        if (col.gameObject.tag.Equals("Melee"))
        {
            health -= 10f;
        }
    }
}

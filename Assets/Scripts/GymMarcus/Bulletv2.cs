using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletv2 : MonoBehaviour {


    public float speed;
    public float bulletDamage = 1f;
    public string bulletOwner = "Player";   //"Player" or "Enemy"
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (bulletOwner == "Player")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                //Debug.Log("Hit Enemy!");
                collision.gameObject.GetComponent<EnemyControl>().Damage(bulletDamage);
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerControl>().Damage(bulletDamage);
            }
        }
        Destroy(this.gameObject);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;
    public int bulletDamage = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ENEMY")
        {
            Debug.Log("Hit Enemy!");
            collision.gameObject.GetComponent<EnemyControl>().Damage(bulletDamage);
        }

        Destroy(this.gameObject);
        
    }
}

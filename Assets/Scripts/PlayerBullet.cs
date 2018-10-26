using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed;
    
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.SendMessage("Damage", SendMessageOptions.DontRequireReceiver);
        }

        Despawn();
    }

    public void Despawn()
    {
        Destroy(this.gameObject);
    }
}

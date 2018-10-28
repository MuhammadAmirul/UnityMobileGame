using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [HideInInspector] public UNITFACTION projectileOwner;
    [HideInInspector] public float projectileDamage;
    [HideInInspector] public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        projectileDamage = 1f;
    }


    public void Recycle()
    {
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "UNIT")
        {
            Unit colUnit = col.gameObject.GetComponent<Unit>();

            if(colUnit.faction != projectileOwner)
            {
                colUnit.ChangeHP(-projectileDamage);
                //Debug.Log("HIT");
                Recycle();
            }
        }
        else
        {
            Recycle();
        }
    }
}

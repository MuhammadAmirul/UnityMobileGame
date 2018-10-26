using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarPosition : MonoBehaviour
{
    public Transform target;
    //public Transform bigEnemy;

    public Image healthBar;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
        else
        {
            Vector2 firstsp = Camera.main.WorldToScreenPoint(target.position);
            this.transform.position = firstsp;
        }
        
        //Vector2 secondsp = Camera.main.WorldToScreenPoint(bigEnemy.position);
        //this.transform.position = secondsp;
    }

    public void UpdateHPValue(float val)
    {
        healthBar.fillAmount = val;
    }

    public void Despawn()
    {
        target = null;
        transform.position = new Vector3(1000f, 0f, 0f);
        //healthBar.fillAmount = 1f;
        this.gameObject.SetActive(false);
    }
}

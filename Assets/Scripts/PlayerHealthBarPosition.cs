using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarPosition : MonoBehaviour
{
    public Transform player;

    public Image healthBar;
    public Image overheatBar;

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector2 sp = Camera.main.WorldToScreenPoint(player.position);
        this.transform.position = sp;
    }
}

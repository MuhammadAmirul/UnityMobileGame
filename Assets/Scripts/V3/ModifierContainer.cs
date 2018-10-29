using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierContainer : MonoBehaviour {

    public float respawnRate = 5.0f;
    public Collider colModifier;

    public void RespawnModifier()
    {
        colModifier.gameObject.SetActive(false);
        Invoke("Respawn", respawnRate);
    }

    public void Respawn()
    {
        colModifier.gameObject.SetActive(true);
    }
}

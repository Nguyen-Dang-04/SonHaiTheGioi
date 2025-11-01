using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Samurai Samurai = other.GetComponent<Samurai>();
        if (Samurai != null)
        {
            Samurai.currentHealth = 0;             
        }
    }
}

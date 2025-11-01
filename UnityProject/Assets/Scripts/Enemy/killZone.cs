using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Samurai Samurai = other.GetComponent<Samurai>();
        if (Samurai != null)
        {
            Debug.Log("Samurai entered the KillZone!");
            Samurai.Die();

        }
    }
}

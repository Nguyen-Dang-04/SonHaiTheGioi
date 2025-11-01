using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public int damage = 20;
    private bool hasCausedDamage = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasCausedDamage)
        {
            Samurai samurai = collision.gameObject.GetComponent<Samurai>();
            if(samurai != null)
            {
                samurai.TakeDamage(damage, transform.position);
                hasCausedDamage = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasCausedDamage = false;
        }
    }
}

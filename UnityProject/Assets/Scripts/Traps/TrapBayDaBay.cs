using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBayDaBay : MonoBehaviour
{
    public int satThuong = 20;
    private bool daGayDamage = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !daGayDamage)
        {
            Samurai samurai = collision.GetComponent<Samurai>();
            if (samurai != null)
            {
                samurai.TakeDamage(satThuong, transform.position);
                daGayDamage = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            daGayDamage = false;
        }
    }
}

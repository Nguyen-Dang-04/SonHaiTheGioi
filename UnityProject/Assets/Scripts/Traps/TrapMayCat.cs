using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public float tocDo;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public int satThuong = 20;
    private bool daGayDamage = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = PointB;
    }

    void Update()
    {
        DiChuyen();
    }

    private void DiChuyen()
    {
        Vector2 huongDiChuyen;
        if (currentPoint == PointB)
        {
            huongDiChuyen = (PointB.position - transform.position).normalized;
            rb.linearVelocity = huongDiChuyen * tocDo;
        }
        else if (currentPoint == PointA)
        {
            huongDiChuyen = (PointA.position - transform.position).normalized;
            rb.linearVelocity = huongDiChuyen * tocDo;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f)
        {
            if (currentPoint == PointB)
            {
                currentPoint = PointA;
            }
            else
            {
                currentPoint = PointB;
            }
        }
    }

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


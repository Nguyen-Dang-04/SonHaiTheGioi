using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPopup : MonoBehaviour
{
    private int thoiGianTonTai = 2;
    private float lucDayLen = 2f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0, lucDayLen);
        }
        Destroy(gameObject, thoiGianTonTai);
    }
}

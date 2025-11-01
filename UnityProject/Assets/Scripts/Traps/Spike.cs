using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public Transform diemTanCong; 
    public float width = 2.0f; 
    public float height = 1.0f; 
    public LayerMask players; 
    public int satThuong = 10;
    private bool dealDamage = false;
    private bool dealtDamage = false;

    private void Update()
    {
        TanCong();
    }

    public void TanCong()
    {
        Collider2D Player = Physics2D.OverlapBox(diemTanCong.position, new Vector2(width, height), 0, players);

        if (Player != null && dealDamage == true && !dealtDamage)
        {
            Player.GetComponent<Samurai>().TakeDamage(satThuong, transform.position);
            dealtDamage = true;
        }
    }

    public void DealDamage()
    {
        dealDamage = true;
    }

    public void UneblaToDealDamage()
    {
        dealDamage = false;
        dealtDamage = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (diemTanCong != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(diemTanCong.position, new Vector3(width, height, 0));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanhDiaBat : MonoBehaviour
{
    private EnemyBat enemyBat;
    void Start()
    {
        if (enemyBat == null)
            enemyBat = GetComponentInChildren<EnemyBat>();
    }

    private void OnTriggerEnter2D(Collider2D vaCham)
    {
        if (vaCham.CompareTag("Player"))
        {
            enemyBat.daPhatHienPlayer = true;
        }
    }
}

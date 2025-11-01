using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanhDiaMusroom : MonoBehaviour
{
    private Musroom musroom;

    void Start()
    {
        musroom = GetComponentInChildren<Musroom>();
    }
    
    private void OnTriggerEnter2D(Collider2D vaCham)
    {
        if (vaCham.CompareTag("Player"))
        {
            musroom.daPhatHienPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D vaCham)
    {
        if (vaCham.CompareTag("Player"))
        {
            musroom.daPhatHienPlayer = false;
        }
    }
}

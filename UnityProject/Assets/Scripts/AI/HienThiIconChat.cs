using UnityEngine;

public class HienThiIconChat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(PlayerPrefs.GetInt("DataChat", 0) == 1);
    }
}

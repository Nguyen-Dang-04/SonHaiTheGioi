using UnityEngine;
using UnityEngine.UI;

public class TriggerZone : MonoBehaviour
{
    public GameObject panel;
    public GameObject NutBam;
    private bool isPlayerInZone = false;

    public GameObject ThongTin;
    public GameObject Setting;
    public GameObject AVT1;
    public GameObject AVT2;
    void Start()
    {
        panel.SetActive(false);
        NutBam.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            NutBam.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            NutBam.SetActive(false);
            if (panel != null)
                panel.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            panel.SetActive(!panel.activeSelf);
            ThongTin.SetActive(false);
            Setting.SetActive(false);
            AVT1.SetActive(true);
            AVT2.SetActive(false);
        }
    }
}

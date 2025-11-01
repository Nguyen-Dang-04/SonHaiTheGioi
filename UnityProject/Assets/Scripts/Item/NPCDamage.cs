using TMPro;
using UnityEngine;
using System.Collections;

public class NPCDamage : MonoBehaviour
{
    public GameObject panelUpgradeDamage;
    public GameObject NutBam;
    private bool isPlayerInZone = false;

    public string[] dialogues;
    public TextMeshPro dialogueText;
    public float displayInterval = 5f;

    public GameObject ThongTin;
    public GameObject Setting;
    public GameObject AVT1;
    public GameObject AVT2;

    void Start()
    {
        panelUpgradeDamage.SetActive(false);
        NutBam.SetActive(false);
        StartCoroutine(DisplayRandomDialogue());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPlayerInZone)
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
            if (panelUpgradeDamage != null)
                panelUpgradeDamage.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            panelUpgradeDamage.SetActive(!panelUpgradeDamage.activeSelf);
            ThongTin.SetActive(false);
            Setting.SetActive(false);
            AVT1.SetActive(true);
            AVT2.SetActive(false);
        }
    }

    IEnumerator DisplayRandomDialogue()
    {
        while (true)
        {
            if (dialogues.Length > 0)
            {
                int randomIndex = Random.Range(0, dialogues.Length);
                dialogueText.text = dialogues[randomIndex];
            }

            yield return new WaitForSeconds(3f);
            dialogueText.text = "";

            yield return new WaitForSeconds(displayInterval - 3f);
        }
    }
}

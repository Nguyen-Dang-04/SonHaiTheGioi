using TMPro;
using UnityEngine;
using System.Collections;

public class NPCMagic : MonoBehaviour
{
    public GameObject panelUpgradeDamageMagic;
    public GameObject NutBam;
    private bool isPlayerInZone = false;
    private bool npcUnlocked; // lưu trạng thái unlock

    public string[] dialogues;
    public TextMeshPro dialogueText;
    public float displayInterval = 5f;

    public GameObject ThongTin;
    public GameObject Setting;
    public GameObject AVT1;
    public GameObject AVT2;
    void Start()
    {
        npcUnlocked = PlayerPrefs.GetInt("UnlockNPC", 0) == 1;
        Debug.Log("Trạng thái UnlockNPC = " + npcUnlocked);

        panelUpgradeDamageMagic.SetActive(false);
        NutBam.SetActive(false);

        if (npcUnlocked)
        {
            StartCoroutine(DisplayRandomDialogue());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPlayerInZone)
        {
            isPlayerInZone = true;

            // chỉ hiện nút khi NPC đã unlock
            if (npcUnlocked)
                NutBam.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            NutBam.SetActive(false);
            if (panelUpgradeDamageMagic != null)
                panelUpgradeDamageMagic.SetActive(false);
        }
    }

    void Update()
    {
        // chỉ khi NPC unlock + player ở trong vùng + bấm E mới mở panel
        if (npcUnlocked && isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            panelUpgradeDamageMagic.SetActive(!panelUpgradeDamageMagic.activeSelf);

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

            yield return new WaitForSeconds(Mathf.Max(displayInterval - 3f, 0f));
        }
    }
}

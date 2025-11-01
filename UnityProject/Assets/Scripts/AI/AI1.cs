using UnityEngine;
using TMPro;
using System.Collections;

public class AI1 : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogueBox;
    public GameObject dialogueBox2;
    public TextMeshPro dialogueText;
    public TextMeshPro dialogueText2;
    [TextArea]
    public string[] messages = { "Xin chào, ta là NPC AI1!", "Trông ngươi có vẻ mệt mỏi, hãy nghỉ chút đi." };

    [Header("Extra Dialogue Sau Khi Nhấn E")]
    [TextArea]
    public string[] extraMessages = { "Ta có chuyện khác muốn nói với ngươi...", "Tạm biệt nhé, hẹn gặp lại!" };

    [Header("Player refs")]
    public Transform playerTf;
    private Coroutine dialogueCoroutine;
    private Collider2D npcCollider;

    public GameObject Button;
    public GameObject Button2;
    private bool isPlayerInZone = false;
    private bool opened = false;

    private Animator anim;

    private bool ThuPhucAI = false;
    private bool CoThuPhucAI = false;
    private bool daNoiChuyen = false;
    private bool dangnoichuyen = false;
    private bool dangnoichuyen2 = false;    

    public Vector2 size = new Vector2(5f, 3f);
    private LayerMask playerLayer;

    [SerializeField] AudioSource Source;
    public AudioClip BienMat;

    void Start()
    {
        int npcLevel = PlayerPrefs.GetInt("AI1_ThuPhuc", 0);

        // Bật/tắt chính object theo Level
        if (npcLevel == 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            return;
        }

        anim = GetComponent<Animator>();
        if (dialogueBox != null) dialogueBox.SetActive(false);
        if (dialogueBox2 != null) dialogueBox2.SetActive(false);
        npcCollider = GetComponent<Collider2D>();
        if (Button != null) Button.SetActive(false);
        if (Button2 != null) Button2.SetActive(false);
        playerLayer = LayerMask.GetMask("Player");
    }

    void Update()
    {
        if (!dangnoichuyen && isPlayerInZone && !opened && Input.GetKeyDown(KeyCode.E))
        {
            NoiChuyen();
        }
        if (!dangnoichuyen && !dangnoichuyen2 && CoThuPhucAI && !daNoiChuyen && Input.GetKeyDown(KeyCode.E))
        {
            Button2.SetActive(false);
            StartCoroutine(KetThucNoiChuyen());
        }
        RoiKhoiVungNoiChuyen();
        FaceToPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !dangnoichuyen && !ThuPhucAI)
        {
            if (Button != null) Button.SetActive(true);
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !ThuPhucAI && !dangnoichuyen)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            // Chỉ thoát nếu animation hiện tại KHÔNG phải "end"
            if (!stateInfo.IsName("end"))
            {
                if (Button != null) Button.SetActive(false);
                isPlayerInZone = false;
            }
        }
    }


    void FaceToPlayer()
    {
        if (playerTf == null) return;

        float dirX = playerTf.position.x - transform.position.x;
        if (Mathf.Abs(dirX) <= 0.01f) return;

        Vector3 s = transform.localScale;
        s.x = Mathf.Sign(dirX) * Mathf.Abs(s.x);
        transform.localScale = s;

        if (dialogueBox != null)
        {
            Vector3 uiS = dialogueBox.transform.localScale;
            uiS.x = Mathf.Sign(dirX) * Mathf.Abs(uiS.x);
            dialogueBox.transform.localScale = uiS;
        }

        if (dialogueBox2 != null)
        {
            Vector3 uiS = dialogueBox2.transform.localScale;
            uiS.x = Mathf.Sign(dirX) * Mathf.Abs(uiS.x);
            dialogueBox2.transform.localScale = uiS;
        }   

        if (Button != null)
        {
            Vector3 btnS = Button.transform.localScale;
            btnS.x = Mathf.Sign(dirX) * Mathf.Abs(btnS.x);
            Button.transform.localScale = btnS;
        }

        if (Button2 != null)
        {
            Vector3 btn2S = Button2.transform.localScale;
            btn2S.x = Mathf.Sign(dirX) * Mathf.Abs(btn2S.x);
            Button2.transform.localScale = btn2S;
        }
    }

    void NoiChuyen()
    {
        if (Button != null) Button.SetActive(false);
        opened = true;
        if (dialogueCoroutine != null) StopCoroutine(dialogueCoroutine);
        dialogueCoroutine = StartCoroutine(ShowExtraDialogueThenEnd());
    }

    IEnumerator ShowExtraDialogueThenEnd()
    {
        dangnoichuyen = true;
        dialogueBox.SetActive(true);

        for (int i = 0; i < messages.Length; i++)
        {
            dialogueText.text = messages[i];
            yield return new WaitForSeconds(2f);
        }
        dialogueBox.SetActive(false);
        ThuPhucAI = true;
        CoThuPhucAI = true;
        dangnoichuyen = false;
        if (Button2 != null) Button2.SetActive(true);
    }

    IEnumerator KetThucNoiChuyen()
    {
        dangnoichuyen2 = true;   
        dialogueBox2.SetActive(true);
        for (int i = 0; i < extraMessages.Length; i++)
        {
            dialogueText2.text = extraMessages[i];
            yield return new WaitForSeconds(2f);
        }
        dialogueBox2.SetActive(false);
        daNoiChuyen = true;
        anim.SetTrigger("end");        
    }

    private void RoiKhoiVungNoiChuyen()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position, size, 0f, playerLayer);
        if (hit != null)
        {
            return;
        }
        else
        {
            // 🛑 Dừng hội thoại đang chạy nếu có
            if (dialogueCoroutine != null)
            {
                StopCoroutine(dialogueCoroutine);
                dialogueCoroutine = null;
            }

            // Ẩn hộp thoại & reset trạng thái
            if (dialogueBox != null) dialogueBox.SetActive(false);
            if (Button2 != null) Button2.SetActive(false);
            opened = false;
            ThuPhucAI = false;
            CoThuPhucAI = false;
            dangnoichuyen = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);
    }

    public void SoudBienMat()
    {
        Source.PlayOneShot(BienMat);
    }

    public void Destroy()
    {
        PlayerPrefs.SetInt("AI1_ThuPhuc", 1);
        PlayerPrefs.SetInt("AI1_Level", 1);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }
}

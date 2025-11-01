using UnityEngine;
using TMPro;
using System.Collections;

public class AIHome1 : MonoBehaviour
{
    [Header("UI")]
    public GameObject boxText;
    public TextMeshPro text;
    public string[] messages;

    [Header("Player")]
    public Transform playerTf;
    public bool useSpriteFlip = true;

    private Collider2D cl2;
    private bool coTheNoiChuyen = false;
    private bool dangNoiChuyen = false;
    private SpriteRenderer sr;
    public GameObject button;

    [Header("Move Settings")]
    public Transform target;
    public float moveTime = 2f;

    // 🔒 ThuPhuc2 lock
    private bool lockThuPhuc2 = false;
    private Animator anim;

    void Start()
    {
        int npcLevel = PlayerPrefs.GetInt("AI1_Level", 0);

        if (npcLevel == 1 || npcLevel == 2 || npcLevel == 3)
            gameObject.SetActive(true);
        else
        {
            gameObject.SetActive(false);
            return;
        }

        cl2 = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (boxText != null) boxText.SetActive(false);
        if (playerTf == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTf = p.transform;
        }

        if (button != null) button.SetActive(false);

        // Khóa nếu đã hoàn thành
        lockThuPhuc2 = PlayerPrefs.GetInt("AI1_ThuPhuc", 0) != 1;
        if (lockThuPhuc2) LockInteraction();
    }

    void Update()
    {
        if (lockThuPhuc2) return; // 🔒

        // 🔹 NPC chỉ xoay theo player nếu KHÔNG đang di chuyển
        if (!isMoving)
            FaceToPlayer();

        if (button != null)
            button.SetActive(coTheNoiChuyen && !dangNoiChuyen);

        if (coTheNoiChuyen && !dangNoiChuyen && Input.GetKeyDown(KeyCode.E))
            StartCoroutine(NoiChuyen());
    }

    private void FaceToPlayer()
    {
        if (playerTf == null) return;

        float dirX = playerTf.position.x - transform.position.x;
        if (Mathf.Abs(dirX) <= 0.01f) return;

        if (useSpriteFlip && sr != null)
            sr.flipX = (dirX < 0);
        else
        {
            Vector3 s = transform.localScale;
            s.x = Mathf.Sign(dirX) * Mathf.Abs(s.x);
            transform.localScale = s;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (lockThuPhuc2) return;
        coTheNoiChuyen = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (lockThuPhuc2) return;
        coTheNoiChuyen = false;
    }

    IEnumerator NoiChuyen()
    {
        if (lockThuPhuc2) yield break;

        dangNoiChuyen = true;
        if (boxText != null) boxText.SetActive(true);
        if (button != null) button.SetActive(false);

        foreach (string message in messages)
        {
            text.text = message;
            yield return new WaitForSeconds(2f);
        }

        text.text = "";
        if (boxText != null) boxText.SetActive(false);
        dangNoiChuyen = false;

        if (!lockThuPhuc2 && button != null && coTheNoiChuyen)
            button.SetActive(true);
    }

    private void LockInteraction()
    {
        if (button != null) button.SetActive(false);
        if (boxText != null) boxText.SetActive(false);
        coTheNoiChuyen = false;
        dangNoiChuyen = false;
        if (cl2 != null) cl2.enabled = false;
    }

    // 👇 Thêm biến trạng thái di chuyển
    private bool isMoving = false;

    public void MoveObject()
    {
        if (target != null)
            StartCoroutine(MoveOverTime(target.position, moveTime));
    }

    private IEnumerator MoveOverTime(Vector3 targetPos, float duration)
    {
        isMoving = true;

        // 🔹 Khi di chuyển: luôn quay về bên trái
        if (useSpriteFlip && sr != null)
            sr.flipX = true;
        else
        {
            Vector3 s = transform.localScale;
            s.x = -Mathf.Abs(s.x); // luôn hướng trái
            transform.localScale = s;
        }

        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
        anim.SetTrigger("end");
    }

    private void TatObject()
    {
       Destroy(gameObject);
    }
}

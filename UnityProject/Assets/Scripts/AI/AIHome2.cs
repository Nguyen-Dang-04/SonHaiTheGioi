using UnityEngine;
using TMPro;
using System.Collections;

public class AIHome2 : MonoBehaviour
{
    [Header("UI")]
    public GameObject boxText;
    public TextMeshPro text;
    [TextArea]
    public string[] messages;   // Câu nói mặc định (cho Level 2)
    [TextArea]
    public string[] messages2;  // Câu nói khi Level = 3

    [Header("Player")]
    public Transform playerTf;
    public bool useSpriteFlip = true;

    private Collider2D cl2;
    private bool coTheNoiChuyen = false;
    private bool dangNoiChuyen = false;
    private SpriteRenderer sr;
    public GameObject button;

    [Header("Move Settings")]
    public Transform target;     // vị trí đích
    public float moveTime = 3f;  // thời gian di chuyển (giây)

    // trạng thái di chuyển
    private bool isMoving = false;
    private Animator anim;

    void Start()
    {
        // Đọc cấp độ
        int npcLevel = PlayerPrefs.GetInt("AI1_Level", 0);

        // 🔹 Bật/tắt NPC theo Level
        if (npcLevel == 2 || npcLevel == 3)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            return;
        }

        // 🔹 Nếu Level 3 → đổi sang messages2
        if (npcLevel == 3 && messages2 != null && messages2.Length > 0)
        {
            messages = messages2;
        }

        // Cache component
        cl2 = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (boxText != null) boxText.SetActive(false);

        // Tự tìm Player nếu chưa gán
        if (playerTf == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTf = p.transform;
        }

        // Ẩn nút ban đầu
        if (button != null) button.SetActive(false);
    }

    void Update()
    {
        // ❗ Chỉ xoay theo player khi KHÔNG di chuyển
        if (!isMoving)
            FaceToPlayer();

        // Hiển thị nút khi player trong vùng và chưa nói
        if (button != null && !isMoving)
            button.SetActive(coTheNoiChuyen && !dangNoiChuyen);

        if (coTheNoiChuyen && !dangNoiChuyen && !isMoving && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(NoiChuyen());
        }
    }

    private void FaceToPlayer()
    {
        if (playerTf == null) return;

        float dirX = playerTf.position.x - transform.position.x;
        if (Mathf.Abs(dirX) <= 0.01f) return;

        if (useSpriteFlip && sr != null)
        {
            sr.flipX = (dirX < 0);
        }
        else
        {
            Vector3 s = transform.localScale;
            s.x = Mathf.Sign(dirX) * Mathf.Abs(s.x);
            transform.localScale = s;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            coTheNoiChuyen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            coTheNoiChuyen = false;
        }
    }

    IEnumerator NoiChuyen()
    {
        dangNoiChuyen = true;
        if (boxText != null) boxText.SetActive(true);
        if (button != null) button.SetActive(false);  // ẩn khi đang nói chuyện

        foreach (string message in messages)
        {
            text.text = message;
            yield return new WaitForSeconds(2f);
        }

        text.text = "";
        if (boxText != null) boxText.SetActive(false);
        dangNoiChuyen = false;

        // Khi nói xong, nếu player vẫn trong vùng => hiện lại nút
        if (button != null && coTheNoiChuyen && !isMoving)
            button.SetActive(true);
    }

    public void MoveObject()
    {
        if (target != null)
            StartCoroutine(MoveOverTime(target.position, moveTime));
    }

    private IEnumerator MoveOverTime(Vector3 targetPos, float duration)
    {
        isMoving = true;
        button.SetActive(false); // ẩn nút khi di chuyển

        // 🔄 Khi di chuyển: LUÔN quay về bên trái
        if (useSpriteFlip && sr != null)
        {
            sr.flipX = true;
        }
        else
        {
            Vector3 s = transform.localScale;
            s.x = -Mathf.Abs(s.x); // scale.x âm -> nhìn trái
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

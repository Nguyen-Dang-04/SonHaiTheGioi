using UnityEngine;
using TMPro;
using System.Collections;

public class AI2 : MonoBehaviour
{
    [Header("UI")]
    public GameObject boxText;
    public TextMeshPro text;
    public string[] messages;

    [Header("Player")]
    public Transform playerTf;          // Kéo Player vào đây, hoặc để trống sẽ tự tìm theo tag "Player"
    public bool useSpriteFlip = true;   // true: dùng SpriteRenderer.flipX; false: dùng scale X

    private Collider2D cl2;
    private bool coTheNoiChuyen = false;
    private bool dangNoiChuyen = false;
    private SpriteRenderer sr;

    private Animator anim;
    public GameObject button;

    [SerializeField] AudioSource Source;
    public AudioClip BienMat;

    void Start()
    {
        int npcLevel = PlayerPrefs.GetInt("AI1_ThuPhuc", 0);

        // Bật/tắt chính object theo Level
        if (npcLevel == 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            return;
        }

        // Cache component
        cl2 = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (boxText != null)
            boxText.SetActive(false);

        // Tự tìm Player nếu chưa gán
        if (playerTf == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTf = p.transform;
        }

        // Ẩn nút ban đầu
        if (button != null)
            button.SetActive(false);
    }

    void Update()
    {
        FaceToPlayer();

        // 🔹 Hiển thị nút khi player trong vùng và chưa nói
        if (button != null)
            button.SetActive(coTheNoiChuyen && !dangNoiChuyen);

        if (coTheNoiChuyen && !dangNoiChuyen && Input.GetKeyDown(KeyCode.E))
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
        anim.SetTrigger("end");     
    }

    public void SoudBienMat()
    {
        Source.PlayOneShot(BienMat);
    }

    public void Destroy()
    {
        PlayerPrefs.SetInt("AI1_Level", 2);
        PlayerPrefs.SetInt("AI1_ThuPhuc", 2);
        PlayerPrefs.Save();
        dangNoiChuyen = false;
        Destroy(gameObject);
    }
}

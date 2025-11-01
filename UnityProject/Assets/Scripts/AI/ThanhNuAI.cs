using System.Collections;
using TMPro;
using UnityEngine;

public class ThanhNuAI : MonoBehaviour
{
    private bool coTheNoiChuyen = false;

    [Header("UI")]
    public GameObject button;
    public Transform tfbutton;
    public GameObject button2;
    public Transform tfbutton2;
    public GameObject textbox;
    public TextMeshPro text;
    public Transform tftext;
    public TextMeshPro text2;
    public Transform tftext2;

    [Header("Dialogues")]
    [TextArea] public string[] messages;
    [SerializeField] private GameObject BoxChatAI;

    [Header("State")]
    public bool dangNoiChuyen = false;
    private bool daNoiThoai1 = false;
    private bool dangNoiChuyen2 = false;

    [Header("Liên kết Player")]
    public Transform player;
    public GameObject datachat;

    // === NEW: lưu offset X gốc (dương) của các UI để đổi bên mà không lật chữ
    private float defBtnX, defBtn2X, defTextX, defText2X;

    // (tùy chọn) dùng SpriteRenderer.flipX lật sprite thay vì lật scale toàn bộ
    private SpriteRenderer sr;

    public GameObject ThongTin;
    public GameObject Setting;
    public GameObject AVT1;
    public GameObject AVT2;

    void Start()
    {
        // mở khóa NPC
        gameObject.SetActive(PlayerPrefs.GetInt("ThanhMauAI", 0) == 1);

        daNoiThoai1 = PlayerPrefs.GetInt("ThanhMauAI_Talked", 0) == 1;

        if (button != null) button.SetActive(false);
        if (button2 != null) button2.SetActive(false);
        if (textbox != null) textbox.SetActive(false);

        dangNoiChuyen = false;
        dangNoiChuyen2 = false;

        // Lưu offset X ban đầu (lấy trị tuyệt đối)
        if (tfbutton != null) defBtnX = Mathf.Abs(tfbutton.localPosition.x);
        if (tfbutton2 != null) defBtn2X = Mathf.Abs(tfbutton2.localPosition.x);
        if (tftext != null) defTextX = Mathf.Abs(tftext.localPosition.x);
        if (tftext2 != null) defText2X = Mathf.Abs(tftext2.localPosition.x);

        // Lấy SpriteRenderer nếu có để flipX (tránh lật cả con)
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Hiển thị đúng nút theo trạng thái
        if (button != null)
            button.SetActive(coTheNoiChuyen && !dangNoiChuyen && !daNoiThoai1);
        if (button2 != null)
            button2.SetActive(coTheNoiChuyen && !dangNoiChuyen && daNoiThoai1 && !dangNoiChuyen2);

        // E để nói chuyện
        if (coTheNoiChuyen && !dangNoiChuyen && !dangNoiChuyen2 && Input.GetKeyDown(KeyCode.E))
        {
            if (!daNoiThoai1) StartCoroutine(NoiChuyen1());
            else NoiChuyen2();
        }

        XoayVaCapNhatUI();
    }

    private IEnumerator NoiChuyen1()
    {
        dangNoiChuyen = true;
        if (textbox != null) textbox.SetActive(true);
        if (button != null) button.SetActive(false);

        foreach (var message in messages)
        {
            text.text = message;
            yield return new WaitForSeconds(2f);
        }

        datachat.SetActive(true);
        PlayerPrefs.SetInt("ThanhMauAI_Talked", 1);
        PlayerPrefs.SetInt("DataChat", 1);
        PlayerPrefs.Save();

        text.text = "";
        if (textbox != null) textbox.SetActive(false);

        dangNoiChuyen = false;
        daNoiThoai1 = true;

        if (coTheNoiChuyen && button2 != null)
            button2.SetActive(true);
    }

    private void NoiChuyen2()
    {
        ThongTin.SetActive(false);
        Setting.SetActive(false);
        AVT1.SetActive(true);
        AVT2.SetActive(false);

        dangNoiChuyen2 = true;
        if (textbox != null) textbox.SetActive(false);
        if (button2 != null) button2.SetActive(false);
        BoxChatAI.SetActive(true);
    }

    public void BatNoiChuyen2()
    {
        dangNoiChuyen2 = false;

        // 🔹 Xoá ngay câu trả lời AI khi người chơi đóng BoxChatAI
        if (text2 != null)
            text2.text = "";

        // 🔹 (tuỳ chọn) Ẩn luôn textbox nếu muốn khung biến mất cùng lúc
        if (textbox != null)
            textbox.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        coTheNoiChuyen = true;

        if (!daNoiThoai1)
        {
            if (button != null && !dangNoiChuyen) button.SetActive(true);
        }
        else
        {
            if (button2 != null && !dangNoiChuyen2) button2.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        coTheNoiChuyen = false;

        if (button != null) button.SetActive(false);
        if (button2 != null) button2.SetActive(false);
    }

    // === FIX CHÍNH Ở ĐÂY ===
    private void XoayVaCapNhatUI()
    {
        if (player == null) return;

        bool faceRight = player.position.x >= transform.position.x;

        // 1) Lật NPC một lần theo Player
        if (sr != null)
        {
            // chỉ lật sprite, không lật cả cây con => UI không bị ảnh hưởng scale
            sr.flipX = !faceRight;
        }
        else
        {
            // nếu không có SpriteRenderer, lật bằng scale của NPC
            var s = transform.localScale;
            s.x = Mathf.Abs(s.x) * (faceRight ? 1 : -1);
            transform.localScale = s;
        }

        // 2) Chuyển UI sang đúng bên, NHƯNG giữ chữ không bị mirror (localScale.x luôn dương)
        ApplyUI(tfbutton, defBtnX, faceRight);
        ApplyUI(tfbutton2, defBtn2X, faceRight);
        ApplyUI(tftext, defTextX, faceRight);
        ApplyUI(tftext2, defText2X, faceRight);
    }

    private void ApplyUI(Transform t, float defaultAbsX, bool faceRight)
    {
        if (t == null) return;

        // Giữ scale X dương để chữ không bị ngược
        var s = t.localScale;
        s.x = Mathf.Abs(s.x);
        t.localScale = s;

        // Đặt lại vị trí X sang bên mặt NPC
        var lp = t.localPosition;
        lp.x = faceRight ? defaultAbsX : -defaultAbsX;
        t.localPosition = lp;
    }

    // === HIỂN THỊ TRẢ LỜI AI TRONG text2 (mes2) ===
    public void ShowAIReply(string content, float visibleSeconds = 3f)
    {
        if (textbox != null) textbox.SetActive(true);  // hiện khung
        if (text2 != null) text2.text = content;      // đổ câu trả lời vào mes2
        StopCoroutine(nameof(HideAIReply));
        StartCoroutine(HideAIReply(visibleSeconds));
    }

    private IEnumerator HideAIReply(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (text2 != null) text2.text = "";
        // KHÔNG tắt textbox nếu bạn còn dùng cho hội thoại khác
        // nếu muốn tắt luôn khung: if (textbox != null) textbox.SetActive(false);
    }

}

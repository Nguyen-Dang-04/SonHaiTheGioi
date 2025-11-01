using UnityEngine;
using System.Collections;

public class ThapTrieuHoiAI : MonoBehaviour
{
    [Header("Điều kiện")]
    private bool coTheTapHopAI = false;   // Đủ điều kiện Level để tập hợp
    private bool tapHopAI = false;        // Player đang ở trong vùng
    private bool dangTapHop = false;      // Đang chạy nghi thức
    private bool daHoanThanh = false;     // 🔒 Đã tập hợp xong => khóa vĩnh viễn

    [Header("UI & Hiệu ứng")]
    public GameObject button;
    public GameObject thanhNuAI;
    public GameObject HieuUngTrieuHoi;

    [Header("Thành viên AI")]
    public AIHome1 ai1;
    public AIHome2 ai2;
    public AIHome3 ai3;

    private Animator anim;

    [SerializeField] AudioSource Source;
    public AudioClip BienMat;
    public AudioClip VaCham;
    public AudioClip TrieuHoi;

    [Header("Chat tạm (tuỳ chọn)")]
    public ChatOutputAutoHide outputAutoHide; // Kéo script này vào trong Inspector
    [SerializeField] private string sealedLine = "Nơi này hình như đang phong ấn thứ gì đó";
    [SerializeField] private float sealedLineDuration = 2f;

    private bool showingSealedMsg = false;

    private const string KEY_DONE = "ThapTrieuHoi_DaHoanThanh";

    void Start()
    {
        int npcLevel = PlayerPrefs.GetInt("AI1_Level", 0);
        coTheTapHopAI = (npcLevel == 3);
        daHoanThanh = PlayerPrefs.GetInt(KEY_DONE, 0) == 1;
        anim = GetComponent<Animator>();

        if (button != null) button.SetActive(false);
        if (daHoanThanh) coTheTapHopAI = false;
    }

    void Update()
    {
        // ✅ CHỈ hiện nút khi:
        // 1) Player trong vùng
        // 2) Không đang chạy nghi thức
        // 3) Chưa hoàn thành
        // 4) Và (AI1_Level == 0 hoặc đủ điều kiện == true)
        int npcLevel = PlayerPrefs.GetInt("AI1_Level", 0);
        bool allowButton = (npcLevel == 0) || coTheTapHopAI;

        bool showBtn = tapHopAI && allowButton && !dangTapHop && !daHoanThanh && !showingSealedMsg;
        if (button != null) button.SetActive(showBtn);

        if (!tapHopAI || dangTapHop || daHoanThanh) return;

        // Ấn E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (coTheTapHopAI)
            {
                // Level = 3 => nghi thức bình thường
                StartCoroutine(taphop());
            }
            else if (npcLevel == 0)
            {
                // Level = 0 => chỉ nói câu "phong ấn"
                if (!showingSealedMsg)
                    StartCoroutine(ShowSealedMessage());
            }
        }
    }

    private IEnumerator ShowSealedMessage()
    {
        showingSealedMsg = true;

        if (button != null) button.SetActive(false);

        // Hiện câu thoại tạm 2 giây
        if (outputAutoHide != null)
            outputAutoHide.ShowThenHide(sealedLine, sealedLineDuration);

        yield return new WaitForSeconds(sealedLineDuration);

        // Sau 2s nếu còn trong vùng thì hiện lại nút
        if (tapHopAI && !dangTapHop && !daHoanThanh && button != null)
            button.SetActive(true);

        showingSealedMsg = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (!daHoanThanh)
            tapHopAI = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        tapHopAI = false;
        if (button != null) button.SetActive(false);
    }

    private IEnumerator taphop()
    {
        dangTapHop = true;
        if (button != null) button.SetActive(false);

        if (ai2 != null) ai2.MoveObject();
        yield return new WaitForSeconds(3f);

        if (anim != null) anim.SetTrigger("nhap");
        if (Source != null && VaCham != null) Source.PlayOneShot(VaCham);
        if (ai1 != null) ai1.MoveObject();
        yield return new WaitForSeconds(2f);

        if (anim != null) anim.SetTrigger("nhap");
        if (Source != null && VaCham != null) Source.PlayOneShot(VaCham);
        if (ai3 != null) ai3.MoveObject();
        yield return new WaitForSeconds(3f);

        if (anim != null) anim.SetTrigger("end");
        if (Source != null && VaCham != null) Source.PlayOneShot(VaCham);      

        dangTapHop = false;
        daHoanThanh = true;
        coTheTapHopAI = false;
        tapHopAI = false;

        if (button != null) button.SetActive(false);

        PlayerPrefs.SetInt(KEY_DONE, 1);
        PlayerPrefs.SetInt("AI1_Level", 4);
        PlayerPrefs.Save();

        yield return new WaitForSeconds(2f);
        ThanhNuAIXuatHien();
    }

    public void ThanhNuAIXuatHien()
    {
        if (Source != null && TrieuHoi != null)
            Source.PlayOneShot(TrieuHoi);

        if (HieuUngTrieuHoi != null)
            HieuUngTrieuHoi.SetActive(true);

        if (thanhNuAI != null)
            thanhNuAI.SetActive(true);

        PlayerPrefs.SetInt("ThanhMauAI", 1);
        PlayerPrefs.Save();
    }

    public void AmThanhEnd()
    {
        if (Source != null && BienMat != null) Source.PlayOneShot(BienMat);
    }
}

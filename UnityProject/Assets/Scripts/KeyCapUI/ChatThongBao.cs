using UnityEngine;
using System.Collections;

public class ChatThongBao : MonoBehaviour
{
    public bool BatHuongDan;
    public ChatOutputAutoHide outputAutoHide; // Kéo vào trong Inspector
    [TextArea] public string cauThongBao1;
    [TextArea] public string cauThongBao2;

    private const string KEY_DA_THONG_BAO = "DaThongBao_SkillActive"; // Lưu lại để không hiện nhiều lần

    void Start()
    {
        // Đọc trạng thái skill
        BatHuongDan = PlayerPrefs.GetInt("ThongBaoSkill", 0) == 1;

        // Nếu skill đã bật và chưa từng thông báo
        if (BatHuongDan && PlayerPrefs.GetInt(KEY_DA_THONG_BAO, 0) == 0)
        {
            StartCoroutine(HienHaiCauChat());
            PlayerPrefs.SetInt(KEY_DA_THONG_BAO, 1);
            PlayerPrefs.Save();
        }
    }

    private IEnumerator HienHaiCauChat()
    {
        if (outputAutoHide != null)
        {
            outputAutoHide.ShowThenHide(cauThongBao1, 2f); // hiện câu đầu
            yield return new WaitForSeconds(2f);         // chờ một chút
            outputAutoHide.ShowThenHide(cauThongBao2, 2f); // hiện câu thứ hai
        }
        else
        {
            Debug.Log(cauThongBao1);
            yield return new WaitForSeconds(2f);
            Debug.Log(cauThongBao2);
        }
    }
}

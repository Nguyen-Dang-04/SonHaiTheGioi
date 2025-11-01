using UnityEngine;
using TMPro; // cần để dùng TextMeshProUGUI

public class UnlockSkill : MonoBehaviour
{
    public Skill skillObject;           // script Skill
    public TextMeshProUGUI statusText;  // Text hiển thị trạng thái

    private bool skillActive = false;

    public void ToggleSkill()
    {
        if (skillObject == null)
            return;

        skillActive = !skillActive;           // đảo trạng thái
        skillObject.skillActive = skillActive; // cập nhật trực tiếp vào script Skill

        // Cập nhật text
        if (statusText != null)
            statusText.text = skillActive ? "Loại bỏ" : "Trang bị";

        Debug.Log("Skill Active: " + skillActive);

        // Lưu trạng thái
        PlayerPrefs.SetInt("SkillActive", skillActive ? 1 : 0);
        PlayerPrefs.Save();
    }

    void Start()
    {
        skillActive = PlayerPrefs.GetInt("SkillActive", 0) == 1;

        if (skillObject != null)
            skillObject.skillActive = skillActive;

        // Thiết lập text lúc Start
        if (statusText != null)
            statusText.text = skillActive ? "Loại bỏ" : "Trang bị";
    }
}

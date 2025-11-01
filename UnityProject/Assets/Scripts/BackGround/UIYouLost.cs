using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIYouLost : MonoBehaviour
{
    private PlayerController gameController;
    public Slider thoiGianHoiSinhSlider;
    public TextMeshProUGUI thoiGianHoiSinhText;

    void Start()
    {
        gameController = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        if (gameController != null)
        {
            thoiGianHoiSinhSlider.value = gameController.thoiGianHoiSinhHienTai;
            thoiGianHoiSinhSlider.maxValue = gameController.thoiGianHoiSinh;

            thoiGianHoiSinhText.text = gameController.thoiGianHoiSinhHienTai.ToString("0");
        }
    }
}

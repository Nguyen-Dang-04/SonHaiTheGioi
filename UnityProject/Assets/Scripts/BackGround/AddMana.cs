using System.Globalization;
using TMPro;
using UnityEngine;

public class AddMana : MonoBehaviour
{
    public Samurai samurai;
    public CoinManager coinManager;
    private int baseCoin = 100;
    private float upgradeMultiplier = 1.2f;
    public TextMeshProUGUI textCoinMn;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("BaseCoinMana");
        // Load lại giá trị baseCoin nếu có lưu trước đó
        if (PlayerPrefs.HasKey("BaseCoinMana"))
        {
            baseCoin = PlayerPrefs.GetInt("BaseCoinMana");
        }

        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberGroupSeparator = ".";
        nfi.NumberGroupSizes = new int[] { 3 };
        textCoinMn.text = baseCoin.ToString("N0", nfi);
    }

    public void Upgrade()
    {
        if (coinManager != null && baseCoin <= coinManager.CoinCount)
        {
            coinManager.CoinCount -= baseCoin;
            baseCoin = Mathf.RoundToInt(baseCoin * upgradeMultiplier);

            // Cập nhật text hiển thị
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3 };
            textCoinMn.text = baseCoin.ToString("N0", nfi);

            // Lưu baseCoin mới sau mỗi lần nâng cấp
            PlayerPrefs.SetInt("BaseCoinMana", baseCoin);
            PlayerPrefs.Save();

            AddManaToPlayer();
        }
    }

    private void AddManaToPlayer()
    {
        samurai.maxMana += 10;
        Samurai.doiTuong.SavePlayer();
        CoinManager.doiTuong.Savecoin();
    }
}

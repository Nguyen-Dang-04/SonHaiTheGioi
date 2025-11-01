using System.Globalization;
using TMPro;
using UnityEngine;

public class TextHP : MonoBehaviour
{
    public Samurai samurai;
    public CoinManager coinManager;
    private int baseCoin = 100;
    private float upgradeMultiplier = 1.2f;
    public TextMeshProUGUI textCoinHP;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("BaseCoinHP");
        // Nếu đã lưu baseCoin trước đó, thì load ra
        if (PlayerPrefs.HasKey("BaseCoinHP"))
        {
            baseCoin = PlayerPrefs.GetInt("BaseCoinHP");
        }

        // Định dạng hiển thị coin
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberGroupSeparator = ".";
        nfi.NumberGroupSizes = new int[] { 3 };
        textCoinHP.text = baseCoin.ToString("N0", nfi);
    }

    public void Upgrade()
    {
        if (coinManager != null && baseCoin <= coinManager.CoinCount)
        {
            coinManager.CoinCount -= baseCoin;
            baseCoin = Mathf.RoundToInt(baseCoin * upgradeMultiplier);

            // Cập nhật lại UI
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3 };
            textCoinHP.text = baseCoin.ToString("N0", nfi);

            // Lưu baseCoin mới vào PlayerPrefs
            PlayerPrefs.SetInt("BaseCoinHP", baseCoin);
            PlayerPrefs.Save();

            AddHealth();
        }
    }

    private void AddHealth()
    {
        samurai.maxHealth += 10;
        Samurai.doiTuong.SavePlayer();
        CoinManager.doiTuong.Savecoin();
    }
}
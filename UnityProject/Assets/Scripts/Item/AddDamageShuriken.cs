using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class AddDamageShuriken : MonoBehaviour
{
    public Samurai samurai;
    public CoinManager coinManager;
    private int baseCoin = 100;
    private float upgradeMultiplier = 1.4f;
    public TextMeshProUGUI textCoinShuriken;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("BaseCoinShuriken");
        // Load lại baseCoin nếu đã lưu trước đó
        if (PlayerPrefs.HasKey("BaseCoinShuriken"))
        {
            baseCoin = PlayerPrefs.GetInt("BaseCoinShuriken");
        }

        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberGroupSeparator = ".";
        nfi.NumberGroupSizes = new int[] { 3 };
        textCoinShuriken.text = baseCoin.ToString("N0", nfi);
    }

    public void Upgrade()
    {
        if (coinManager != null && baseCoin <= coinManager.CoinCount)
        {
            coinManager.CoinCount -= baseCoin;
            baseCoin = Mathf.RoundToInt(baseCoin * upgradeMultiplier);

            // Cập nhật UI
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3 };
            textCoinShuriken.text = baseCoin.ToString("N0", nfi);

            // Lưu lại baseCoin
            PlayerPrefs.SetInt("BaseCoinShuriken", baseCoin);
            PlayerPrefs.Save();

            Adddmage();
        }
    }

    private void Adddmage()
    {
        samurai.maxDamageShuriken += 3;
        samurai.minDamageShuriken += 3;
        samurai.manaThorow += 5;
        Samurai.doiTuong.SavePlayer();
        CoinManager.doiTuong.Savecoin();
    }
}

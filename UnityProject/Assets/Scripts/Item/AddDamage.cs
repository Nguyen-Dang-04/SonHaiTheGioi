using System.Globalization;
using TMPro;
using UnityEngine;

public class AddDamage : MonoBehaviour
{
    public Samurai samurai;
    public CoinManager coinManager;
    private int baseCoin = 100;
    private float upgradeMultiplier = 1.4f;
    public TextMeshProUGUI textCoinDamage;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("BaseCoinDamage");
        if (PlayerPrefs.HasKey("BaseCoinDamage"))
        {
            baseCoin = PlayerPrefs.GetInt("BaseCoinDamage");
        }

        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberGroupSeparator = ".";
        nfi.NumberGroupSizes = new int[] { 3 };
        textCoinDamage.text = baseCoin.ToString("N0", nfi);
    }

    public void Upgrade()
    {
        if (coinManager != null && baseCoin <= coinManager.CoinCount)
        {
            coinManager.CoinCount -= baseCoin;
            baseCoin = Mathf.RoundToInt(baseCoin * upgradeMultiplier);

            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3 };
            textCoinDamage.text = baseCoin.ToString("N0", nfi);

            PlayerPrefs.SetInt("BaseCoinDamage", baseCoin);
            PlayerPrefs.Save();

            AddDamageValue();
        }
    }

    private void AddDamageValue()
    {
        samurai.maxDamage += 3;
        samurai.minDamage += 3;
        Samurai.doiTuong.SavePlayer();
        CoinManager.doiTuong.Savecoin();
    }
}
using System.Globalization;
using TMPro;
using UnityEngine;

public class AddDamageDashAttack : MonoBehaviour
{
    public Samurai samurai;
    public CoinManager coinManager;
    private int baseCoin = 100;
    private float upgradeMultiplier = 1.4f;
    public TextMeshProUGUI textCoinDamageDashAttack;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("BaseCoinDamage");
        if (PlayerPrefs.HasKey("BaseCoinDamageDashAttack"))
        {
            baseCoin = PlayerPrefs.GetInt("BaseCoinDamageDashAttack");
        }

        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberGroupSeparator = ".";
        nfi.NumberGroupSizes = new int[] { 3 };
        textCoinDamageDashAttack.text = baseCoin.ToString("N0", nfi);
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
            textCoinDamageDashAttack.text = baseCoin.ToString("N0", nfi);

            PlayerPrefs.SetInt("BaseCoinDamageDashAttack", baseCoin);
            PlayerPrefs.Save();

            AddDamageValue();
        }
    }

    private void AddDamageValue()
    {
        samurai.maxDamageDashAttack += 3;
        samurai.minDamageDashAttack += 3;
        samurai.manaDashAttack += 5;
        Samurai.doiTuong.SavePlayer();
        CoinManager.doiTuong.Savecoin();
    }
}

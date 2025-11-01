using System.Globalization;
using TMPro;
using UnityEngine;

public class TextDefend : MonoBehaviour
{
    public Samurai samurai;
    public CoinManager coinManager;
    private int baseCoin = 200;
    private float upgradeMultiplier = 1.5f;
    public TextMeshProUGUI textCoinDef;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("BaseCoinDefend");
        // Dùng đúng key cho hệ thống phòng thủ
        if (PlayerPrefs.HasKey("BaseCoinDefend"))
        {
            baseCoin = PlayerPrefs.GetInt("BaseCoinDefend");
        }

        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberGroupSeparator = ".";
        nfi.NumberGroupSizes = new int[] { 3 };
        textCoinDef.text = baseCoin.ToString("N0", nfi);
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
            textCoinDef.text = baseCoin.ToString("N0", nfi);

            // Lưu baseCoin mới sau mỗi lần nâng cấp
            PlayerPrefs.SetInt("BaseCoinDefend", baseCoin);
            PlayerPrefs.Save();

            AddDefend();
        }
    }

    public void AddDefend()
    {
        samurai.maxDef += 1;
        Samurai.doiTuong.SavePlayer();
        CoinManager.doiTuong.Savecoin();
    }
}

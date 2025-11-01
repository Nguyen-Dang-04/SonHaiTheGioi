using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int CoinCount;
    public static CoinManager doiTuong;
    private void Awake()
    {
        doiTuong = this;
        SaveCoinSystem.CreateSaveFile(); // đảm bảo có file save
        LoadCoin();
    }

    public void Savecoin()
    {
        SaveCoinSystem.SaveCoin(this);
    }
    public void LoadCoin()
    {
        CoinData data = SaveCoinSystem.LoadCoin();
        CoinCount = data.coin;
    }
}

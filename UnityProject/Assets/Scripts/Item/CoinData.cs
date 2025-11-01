[System.Serializable]
public class CoinData
{
    public int coin;

    // constructor từ CoinManager (dùng khi save)
    public CoinData(CoinManager coinManager)
    {
        coin = coinManager.CoinCount;
    }

    // constructor mặc định (dùng khi tạo file mới)
    public CoinData()
    {
        coin = 100;
    }
}

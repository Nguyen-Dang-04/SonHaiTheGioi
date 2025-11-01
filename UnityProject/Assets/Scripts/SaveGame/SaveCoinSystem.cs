using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveCoinSystem
{
    public static void SaveCoin(CoinManager coin)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/coin.tobi";
        FileStream stream = new FileStream(path, FileMode.Create);

        CoinData data = new CoinData(coin);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static CoinData LoadCoin()
    {
        string path = Application.persistentDataPath + "/coin.tobi";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CoinData data = formatter.Deserialize(stream) as CoinData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    // ✅ Hàm tạo file mới mặc định (coin = 0)
    public static void CreateSaveFile()
    {
        string path = Application.persistentDataPath + "/coin.tobi";
        if (!File.Exists(path)) // chỉ tạo khi chưa có file
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            CoinData data = new CoinData(); // constructor mặc định coin = 0

            formatter.Serialize(stream, data);
            stream.Close();

            Debug.Log("New save file created at: " + path);
        }
        else
        {
            Debug.LogWarning("Save file already exists at: " + path);
        }
    }
}

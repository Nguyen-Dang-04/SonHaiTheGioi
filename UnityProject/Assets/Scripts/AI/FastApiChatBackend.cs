using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using System;

public class FastApiChatBackend : MonoBehaviour
{
    [Header("Server")]
    [Tooltip("VD: http://127.0.0.1:8000/chat")]
    public string serverUrl = "http://127.0.0.1:8000/chat";
    public string gameName = "My Unity Game";

    [Serializable] class ChatIn { public string message; public string game_name; }
    [Serializable] class ChatOut { public string reply; }

    public void Send(string userMsg, Action<string> onReply, Action<string> onError = null)
    {
        StartCoroutine(SendCo(userMsg, onReply, onError));
    }

    IEnumerator SendCo(string msg, Action<string> onReply, Action<string> onError)
    {
        var payload = JsonUtility.ToJson(new ChatIn { message = msg, game_name = gameName });
        var req = new UnityWebRequest(serverUrl, "POST");
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(payload));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            onError?.Invoke(req.error);
            yield break;
        }

        try
        {
            var data = JsonUtility.FromJson<ChatOut>(req.downloadHandler.text);
            onReply?.Invoke(data.reply);
        }
        catch
        {
            onError?.Invoke("Parse response failed: " + req.downloadHandler.text);
        }
    }
}

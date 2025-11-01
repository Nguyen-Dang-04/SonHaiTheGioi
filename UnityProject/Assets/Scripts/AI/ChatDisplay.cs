using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChatDisplay : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform container;
    public ScrollRect scrollRect;

    [Header("Prefabs")]
    public GameObject aiMessagePrefab;
    public GameObject playerMessagePrefab;

    [Header("Settings")]
    public int maxMessages = 100;

    // 🟢 Hàng đợi tin chưa hiển thị (khi panel đang ẩn)
    private readonly Queue<(string sender, string msg)> pending = new();

    void OnEnable()
    {
        // Khi panel bật lại, hiển thị các tin còn trong hàng đợi
        while (pending.Count > 0)
        {
            var (sender, message) = pending.Dequeue();
            InternalAdd(sender, message, false);
        }

        // Cuộn xuống cuối cùng sau khi hiển thị
        if (isActiveAndEnabled)
            StartCoroutine(ScrollToBottomNextFrame());
    }

    public void AddMessage(string sender, string message)
    {
        if (container == null || string.IsNullOrEmpty(message)) return;

        // Nếu panel đang tắt, lưu tin nhắn vào hàng đợi
        if (!gameObject.activeInHierarchy || !isActiveAndEnabled)
        {
            pending.Enqueue((sender, message));
            return;
        }

        InternalAdd(sender, message, true);
    }

    private void InternalAdd(string sender, string message, bool doScroll)
    {
        GameObject prefab = sender == "AI" ? aiMessagePrefab :
                            sender == "Player" ? playerMessagePrefab : null;
        if (prefab == null)
        {
            Debug.LogWarning("Sender không hợp lệ (phải là 'AI' hoặc 'Player').");
            return;
        }

        GameObject msgObj = Instantiate(prefab, container);
        var text = msgObj.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null) text.text = message;

        if (maxMessages > 0 && container.childCount > maxMessages)
            Destroy(container.GetChild(0).gameObject);

        if (doScroll && isActiveAndEnabled)
            StartCoroutine(ScrollToBottomNextFrame());
    }

    private IEnumerator ScrollToBottomNextFrame()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(container);
        yield return null;
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 0f;
    }
}

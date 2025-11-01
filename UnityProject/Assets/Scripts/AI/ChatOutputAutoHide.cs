using System.Collections;
using TMPro;
using UnityEngine;

public class ChatOutputAutoHide : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text output;               // KÉO TMP_Text (ô hiển thị) vào đây
    public float defaultDuration = 3f;    // thời gian mặc định để ẩn

    private Coroutine hideRoutine;

    /// <summary>
    /// Hiển thị text rồi tự ẩn sau 'duration' giây.
    /// Script này chạy trên GameObject KHÁC với BoxChatAI, nên không bị tắt giữa chừng.
    /// </summary>
    public void ShowThenHide(string text, float duration = -1f)
    {
        if (output == null) return;

        // dừng ẩn cũ (nếu có), sau đó hiển thị text mới
        if (hideRoutine != null) StopCoroutine(hideRoutine);
        output.text = text;

        // đảm bảo GameObject chứa output vẫn đang bật để render
        if (!gameObject.activeSelf) gameObject.SetActive(true);

        float d = duration > 0f ? duration : defaultDuration;
        hideRoutine = StartCoroutine(HideAfterDelay(d));
    }

    private IEnumerator HideAfterDelay(float d)
    {
        yield return new WaitForSeconds(d);
        if (output != null) output.text = string.Empty;
        hideRoutine = null;
    }
}

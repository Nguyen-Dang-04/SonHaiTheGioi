using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BoxChatAI : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField inputField;
    public ChatOutputAutoHide outputAutoHide;

    private TMP_Text placeholderText;

    // Chỉ chặn gửi khi đang chờ AI
    private bool isWaitingReply = false;

    private string readyPlaceholder = "Bạn có thể tiếp tục nói chuyện rồi!";
    private string thinkingPlaceholder = "Huyền Nguyệt đang suy nghĩ…";

    [Header("Liên kết NPC / Player (tuỳ dự án)")]
    public Samurai samurai;
    public ThanhNuAI thanhNuAI;

    private Coroutine closeRoutine;
    private bool IsCooldown => isWaitingReply;   // ✅ chỉ chặn khi AI đang nghĩ

    [Header("AI Backend")]
    public FastApiChatBackend backend;

    [Header("Lưu lịch sử hội thoại")]
    public ChatDisplay chatDisplay;

    // === Cách 1: đóng trì hoãn ===
    private bool pendingClose = false;

    void Start()
    {
        if (inputField != null && inputField.placeholder != null)
            placeholderText = inputField.placeholder.GetComponent<TMP_Text>();

        if (placeholderText != null)
            placeholderText.text = string.Empty;

        inputField.onSelect.AddListener(OnInputSelected);
        inputField.onDeselect.AddListener(OnInputDeselected);
        inputField.onSubmit.AddListener(OnMessageSubmit);
        inputField.onValueChanged.AddListener(OnTextChanged);
        inputField.onValidateInput += FilterWhileCooldown;

        StartCoroutine(FocusNextFrame());
    }

    void OnEnable()
    {
        if (samurai != null) samurai.TamDungDiChuyen = true;
        StartCoroutine(FocusNextFrame());
    }

    void OnDisable()
    {
        if (samurai != null) samurai.TamDungDiChuyen = false;
    }

    void OnDestroy()
    {
        if (inputField != null)
        {
            inputField.onSelect.RemoveListener(OnInputSelected);
            inputField.onDeselect.RemoveListener(OnInputDeselected);
            inputField.onSubmit.RemoveListener(OnMessageSubmit);
            inputField.onValueChanged.RemoveListener(OnTextChanged);
            inputField.onValidateInput -= FilterWhileCooldown;
        }
        if (samurai != null) samurai.TamDungDiChuyen = false;
    }

    private IEnumerator FocusNextFrame()
    {
        yield return null;
        if (inputField != null)
        {
            inputField.Select();
            inputField.ActivateInputField();
            inputField.caretPosition = inputField.text?.Length ?? 0;
        }
    }

    void OnInputSelected(string _)
    {
        if (placeholderText != null)
            placeholderText.text = isWaitingReply ? thinkingPlaceholder : string.Empty;

        CancelCloseRoutine();
        // người dùng quay lại focus => ý định đóng trước đó không còn
        pendingClose = false;
    }

    void OnInputDeselected(string _)
    {
        // Nếu đang chờ AI, không đóng ngay: chỉ ghi nhớ ý định đóng
        if (isWaitingReply)
        {
            pendingClose = true;

            if (placeholderText != null && string.IsNullOrEmpty(inputField.text))
                placeholderText.text = thinkingPlaceholder;
            return;
        }

        if (placeholderText != null && string.IsNullOrEmpty(inputField.text))
            placeholderText.text = string.Empty;

        if (closeRoutine == null)
            closeRoutine = StartCoroutine(TurnOffChatBoxAfterDelay(0.05f));
    }

    void OnMessageSubmit(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        if (isWaitingReply) return;

        // ✅ Lưu tin nhắn người chơi vào log
        chatDisplay?.AddMessage("Player", text);

        isWaitingReply = true;

        outputAutoHide?.ShowThenHide(text, 2f);

        if (backend != null)
            backend.Send(text, OnAIReply, OnAIError);

        inputField.SetTextWithoutNotify(string.Empty);
        inputField.ActivateInputField();
        inputField.caretPosition = 0;

        if (placeholderText != null)
            placeholderText.text = thinkingPlaceholder;
    }

    private IEnumerator TurnOffChatBoxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Nếu đã lấy lại focus hoặc đang chờ AI thì không đóng
        if ((inputField != null && inputField.isFocused) || isWaitingReply)
        {
            closeRoutine = null;
            yield break;
        }

        if (samurai != null) samurai.TamDungDiChuyen = false;

        gameObject.SetActive(false);

        if (thanhNuAI != null) thanhNuAI.BatNoiChuyen2();

        closeRoutine = null;
    }

    void OnTextChanged(string newText)
    {
        if (!isWaitingReply) return;

        if (!string.IsNullOrEmpty(newText))
        {
            inputField.SetTextWithoutNotify(string.Empty);
            if (placeholderText != null)
                placeholderText.text = thinkingPlaceholder;
        }
    }

    private char FilterWhileCooldown(string currentText, int charIndex, char addedChar)
    {
        return isWaitingReply ? '\0' : addedChar;
    }

    private void CancelCloseRoutine()
    {
        if (closeRoutine != null)
        {
            StopCoroutine(closeRoutine);
            closeRoutine = null;
        }
    }

    // ==== CALLBACK TỪ BACKEND ====
    private void OnAIReply(string ai)
    {
        CancelCloseRoutine();

        chatDisplay?.AddMessage("AI", ai);   // ✅ log trả lời AI

        // Không hiển thị ở bubble — chỉ gửi cho NPC
        thanhNuAI?.ShowAIReply(ai, 5f);

        StartCoroutine(ReplyCooldownAfterAI());

        // Nếu người dùng đã click ra ngoài khi đang nghĩ và ô hiện không còn focus → tự đóng
        if (pendingClose && (inputField == null || !inputField.isFocused))
        {
            if (closeRoutine == null)
                closeRoutine = StartCoroutine(TurnOffChatBoxAfterDelay(0.05f));
        }

        pendingClose = false;
    }

    private void OnAIError(string err)
    {
        CancelCloseRoutine();

        // 🎭 Các câu fallback ngẫu nhiên khi mất mạng hoặc server lỗi
        string[] fallbackReplies = new string[]
        {
          "Ờm... giờ ta hơi mệt, huynh cho ta tĩnh tâm chút nhé~",
          "Linh khí quanh đây hơi loạn, ta chưa nghe rõ lời huynh ♡",
          "Ma pháp đang dao động, huynh chờ ta ổn định linh lực chút nha~",
          "Ta đang bế quan hấp thụ linh khí, lát nữa nói chuyện tiếp nhé ♫",
        };

        string msg = fallbackReplies[UnityEngine.Random.Range(0, fallbackReplies.Length)];

        chatDisplay?.AddMessage("AI", msg);
        thanhNuAI?.ShowAIReply(msg, 5f);

        StartCoroutine(ReplyCooldownAfterAI());

        if (pendingClose && (inputField == null || !inputField.isFocused))
        {
            if (closeRoutine == null)
                closeRoutine = StartCoroutine(TurnOffChatBoxAfterDelay(0.05f));
        }

        pendingClose = false;
    }

    private IEnumerator ReplyCooldownAfterAI()
    {
        isWaitingReply = true;

        if (placeholderText != null)
            placeholderText.text = "Huyền Nguyệt đang trả lời…";

        yield return new WaitForSeconds(5f);

        isWaitingReply = false;

        if (placeholderText != null)
            placeholderText.text = readyPlaceholder;
    }

}

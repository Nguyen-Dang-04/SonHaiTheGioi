using System.Collections;
using UnityEngine;
using TMPro;

public class TextEndGame : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    private string fullText = "Bản demo của Samurai's Revenge kết thúc tại đây.\n\n" +
                            "Trong tương lai, Ken sẽ tiếp tục phát triển thêm nhiều tính năng hấp dẫn và cải thiện trải nghiệm chơi game tổng thể. Hy vọng bạn sẽ thích và tiếp tục ủng hộ trò chơi này.\n\n" +
                            "Báo cáo vấn đề và hỗ trợ tại Discord:\n" +
                            "Theo dõi Tiktok để cập nhật mới nhất:";
    public float typingSpeed = 0.05f;

    private string currentText = " ";

    public GameObject buttonDiscord; // Nút Discord
    public GameObject buttonTiktok;
    public GameObject buttonMenu;   // Nút Menu

    [SerializeField] AudioSource audioSource;

    private bool hasShownDiscordButton = false; // Để kiểm tra hiện Discord nút
    private bool hasShownTiktokdButton = false; 

    void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText += fullText[i];
            textDisplay.text = currentText;

            // Khi gõ đến đoạn Discord, hiện button Discord
            if (!hasShownDiscordButton && currentText.Contains("Báo cáo vấn đề và hỗ trợ tại Discord:"))
            {
                hasShownDiscordButton = true;
                ShowDiscordButton();
            }

            if (!hasShownTiktokdButton && currentText.Contains("Theo dõi Tiktok để cập nhật mới nhất:"))
            {
                hasShownTiktokdButton = true; 
                ShowTiktokButton();
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        // Khi gõ xong toàn bộ thì hiện button Menu
        ShowMenuButton();
    }

    void ShowDiscordButton()
    {
        buttonDiscord.SetActive(true);
    }

    void ShowTiktokButton()
    {
        buttonTiktok.SetActive(true);
    }

    void ShowMenuButton()
    {
        audioSource.Stop();
        buttonMenu.SetActive(true);
    }
}

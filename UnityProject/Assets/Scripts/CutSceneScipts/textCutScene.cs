using System.Collections;
using TMPro;
using UnityEngine;

public class textCutScene : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    private string fullText = "Một pháp sư quyền năng đã tạm thời ngăn chặn lũ ma thú, tạo cơ hội quý giá để bạn hành động. Nhiệm vụ của bạn là tiến vào các cánh cổng, tiêu diệt những ma thú còn sót lại, và tìm cách nhận được sự trợ giúp từ pháp sư ấy để đánh bại ma thần, chấm dứt mối đe dọa vĩnh viễn.";
    public float typingSpeed = 0.05f;

    private string currentText = " ";

    public GameObject buttonMenu;

    [SerializeField] AudioSource audioSource;

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
            yield return new WaitForSeconds(typingSpeed);
        }
        ShowMenu();
    }

    void ShowMenu()
    {
        audioSource.Stop();
        buttonMenu.SetActive(true);
    }
}

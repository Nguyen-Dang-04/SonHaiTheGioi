using UnityEngine;
using System.Collections;

public class EnableAfterDelay : MonoBehaviour
{
    public GameObject CutScene;       // Đối tượng muốn bật
    public GameObject anphimbatki;        // UI hiển thị "ấn phím bất kỳ"
    public StartScene startScene;
    public bool TatCutScene = false;

    void Start()
    {
        TatCutScene = PlayerPrefs.GetInt("TatCutStartScene", 0) == 1;

        if (!TatCutScene)
        {
            StartCoroutine(BatSauDelay());
            StartCoroutine(DuocPhepInput());
        }
        else
        {
            StartCoroutine(KoPhatCutScene());
        }
    }

    IEnumerator BatSauDelay()
    {
        yield return new WaitForSeconds(2f);

        if (CutScene != null)
        {
            CutScene.SetActive(true);
        }
    }

    IEnumerator DuocPhepInput()
    {
        yield return new WaitForSeconds(6.666667f);

        if (anphimbatki != null)
        {
            anphimbatki.SetActive(true); 
        }

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        PlayerPrefs.SetInt("TatCutStartScene", 1);
        PlayerPrefs.Save();
        anphimbatki.SetActive(false);
        CutScene.SetActive(false);
        startScene.TatVatLieu();
    }

    IEnumerator KoPhatCutScene()
    {
        anphimbatki.SetActive(false);
        CutScene.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        startScene.TatVatLieu();
    }
}

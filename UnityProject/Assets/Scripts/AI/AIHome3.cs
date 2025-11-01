using UnityEngine;
using System.Collections;

public class AIHome3 : MonoBehaviour
{

    public Transform target;     // vị trí đích
    public float moveTime = 3f;  // thời gian di chuyển (giây)
    private Animator anim;

    void Start()
    {
        // Đọc cấp độ
        int npcLevel = PlayerPrefs.GetInt("AI1_Level", 0);

        // Bật/tắt chính object theo Level
        if (npcLevel == 3)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            return;
        }

        anim = GetComponent<Animator>();
    }

    public void MoveObject()
    {
        if (target != null)
            StartCoroutine(MoveOverTime(target.position, moveTime));
    }

    private IEnumerator MoveOverTime(Vector3 targetPos, float duration)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Di chuyển mượt
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null; // đợi frame kế tiếp
        }

        transform.position = targetPos; // đảm bảo đến đúng vị trí
        anim.SetTrigger("end");
    }

    private void TatObject()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using UnityEngine;

public class SummonTheBoss : MonoBehaviour
{
    private skeleton2 ske;
    public GameObject boss;
    public GameObject effect;
    private bool hasBeenSummoned = false;
    private bool theEffectIsEnabled = false;
    private BossWitch bossWitch;

    public GameObject CanhBao;

    void Start()
    {
        ske = GetComponent<skeleton2>();

        GameObject boss = GameObject.Find("Witch");
        if (boss != null)
        {
            bossWitch = boss.GetComponent<BossWitch>();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy đối tượng với tên Witch.");
        }
    }

    void Update()
    {
        if (!hasBeenSummoned && !theEffectIsEnabled && bossWitch.revive)
        {
            StartCoroutine(InstantiateEffectAndBoss());
        }

    }

    private IEnumerator InstantiateEffectAndBoss()
    {

        Vector3 PosCanhBao = transform.position + new Vector3(0, 2, 0);
        Instantiate(CanhBao, PosCanhBao, transform.rotation);

        theEffectIsEnabled = true;
        yield return new WaitForSeconds(1f);
        Vector3 newPosition = transform.position + new Vector3(0, 0.3f, 0);
        Instantiate(effect, newPosition, transform.rotation);
        yield return new WaitForSeconds(1.3f);
        if (ske.isFacingRight)
        {
            Instantiate(boss, transform.position, transform.rotation);
        }
        else 
        {
            Instantiate(boss, transform.position, transform.rotation);
            BossSkeleton.doiTuong.Flip();
        }
        Destroy(gameObject);
        hasBeenSummoned = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Earth : MonoBehaviour
{
    private Animator anim;
    public GameObject diemTanCong;
    public float radius;
    public int satThuong;
    public LayerMask enemies;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DestroyAfterAnimation());
    }

    public void TanCong()
    {
        Collider2D[] Enemy = Physics2D.OverlapCircleAll(diemTanCong.transform.position, radius, enemies);

        foreach (Collider2D EnemyGameobject in Enemy)
        {
            Debug.Log("Hit enemy");
            EnemyGameobject.GetComponent<EnemySkeleton>().heal -= satThuong;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(diemTanCong.transform.position, radius);
    }
    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}

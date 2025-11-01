using System.Collections;
using UnityEngine;

public class LightningSpell : MonoBehaviour
{
    private Animator anim;
    public GameObject pointAttack;
    public float radius;
    public int damage;
    public LayerMask player;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(DestroyAfterAnimation());
    }

    public void Attack()
    {
        Collider2D Player = Physics2D.OverlapCircle(pointAttack.transform.position, radius, player);

        if (Player != null)
        {
            Player.GetComponent<Samurai>().TakeDamage(damage, transform.position);
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointAttack.transform.position, radius);
    }
}

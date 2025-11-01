using System.Collections;
using UnityEngine;

public class Effect_Wind : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    public GameObject pointAttack;
    public float radius;
    public int damage;
    private bool dealtDamage;
    public LayerMask player;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dealtDamage = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            anim.SetTrigger("isEffect");
            /*anim.SetTrigger("isEffect2");*/
            Attack();
            rb.linearVelocity = Vector2.zero;
            StartCoroutine(DestroyAfterAnimation());
            Destroy(gameObject, 30f);
        }
    }

    public void Attack()
    {
        Collider2D Player = Physics2D.OverlapCircle(pointAttack.transform.position, radius, player);

        if (Player != null && !dealtDamage)
        {
            Player.GetComponent<Samurai>().TakeDamage(damage, transform.position);
            dealtDamage = true;
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

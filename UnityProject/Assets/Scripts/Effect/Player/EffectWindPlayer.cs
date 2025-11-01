using UnityEngine;
using System.Collections;

public class EffectWindPlayer : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    public GameObject pointAttack;
    public float radius;
    public int damage;
    private bool dealtDamage;
    public LayerMask enemy;

    private bool hasHitEnemy = false; // 🟢 Kiểm tra xem đã va chạm enemy chưa

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dealtDamage = false;

        // 🔥 Lấy damegeMagic từ PlayerData
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            damage = data.damegeMagic; // Lấy từ file save
        }
        else
        {
            damage = 1; // Giá trị mặc định nếu chưa có save
        }

        // 🕒 Nếu sau 1 giây không trúng enemy thì tự hủy
        StartCoroutine(AutoDestroyIfNoHit());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            hasHitEnemy = true; // ✅ Đánh dấu là đã trúng enemy
            anim.SetTrigger("isEffect");
            Attack();
            rb.linearVelocity = Vector2.zero; // Dừng lại
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    public void Attack()
    {
        Collider2D Enemy = Physics2D.OverlapCircle(pointAttack.transform.position, radius, enemy);

        if (Enemy != null && !dealtDamage)
        {
            Enemy.GetComponent<IEnemy>().TakeDamage(damage);
            dealtDamage = true;
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    // 🕒 Coroutine tự xóa nếu không va chạm enemy sau 1 giây
    private IEnumerator AutoDestroyIfNoHit()
    {
        yield return new WaitForSeconds(1f);

        if (!hasHitEnemy) // Nếu sau 1 giây mà chưa trúng enemy
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pointAttack.transform.position, radius);
    }
}

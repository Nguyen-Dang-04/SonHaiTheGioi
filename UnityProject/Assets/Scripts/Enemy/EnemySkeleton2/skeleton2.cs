using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class skeleton2 : MonoBehaviour, IEnemy
{
    [Header("Patrol")]
    public Transform PointA;
    public Transform PointB;
    private Transform CurrentPoint;
    private Rigidbody2D rb;
    public float speed = 2f;
    private Animator anim;
    public bool isFacingRight = true;

    [Header("Attack")]
    public GameObject pointAttack;
    public float radius = 1f;
    private bool attacking = false;                 // cờ từ Animation Event
    public LayerMask player;                        // layer của Player
    public int damage = 1;

    [Header("Pursuit")]
    public bool Pursuit = false;
    public Transform playerTransform;

    [Header("Limit Zone (Guard Area)")]
    public Transform limitPoint;    // biên trái/phải (1)
    public Transform limitPoint2;   // biên trái/phải (2)

    [Header("Health")]
    public int maxHealth = 10;
    public int currentHeal;
    public bool isDead = false;
    public float Health
    {
        get { return currentHeal; }
        set { currentHeal = (int)value; }
    }

    [Header("UI & Effects")]
    public TMP_Text popUptext;          // text hiển thị damage (tham chiếu nếu dùng)
    public GameObject popUpDamage;      // prefab popup damage
    public HealthBar healthBar;
    public GameObject active;

    [Header("Drop Rates (0..1) & Counts")]
    [SerializeField] private float dropRateHeart = 0.4f;
    [SerializeField] private int maxHeart = 2;
    [SerializeField] private float dropRateMana = 0.3f;
    [SerializeField] private int maxMana = 2;
    [SerializeField] private float dropRateCoin = 0.8f;
    [SerializeField] private int maxCoin = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        CurrentPoint = PointB;
        currentHeal = maxHealth;
        if (healthBar != null) healthBar.SetMaxHealth(maxHealth);

        // Nếu thiếu playerTransform, cố gắng tìm theo tag (không bắt buộc)
        if (playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
        }
    }

    void Update()
    {
        // 1) Ép giới hạn ngay đầu frame (nếu có vùng gác)
        EnforceLimits();

        // 2) Kiểm tra tấn công/đuổi nhưng CHỈ trong vùng gác (nếu có)
        CheckAttackWithinBounds();

        // 3) Nếu đang ở state Hit/Attack1 → đứng yên
        if (anim != null && (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") ||
                             anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 4) Di chuyển
        if (Pursuit)
        {
            PursuePlayer();
            // Không cần CheckLimitPoint kiểu cũ nữa (đã có EnforceLimits)
        }
        else
        {
            MoveToCurrentPoint();
            CheckSwitchPoint();
        }
    }

    // ========== Patrol ==========
    void MoveToCurrentPoint()
    {
        if (CurrentPoint == null) return;

        Vector2 dir = (CurrentPoint.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);
        if (anim != null) anim.SetBool("isWalk", true);

        if ((dir.x > 0 && !isFacingRight) || (dir.x < 0 && isFacingRight))
            Flip();
    }

    void CheckSwitchPoint()
    {
        if (CurrentPoint == null) return;

        if (Vector2.Distance(transform.position, CurrentPoint.position) < 0.1f)
        {
            CurrentPoint = (CurrentPoint == PointA) ? PointB : PointA;
            Flip();
        }
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector2 loacalScale = transform.localScale;
        loacalScale.x *= -1;
        transform.localScale = loacalScale;

        Vector2 healthBarScale = healthBar.transform.localScale;
        healthBarScale.x *= -1;
        healthBar.transform.localScale = healthBarScale;
    }

    // ========== Attack / Pursuit ==========
    void CheckAttackWithinBounds()
    {
        // Fallback: nếu KHÔNG có limitPoint/limitPoint2 → dùng logic cũ
        if (limitPoint == null || limitPoint2 == null)
        {
            CheckAttack_FallbackNoLimits();
            return;
        }

        // Nếu bản thân ra ngoài vùng → tắt đuổi/đánh
        if (!IsInsideGuardZone(transform.position))
        {
            Pursuit = false;
            if (anim != null) anim.SetBool("isAttack", false);
            return;
        }

        // Nếu player chưa gán → tắt attack
        if (playerTransform == null)
        {
            if (anim != null) anim.SetBool("isAttack", false);
            return;
        }

        // Nếu player ở ngoài vùng → tắt đuổi/đánh
        if (!IsInsideGuardZone(playerTransform.position))
        {
            Pursuit = false;
            if (anim != null) anim.SetBool("isAttack", false);
            return;
        }

        // Kiểm tra phạm vi tấn công
        if (pointAttack == null)
        {
            if (anim != null) anim.SetBool("isAttack", false);
            return;
        }

        Collider2D[] playerInRange = Physics2D.OverlapCircleAll(pointAttack.transform.position, radius, player);
        bool canAttack = playerInRange.Length > 0;

        if (anim != null) anim.SetBool("isAttack", canAttack);

        // Chỉ bật Pursuit khi vẫn ở trong vùng gác
        if (canAttack) Pursuit = true;
    }

    // Dùng khi không có limitPoint/limitPoint2 (giữ nguyên behavior cũ)
    void CheckAttack_FallbackNoLimits()
    {
        if (pointAttack == null)
        {
            if (anim != null) anim.SetBool("isAttack", false);
            return;
        }

        Collider2D[] playerInRange = Physics2D.OverlapCircleAll(pointAttack.transform.position, radius, player);
        bool canAttack = playerInRange.Length > 0;

        if (anim != null) anim.SetBool("isAttack", canAttack);
        if (canAttack) Pursuit = true;
    }

    public void Attack()
    {
        if (pointAttack == null) return;

        Collider2D PlayerCol = Physics2D.OverlapCircle(pointAttack.transform.position, radius, player);
        if (PlayerCol != null)
        {
            Samurai s = PlayerCol.GetComponent<Samurai>();
            if (s != null) s.TakeDamage(damage, transform.position);
        }
    }

    public void BatDauTanCong() { attacking = true; }
    public void KetThucTanCong() { attacking = false; }

    void PursuePlayer()
    {
        // Nếu player chết/không có player → đứng yên
        if (playerTransform == null || Samurai.doiTuong == null || Samurai.doiTuong.isDead)
        {
            rb.linearVelocity = Vector2.zero;
            if (anim != null) anim.SetBool("isWalk", false);
            return;
        }

        // Nếu có vùng gác: chỉ đuổi khi cả 2 ở trong vùng
        if (limitPoint != null && limitPoint2 != null)
        {
            if (!IsInsideGuardZone(transform.position) || !IsInsideGuardZone(playerTransform.position))
            {
                rb.linearVelocity = Vector2.zero;
                if (anim != null)
                {
                    anim.SetBool("isWalk", false);
                    anim.SetBool("isAttack", false);
                }
                Pursuit = false;
                return;
            }

            // Dự đoán bước tiếp theo, nếu vượt biên thì dừng
            Vector2 dirPred = (playerTransform.position - transform.position).normalized;
            float left, right; GetBounds(out left, out right);
            float nextX = transform.position.x + dirPred.x * speed * Time.deltaTime;
            if (nextX < left || nextX > right)
            {
                rb.linearVelocity = Vector2.zero;
                if (anim != null)
                {
                    anim.SetBool("isWalk", false);
                    anim.SetBool("isAttack", false);
                }
                Pursuit = false;
                Flip();
                return;
            }
        }

        // Đuổi
        Vector2 dir = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);
        if (anim != null) anim.SetBool("isWalk", true);

        if ((dir.x > 0 && !isFacingRight) || (dir.x < 0 && isFacingRight))
            Flip();
    }

    // ========== Damage / Death ==========
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (attacking)
        {
            // Đang vung đòn thì không trúng (theo thiết kế hiện tại)
            // Nếu muốn giảm damage thay vì miễn nhiễm, đổi tại đây.
        }
        else
        {
            // Popup damage: nên set text trên instance, tránh share ref
            if (popUpDamage != null)
            {
                var inst = Instantiate(popUpDamage, transform.position + Vector3.up * 2f, Quaternion.identity);
                var tmp = inst.GetComponentInChildren<TMP_Text>();
                if (tmp != null) tmp.text = damage.ToString();
            }
            else if (popUptext != null)
            {
                popUptext.text = damage.ToString();
            }

            currentHeal -= damage;
            if (healthBar != null) healthBar.SetHealth(currentHeal);
            if (anim != null) anim.SetTrigger("Attacked");
        }

        if (currentHeal <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        if (currentHeal <= 0 && !isDead)
        {
            currentHeal = 0;
            DropItems();

            isDead = true;
            if (anim != null) anim.SetTrigger("isDead");
            if (active != null) active.SetActive(false);

            playerTransform = null;

            var col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            var body = GetComponent<Rigidbody2D>();
            if (body != null) body.simulated = false;

            Destroy(gameObject, 30f);
        }
    }

    // ========== Guard Area Helpers ==========
    // Trả về left/right theo X từ 2 limitPoint
    void GetBounds(out float left, out float right)
    {
        float a = limitPoint.position.x;
        float b = limitPoint2.position.x;
        left = Mathf.Min(a, b);
        right = Mathf.Max(a, b);
    }

    bool IsInsideGuardZone(Vector2 pos)
    {
        if (limitPoint == null || limitPoint2 == null) return true; // nếu không có limit, coi như luôn đúng
        float left, right; GetBounds(out left, out right);
        return pos.x >= left && pos.x <= right;
    }

    // Ép enemy không vượt biên: clamp vị trí + tắt đuổi/đánh + dừng velocity nếu cần
    void EnforceLimits()
    {
        if (limitPoint == null || limitPoint2 == null) return;

        float left, right; GetBounds(out left, out right);
        float x = transform.position.x;

        if (x < left)
        {
            transform.position = new Vector3(left, transform.position.y, transform.position.z);
            rb.linearVelocity = Vector2.zero;
            Pursuit = false;
            if (anim != null) anim.SetBool("isAttack", false);
            Flip();
        }
        else if (x > right)
        {
            transform.position = new Vector3(right, transform.position.y, transform.position.z);
            rb.linearVelocity = Vector2.zero;
            Pursuit = false;
            if (anim != null) anim.SetBool("isAttack", false);
            Flip();
        }
    }

    // ========== Gizmos ==========
    private void OnDrawGizmos()
    {
        if (pointAttack != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pointAttack.transform.position, radius);
        }

        Gizmos.color = Color.cyan;
        if (limitPoint != null) Gizmos.DrawWireSphere(limitPoint.position, 0.2f);
        if (limitPoint2 != null) Gizmos.DrawWireSphere(limitPoint2.position, 0.2f);
    }

    // ========== Drops ==========
    void DropItems()
    {
        TryDrop("Heart", dropRateHeart, maxHeart);
        TryDrop("Mana", dropRateMana, maxMana);
        TryDrop("Coin", dropRateCoin, maxCoin);
    }

    void TryDrop(string itemName, float dropRate, int maxCount)
    {
        if (Random.value < dropRate)
        {
            int count = Random.Range(1, maxCount + 1);
            for (int i = 0; i < count; i++)
            {
                GameObject item = ObjectPoolManager.Instance != null
                    ? ObjectPoolManager.Instance.GetObjectFromPool(itemName)
                    : null;

                if (item != null)
                {
                    item.transform.position = transform.position;

                    Rigidbody2D body = item.GetComponent<Rigidbody2D>();
                    if (body != null)
                    {
                        body.AddForce(new Vector2(Random.Range(-1f, 1f),
                                                  Random.Range(3f, 6f)),
                                                  ForceMode2D.Impulse);
                    }
                }
            }
        }
    }
}

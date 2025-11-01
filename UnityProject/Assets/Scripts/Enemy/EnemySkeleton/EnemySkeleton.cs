using System.Collections;
using UnityEngine;
using TMPro;

public class EnemySkeleton : MonoBehaviour, IEnemy
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;
    private bool isWaiting = false;

    public int heal;
    public int currentHeal;
    private bool isDead = false;

    private bool dangTanCong = false;
    public int satThuong;
    public GameObject diemTanCong;
    public LayerMask players;
    public float radius;

    public TMP_Text popUptext;
    public GameObject popUpDamage;

    public HealthBar healthBar;
    public GameObject active;

    [SerializeField] private float dropRateHeart;   // 40% rơi Heart
    [SerializeField] private int maxHeart;             // rơi tối đa 2 bình máu

    [SerializeField] private float dropRateMana;    // 30% rơi Mana
    [SerializeField] private int maxMana;              // rơi tối đa 2 bình mana

    [SerializeField] private float dropRateCoin;    // 80% rơi Coin
    [SerializeField] private int maxCoin;              // rơi tối đa 5 xu

    public float Health 
    {
        get { return currentHeal; }
        set { currentHeal = (int)value; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isWalk", true);
        currentHeal = heal;
        healthBar.SetMaxHealth(heal);
    }

    void Update()
    {
        if (!isWaiting)
        {
            DiChuyen();
        }

        CheckAttack();
    }

    void DiChuyen()
    {
        if (isWaiting || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f)
        {
            StartCoroutine(PauseBeforeSwitching());
        }
    }

    void CheckAttack()
    {
        Collider2D[] playersInRange = Physics2D.OverlapCircleAll(diemTanCong.transform.position, radius, players);

        if (playersInRange.Length > 0)
        {
            anim.SetBool("isAttack", true);
        }
        else
        {
            anim.SetBool("isAttack", false);
        }
    }
    public void BatDauTanCong()
    {
        dangTanCong = true;
    }
    public void KetThucTanCong()
    {
        dangTanCong = false;
    }
    public void TanCong()
    {
        Collider2D Player = Physics2D.OverlapCircle(diemTanCong.transform.position, radius, players);

        if (Player != null)
        {
            Player.GetComponent<Samurai>().TakeDamage(satThuong, transform.position);

        }
    }

    public void TakeDamage(int damage)
    {
        if(dangTanCong)
        {
            currentHeal -= 0;
        }
        else
        {
            popUptext.text = damage.ToString();
            currentHeal -= damage;
            healthBar.SetHealth(currentHeal);
            anim.SetTrigger("Attacked");
            GameObject newObject = Instantiate(popUpDamage, transform.position, transform.rotation);
            newObject.transform.position += new Vector3(0, 2f, 0);
        }

        if (currentHeal <= 0 && !isDead)
        {
            Heal();
        }
    }

    void Heal()
    {
        if (currentHeal <= 0 && !isDead)
        {
            DropItems();
            isDead = true;
            anim.SetBool("isDead", true);
            active.SetActive(false);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;

            Destroy(gameObject, 30);
        }
    }

    private IEnumerator PauseBeforeSwitching()
    {
        isWaiting = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isWalk", false);
        yield return new WaitForSeconds(0);

        if (currentPoint == pointB.transform)
        {
            Flip();
            currentPoint = pointA.transform;
        }
        else
        {
            Flip();
            currentPoint = pointB.transform;
        }

        anim.SetBool("isWalk", true);
        isWaiting = false;
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

        Vector3 healthBarScale = healthBar.transform.localScale;
        healthBarScale.x *= -1;
        healthBar.transform.localScale = healthBarScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(diemTanCong.transform.position, radius);
    }

    void DropItems()
    {
        TryDrop("Heart", dropRateHeart, maxHeart);
        TryDrop("Mana", dropRateMana, maxMana);
        TryDrop("Coin", dropRateCoin, maxCoin);
    }

    void TryDrop(string itemName, float dropRate, int maxCount)
    {
        // Chỉ rơi nếu Random.value < tỉ lệ rơi
        if (Random.value < dropRate)
        {
            int count = Random.Range(1, maxCount + 1); // rơi từ 1 đến maxCount vật phẩm
            for (int i = 0; i < count; i++)
            {
                GameObject item = ObjectPoolManager.Instance.GetObjectFromPool(itemName);
                if (item != null)
                {
                    item.transform.position = transform.position;
                    // 🎇 (Tuỳ chọn) Thêm hiệu ứng bay lên nhẹ
                    Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(3f, 6f)), ForceMode2D.Impulse);
                    }
                }
            }
        }
    }
}

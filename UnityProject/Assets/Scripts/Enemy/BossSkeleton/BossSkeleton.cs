using TMPro;
using UnityEngine;
using System.Collections;

public class BossSkeleton : MonoBehaviour, IEnemy
{
    private Rigidbody2D rb;
    public float speed;
    private Animator anim;

    public GameObject pointAttack;
    public float radius;
    private bool attacking = false;
    public LayerMask player;
    public int damage;
    public float timeSkill1;
    private float currentTimeSkill1;
    private bool isOnCoolDown = false;

    public GameObject effectSkill1;
    public Transform pointEffect;
    public float speedEffect1;

    public Transform playerTransform;
    private bool isFacingRight = true;

    private bool isBoosted = false;

    public int maxHealth;
    public int currentHeal;
    public bool isDead = false;
    public float Health
    {
        get { return currentHeal; }
        set { currentHeal = (int)value; }
    }

    public TMP_Text popUptext;
    public GameObject popUpDamageBoss;

    public HealthBar healthBar;
    public GameObject active;

    public static BossSkeleton doiTuong;

    public GameObject unlockPrefab; // gán prefab trong Inspector

    [SerializeField] private float dropRateCoin;    // 80% rơi Coin
    [SerializeField] private int maxCoin;

    void Awake()
    {
        doiTuong = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHeal = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            return;
        }
    }

    void Update()
    {
        CheckAttack();

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Rivive") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            PursuePlayer();
        }

        if (isOnCoolDown)
        {
            currentTimeSkill1 -= Time.deltaTime;
            if (currentTimeSkill1 < 0)
            {
                currentTimeSkill1 = 0;
                isOnCoolDown = false;
            }
        }

        if (currentHeal < maxHealth * 0.5f && !isDead && !isBoosted)
        {
            isBoosted = true;
            StartCoroutine(LowHealthBoost());
        }

    }

    void CheckAttack()
    {
        Collider2D[] playerInRange = Physics2D.OverlapCircleAll(pointAttack.transform.position, radius, player);

        if (playerInRange.Length > 0)
        {
            anim.SetBool("isAttack2", true);
        }
        else if (!isOnCoolDown && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            anim.SetBool("isAttack1", true);
            currentTimeSkill1 = timeSkill1;
            isOnCoolDown = true;
        }
        else
        {
            anim.SetBool("isAttack2", false);
            anim.SetBool("isAttack1", false);
        }
    }
    public void Attack()
    {
        Collider2D Player = Physics2D.OverlapCircle(pointAttack.transform.position, radius, player);

        if (Player != null)
        {
            Player.GetComponent<Samurai>().TakeDamage(damage, transform.position);
        }
    }

    void PursuePlayer()
    {
        if (playerTransform != null && Samurai.doiTuong.isDead == false)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
            anim.SetBool("isWalk", true);
            if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
            {
                Flip();
            }
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            anim.SetBool("isWalk", false);
        }
    }

    public void BatDauTanCong()
    {
        attacking = true;
    }
    public void KetThucTanCong()
    {
        attacking = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointAttack.transform.position, radius);
    }

    public void TakeDamage(int damage)
    {
        if (attacking || anim.GetCurrentAnimatorStateInfo(0).IsName("Rivive"))
        {
            currentHeal -= 0;
        }
        else
        {
            popUptext.text = damage.ToString();
            currentHeal -= damage;
            healthBar.SetHealth(currentHeal);
            anim.SetTrigger("Attacked");
            GameObject newObject = Instantiate(popUpDamageBoss, transform.position, transform.rotation);
            newObject.transform.position += new Vector3(0, 2f, 0);
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

            if (PlayerPrefs.GetInt("UnlockSpawned", 0) == 0)
            {
                Instantiate(unlockPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
            }

            isDead = true;
            BossWitch.doiTuong.stopSpawning = true;
            anim.SetTrigger("isDie");
            active.SetActive(false);
            playerTransform = null;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            StartCoroutine(BossWitch.doiTuong.Hide());
            Destroy(gameObject, 30);
        }
    }

    public void EffectSkill1()
    {
        var Effect = Instantiate(effectSkill1, pointEffect.position, pointEffect.rotation);

        if (isFacingRight)
        {
            Effect.GetComponent<Rigidbody2D>().linearVelocity = pointEffect.right * speedEffect1;
            Effect.transform.localScale = new Vector3(Mathf.Abs(Effect.transform.localScale.x), Effect.transform.localScale.y, Effect.transform.localScale.z);
        }
        else
        {
            Effect.GetComponent<Rigidbody2D>().linearVelocity = -pointEffect.right * speedEffect1;
            Effect.transform.localScale = new Vector3(-Mathf.Abs(Effect.transform.localScale.x), Effect.transform.localScale.y, Effect.transform.localScale.z);
        }
    }

    IEnumerator LowHealthBoost()
    {
        speed *= 1.5f;
        timeSkill1 /= 2f;
        speedEffect1 *= 2f;
        anim.speed *= 1.5f;
        yield return new WaitForSeconds(20f);
        speed /= 1.5f;
        timeSkill1 *= 1.5f;
        speedEffect1 /= 1.5f;
        anim.speed /= 1.5f;
    }

    void DropItems()
    {
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

using TMPro;
using UnityEngine;

public class Musroom : MonoBehaviour, IEnemy
{
    public bool daPhatHienPlayer = false;
    private bool trongPhamViTanCong = false;
    private bool dangTanCong = false;
    public float tocDo;

    private Animator anim;
    private Transform player;

    public Transform phamViTanCong;
    public LayerMask players;
    public float vongTronPhamVi;
    public int satThuong;

    public int maxHealth;
    public int currentHealth;
    public bool isDead = false;

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
        get { return currentHealth; }
        set { currentHealth = (int)value; }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        Invoke("FindPlayer", 2f);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            return;
        }
    }

    void Update()
    {
        CheckTanCong();
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") && !anim.GetCurrentAnimatorStateInfo(0).IsName("AttackStun") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            DiChuyen();
        }
    }

    private void CheckTanCong()
    {
        Collider2D Player = Physics2D.OverlapCircle(phamViTanCong.transform.position, vongTronPhamVi, players);
        if (Player != null && !isDead )
        {
            trongPhamViTanCong = true;
            anim.SetBool("isAttack", true);
        }
        else
        {
            trongPhamViTanCong = false;
        }
    }

    public void TanCong()
    {
        Collider2D Player = Physics2D.OverlapCircle(phamViTanCong.transform.position, vongTronPhamVi, players);

        if (Player != null && !isDead)
        {
            Player.GetComponent<Samurai>().TakeDamage(satThuong, transform.position);
        }
    }

    public void BatDauTanCong()
    {
        dangTanCong = true;
    }
    public void KetThucTanCong()
    {
        dangTanCong = false;
        anim.SetBool("isAttack", false);
        anim.SetTrigger("isStun");
    }

    private void DiChuyen()
    {
        if (daPhatHienPlayer && !trongPhamViTanCong && !dangTanCong && !isDead)
        {
            anim.SetBool("isRun", true);
            Vector2 viTriPlayer = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, viTriPlayer, tocDo * Time.deltaTime);

            if (viTriPlayer.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                healthBar.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (viTriPlayer.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                healthBar.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            anim.SetBool("isRun", false);
        }
    }

    public void TakeDamage(int satThuong)
    {
        if (dangTanCong)
        {
            currentHealth -= 0;
        }
        else
        {
            anim.SetTrigger("isHit");
            popUptext.text = satThuong.ToString();
            GameObject newObject = Instantiate(popUpDamage, transform.position, transform.rotation);
            newObject.transform.position += new Vector3(0, 2f, 0);
            currentHealth -= satThuong;
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        currentHealth = 0;
        anim.SetBool("isDie", true);

        active.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        DropItems();

        Destroy(gameObject, 30);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(phamViTanCong.transform.position, vongTronPhamVi);
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

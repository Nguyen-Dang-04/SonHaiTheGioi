using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyBat : MonoBehaviour, IEnemy
{   
    public bool daPhatHienPlayer = false;
    private Transform player;
    public float tocDo;
    public Transform viTriTanCong;
    private float phamViTanCong = 1f;
    public LayerMask Player;
    private Animator anim;
    public int satThuong;
    private bool dangTanCong = false;
    private bool dangTanCong2 = false;
    private int soLanBiTanCong;
    private bool coTheRun = false;

    public int maxHealth;
    public int currentHealth;
    public bool isDead = false;
    public GameObject popUpDamage;
    public TMP_Text popUptext;

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
        Invoke("FindPlayer", 2f);
        anim = GetComponent<Animator>();
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
        if (coTheRun == false)
        {
            WakeUp();
        }

        CheckTanCong();
        Run();
    }

    private void WakeUp()
    {
        if (daPhatHienPlayer)
        {
            anim.SetTrigger("isWakeUp");
            StartCoroutine(SequenceCoroutine());
        }
    }

    private IEnumerator SequenceCoroutine()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetTrigger("isIdie");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        coTheRun = true;
    }

    public void Run()
    {
        if (coTheRun == true && daPhatHienPlayer && !dangTanCong && !dangTanCong2
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") && !isDead)
        {
            anim.SetBool("isRun", true);

            // Vị trí player
            Vector2 viTriPlayer = new Vector2(player.position.x, player.position.y);

            // Vector hướng từ enemy tới player
            Vector2 huong = (viTriPlayer - (Vector2)transform.position).normalized;

            // Vị trí mục tiêu cách player 2f
            Vector2 viTriMucTieu = viTriPlayer - huong * 2f;

            // Chỉ di chuyển nếu chưa tới gần
            if (Vector2.Distance(transform.position, viTriPlayer) > 2f)
            {
                transform.position = Vector2.MoveTowards(transform.position, viTriMucTieu, tocDo * Time.deltaTime);
            }

            // Lật mặt theo hướng
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
    }

    private void CheckTanCong()
    {
        Collider2D player = Physics2D.OverlapCircle(viTriTanCong.transform.position, phamViTanCong, Player);
        if(player != null && !dangTanCong && !dangTanCong2 && soLanBiTanCong < 2 && !isDead)
        {
            anim.SetBool("isAttack1",true);
        }
        else if (player != null && !dangTanCong && !dangTanCong2 && soLanBiTanCong >= 2 && !isDead)
        {
            anim.SetBool("isAttack2", true);
        }
    }

    public void BatDauTanCong()
    {
        dangTanCong = true;
    }
    public void KetThucTanCong()
    {
        dangTanCong = false;
        anim.SetBool("isAttack1", false); // reset lại
    }

    public void BatDauTanCong2()
    {
        dangTanCong2 = true;
    }
    public void KetThucTanCong2()
    {
        dangTanCong2 = false;
        soLanBiTanCong = 0;
        anim.SetBool("isAttack2", false); // reset lại
    }
    public void TanCong()
    {
        Collider2D player = Physics2D.OverlapCircle(viTriTanCong.transform.position, phamViTanCong, Player);
        if (player != null && soLanBiTanCong < 2 && !isDead)
        {
            player.GetComponent<Samurai>().TakeDamage(satThuong, transform.position);
        }
        else if(player != null && soLanBiTanCong >= 2 && !isDead)
        {
            player.GetComponent<Samurai>().TakeDamage(satThuong * 2, transform.position);
        }
    }

    public void TakeDamage(int satThuong)
    {
        if (dangTanCong2)
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
            soLanBiTanCong++;
        }

        if (currentHealth <= 0 && !isDead)
        {
            anim.SetBool("isAttack1", false);
            anim.SetBool("isAttack2", false);
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        currentHealth = 0;
        anim.SetBool("isDead", true);
   
        active.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        DropItems();
        Destroy(gameObject, 30);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(viTriTanCong.transform.position, phamViTanCong);
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

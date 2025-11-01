using TMPro;
using UnityEngine;
using System.Collections;

public class BatMiniBoss : MonoBehaviour, IEnemy
{
    private Animator anim;
    private Rigidbody2D rb;
    public float speed;
    public Transform playerTransform;
    private bool isFacingRight = true;

    public GameObject pointAttack;
    public float radius;
    private bool attacking = false;
    public LayerMask player;
    public int damage;

    public int maxHealth;
    public int currentHeal;
    private bool isDead = false;
    public float Health
    {
        get { return currentHeal; }
        set { currentHeal = (int)value; }
    }

    public TMP_Text popUptext;
    public GameObject popUpDamageBoss;

    public HealthBar healthBar;
    public GameObject active;

    public static BatMiniBoss doiTuong;

    void Awake()
    {
        doiTuong = this;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || anim.GetCurrentAnimatorStateInfo(0).IsName("Start"))
        {
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            PursuePlayer();
        }
    }

    void PursuePlayer()
    {
        if (playerTransform != null && Samurai.doiTuong.isDead == false)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * speed, direction.y * speed);
            anim.SetBool("isRun", true);
            if ((direction.x < 0 && !isFacingRight) || (direction.x > 0 && isFacingRight))
            {
                Flip();
            }
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            anim.SetBool("isRun", false);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector2 loacalScale = transform.localScale;
        loacalScale.x *= -1;
        transform.localScale = loacalScale;

        Vector2 healthBarScale = healthBar.transform.localScale;
        healthBarScale.x *= -1;
        healthBar.transform.localScale = healthBarScale;

    }

    void CheckAttack()
    {
        Collider2D[] playerInRange = Physics2D.OverlapCircleAll(pointAttack.transform.position, radius, player);

        if (playerInRange.Length > 0)
        {
            anim.SetBool("isAttack", true);
        }
        else
        {
            anim.SetBool("isAttack", false);
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

    public void TakeDamage(int damage)
    {
        if (attacking || anim.GetCurrentAnimatorStateInfo(0).IsName("Start"))
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
            isDead = true;
            anim.SetTrigger("isDie");
            active.SetActive(false);
            playerTransform = null;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;

            Destroy(gameObject, 30);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointAttack.transform.position, radius);
    }
}

using TMPro;
using UnityEngine;
using System.Collections;

public class Samurai2 : MonoBehaviour,IEnemy
{
    public Transform Point;
    private Transform CurrentPoint;
    private Rigidbody2D rb;
    public float speed;
    private Animator anim;

    public GameObject pointAttack;
    public float radius;
    private bool attacking = false;
    public LayerMask player;
    public float damagePercent = 0.1f;

    public bool Pursuit = false;
    public Transform playerTransform;

    public Transform limitPoint;
    public Transform limitPoint2;

    public bool isFacingRight = true;
    public TMP_Text popUptext;
    public GameObject popUpDamage;

    public int maxHealth;
    public int currentHeal;

    private AudioSamurai2 audioSamurai2;

    public float Health
    {
        get { return currentHeal; }
        set { currentHeal = (int)value; }
    }

    public string[] dialogues;
    public TextMeshPro dialogueText; 
    public float displayInterval = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSamurai2 = GetComponent<AudioSamurai2>();
        CurrentPoint = Point;
        currentHeal = maxHealth;

        StartCoroutine(Healing());
        StartCoroutine(DisplayRandomDialogue());
    }

    void Update()
    {
        CheckAttack();

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            if (Pursuit)
            {
                PursuePlayer();
                CheckLimitPoint();
            }
            else
            {
                MoveToCurrentPoint();
                CheckSwitchPoint();
            }
        }
    }
    void MoveToCurrentPoint()
    {
        Vector2 direction = (CurrentPoint.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
        anim.SetBool("isRun", true);

        if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
        {
            Flip();
        }
    }

    void CheckSwitchPoint()
    {
        if (Vector2.Distance(transform.position, CurrentPoint.position) <= 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isRun", false);           
            if (isFacingRight) 
            {
                Flip();
            }
        }
    }

    void CheckAttack()
    {
        Collider2D[] playerInRange = Physics2D.OverlapCircleAll(pointAttack.transform.position, radius, player);

        if (playerInRange.Length > 0)
        {
            anim.SetBool("isAttack", true);
            Pursuit = true;
        }
        else
        {
            anim.SetBool("isAttack", false);
        }
    }

    public void Attack()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(pointAttack.transform.position, radius, player);

        if (playerCollider != null)
        {
            Samurai samurai = playerCollider.GetComponent<Samurai>();

            if (samurai != null)
            {
                int damage = (int)(samurai.maxHealth * damagePercent);
                samurai.TakeDamage(damage, transform.position);
            }
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

    void PursuePlayer()
    {
        if (playerTransform != null && Samurai.doiTuong.isDead == false)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
            anim.SetBool("isRun", true);
            if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
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

    public void TakeDamage(int damage)
    {
        if (attacking)
        {
            return;
        }
        else
        {
            popUptext.text = damage.ToString();
            currentHeal -= damage;
            anim.SetTrigger("isHurt");
            GameObject newObject = Instantiate(popUpDamage, transform.position, transform.rotation);
            newObject.transform.position += new Vector3(0, 2f, 0);
        }
    }

    void CheckLimitPoint()
    {
        if ((limitPoint != null && Vector2.Distance(transform.position, limitPoint.position) < 0.1f) ||
            (limitPoint2 != null && Vector2.Distance(transform.position, limitPoint2.position) < 0.1f))
        {
            Pursuit = false;
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointAttack.transform.position, radius);
        if (limitPoint != null)
        {
            Gizmos.DrawWireSphere(limitPoint.position, 0.2f);
        }
        if (limitPoint2 != null)
        {
            Gizmos.DrawWireSphere(limitPoint2.position, 0.2f);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        RectTransform textRectTransform = transform.GetChild(0).GetComponent<RectTransform>();

        Vector3 localScale = textRectTransform.localScale;
        localScale.x *= -1;
        textRectTransform.localScale = localScale;

        Vector3 parentLocalScale = transform.localScale;
        parentLocalScale.x *= -1; 
        transform.localScale = parentLocalScale;
    }


    IEnumerator Healing()
    {
        while (true)
        {
            yield return new WaitForSeconds(120f);
            currentHeal = maxHealth;
        }
    }

    IEnumerator DisplayRandomDialogue()
    {
        while (true) 
        {
            if (dialogues.Length > 0)
            {
                int randomIndex = Random.Range(0, dialogues.Length);
                dialogueText.text = dialogues[randomIndex]; 
            }

            yield return new WaitForSeconds(3f);
            dialogueText.text = ""; 

            yield return new WaitForSeconds(displayInterval - 3f);
        }
    }
}

using UnityEngine;

public class TrapsHatTung : MonoBehaviour
{
    public float lucHatTung = 10f;
    public float banKinh = 1f;
    private Animator anim;
    public LayerMask playerLayer;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void KichHoatHatTung()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, banKinh, playerLayer);

        if (playerCollider != null)
        {
            Rigidbody2D playerRb = playerCollider.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 hattung = Vector2.up * lucHatTung;
                playerRb.AddForce(hattung, ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isHatTung", true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isHatTung", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, banKinh);
    }
}
using UnityEngine;

public class Heart : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D cl;
    private bool daVaCham = false;
    private bool coTheVaCham = false;

    public AudioSource SFXSource;
    public AudioClip audioColiision;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MatDat") && !coTheVaCham)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero; // Thay vì Vector3.zero
            coTheVaCham = true;
        }

        if (other.gameObject.CompareTag("Player") && !daVaCham && coTheVaCham)
        {
            daVaCham = true;
            SFXSource.PlayOneShot(audioColiision);
            HealingHeart.doiTuong.coTheHoiMau = true;
            cl.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            anim.SetTrigger("isCollision");
        }
    }

    public void SomeMethod()
    {
        GameObject objectToReturn = this.gameObject;
        ObjectPoolManager.Instance.ReturnObjectToPool("Heart", objectToReturn);
        HealingHeart.doiTuong.coTheHoiMau = false;
        cl.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        daVaCham = false;
    }
}

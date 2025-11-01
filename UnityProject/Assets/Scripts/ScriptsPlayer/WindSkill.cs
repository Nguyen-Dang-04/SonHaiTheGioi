using UnityEngine;

public class WindSkill : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D bl;
    public bool daVaCham = false;
    public bool coTheVaCham = false;

    public AudioSource SFXSource;
    public AudioClip audioUnlockSkill;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bl = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MatDat") && !coTheVaCham)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            coTheVaCham = true;
        }

        if (other.gameObject.CompareTag("Player") && !daVaCham && coTheVaCham)
        {
            daVaCham = true;
            SFXSource.PlayOneShot(audioUnlockSkill);

            bool newState = true;
            PlayerPrefs.SetInt("UnlockNPC", newState ? 1 : 0);
            PlayerPrefs.SetInt("UnlockSpawned", 1);
            PlayerPrefs.SetInt("ThongBaoSkill", 1);
            PlayerPrefs.Save();
            Debug.Log("Đã lưu UnlockNPC = " + newState);

            bl.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            anim.SetTrigger("End"); 
        }
    }

    // Gọi từ animation event để hủy object
    public void DestroyCoin()
    {
        Destroy(gameObject);
    }
}

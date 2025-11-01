using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D cl;
    private bool daVaCham = false;
    private bool coTheVaCham = false;
    public int numberOfCoins;

    private CoinManager coinManager;
    public AudioSource SFXSource;
    public AudioClip audioCoin;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<Collider2D>();
        GameObject manager = GameObject.FindWithTag("GameManager");
        if (manager != null)
        {
            coinManager = manager.GetComponent<CoinManager>();
        }
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
            SFXSource.PlayOneShot(audioCoin);
            cl.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            coinManager.CoinCount += numberOfCoins;
            anim.SetTrigger("isCollision");
        }
    }

    public void SomeMethod()
    {
        GameObject objectToReturn = this.gameObject;
        ObjectPoolManager.Instance.ReturnObjectToPool("Coin", objectToReturn);
        cl.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        daVaCham = false;
        coTheVaCham = false;
    }
}

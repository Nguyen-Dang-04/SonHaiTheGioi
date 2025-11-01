using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
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
            rb.bodyType = RigidbodyType2D.Kinematic;  // Updated
            rb.linearVelocity = Vector2.zero;  // Updated to use linearVelocity
            coTheVaCham = true;
        }

        if (other.gameObject.CompareTag("Player") && !daVaCham && coTheVaCham)
        {
            daVaCham = true;
            SFXSource.PlayOneShot(audioColiision);
            HealingHeart.doiTuong.coTheHoiMana = true;
            cl.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;  // Updated
            rb.linearVelocity = Vector2.zero;  // Updated to use linearVelocity
            anim.SetTrigger("isCollision");
        }
    }

    public void SomeMethod()
    {
        GameObject objectToReturn = this.gameObject;
        ObjectPoolManager.Instance.ReturnObjectToPool("Mana", objectToReturn);
        cl.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;  // Updated
        daVaCham = false;
    }
}

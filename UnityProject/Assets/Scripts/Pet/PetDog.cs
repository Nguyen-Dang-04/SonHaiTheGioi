using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetDog : MonoBehaviour
{
    private Animator anim;
    [SerializeField] AudioSource SFXSource;
    public AudioClip AudioDog;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayDogAudio()
    {
        SFXSource.PlayOneShot(AudioDog);
    }


    private void OnTriggerEnter2D(Collider2D vaCham)
    {
        if (vaCham.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isAttack", true);
        }
    }

    private void OnTriggerExit2D(Collider2D thoatVaCham)
    {
        if (thoatVaCham.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isAttack", false);
        }   
    }
}

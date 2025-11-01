using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSkeleton2 : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public AudioClip Dead;
    public AudioClip Hit;
    public AudioClip Attack;
    
    public void audioDead()
    {
        audioSource.PlayOneShot(Dead);
    }

    public void audioHit()
    {
        audioSource.PlayOneShot(Hit);
    }

    public void audioAttack()
    {
        audioSource.PlayOneShot(Attack);
    }
}

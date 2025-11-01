using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBossSkeleton : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public AudioClip Dead;
    public AudioClip Hit;
    public AudioClip Attack;
    public AudioClip Revive;

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

    public void audioRevevi()
    {
        audioSource.PlayOneShot(Revive);
    }
}

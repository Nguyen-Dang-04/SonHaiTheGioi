using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AudioBossWith : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public AudioClip Hide;
    public AudioClip Attack;
    public void audioHide()
    {
        audioSource.PlayOneShot(Hide);
    }

    public void audioAttack()
    {
        audioSource.PlayOneShot(Attack);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioWind : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public AudioClip Attack;

    public void audioAttack()
    {
        audioSource.PlayOneShot(Attack);
    }
}

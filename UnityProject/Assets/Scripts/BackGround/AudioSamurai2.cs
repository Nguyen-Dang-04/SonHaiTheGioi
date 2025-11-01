using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSamurai2 : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public AudioClip attack;
    public AudioClip hit;
    public AudioClip laugh;

    public void AudioAttack()
    {
        audioSource.PlayOneShot(attack);
    }

    public void AudioHit()
    {
        audioSource.PlayOneShot(hit);
    }

    public void AudioLaugh()
    {
        audioSource.PlayOneShot(laugh);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerSkeleton : MonoBehaviour
{
    [SerializeField] AudioSource SFXSource;

    public AudioClip enemyHit;
    public AudioClip enemyAttack;
    public AudioClip enemyDie;


    public void AudioHit()
    {
        SFXSource.PlayOneShot(enemyHit);
    }

    public void AudioAttack()
    {
        SFXSource.PlayOneShot(enemyAttack);
    }

    public void AudioDie()
    {
        SFXSource.PlayOneShot(enemyDie);
    }
}

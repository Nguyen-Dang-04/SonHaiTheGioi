using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioBat : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public AudioClip BiDanh;
    public AudioClip Danh;
    public AudioClip TiengCan;

    public void AudioCan()
    {
        audioSource.PlayOneShot(TiengCan);
    }

    public void AudioDanh()
    {
        audioSource.PlayOneShot(Danh);
    }
    public void AudioBiDanh()
    {
        audioSource.PlayOneShot(BiDanh);
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
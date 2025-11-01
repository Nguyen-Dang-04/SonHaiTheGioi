using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMushroom : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public AudioClip audioClipTeNga;
    public AudioClip BiChoang;
    public AudioClip BiDanh;
    public AudioClip Chet;

    public void AudioTeNga()
    {
        audioSource.PlayOneShot(audioClipTeNga);
    }
    public void AudioBiChoang()
    {
        audioSource.PlayOneShot(BiChoang);
    }
    public void AudioBiDanh()
    {
        audioSource.PlayOneShot(BiDanh);
    }

    public void AudioChet()
    {
        audioSource.PlayOneShot(Chet);
    }
    public void StopAudio()
    {
        audioSource.Stop();
    }
}

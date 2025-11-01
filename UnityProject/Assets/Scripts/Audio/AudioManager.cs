using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------Audio source-----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("------Audio Clip-----")]
    public AudioClip backGround;
    public AudioClip attack;
    public AudioClip attack2;
    public AudioClip attack3;
    public AudioClip dash;
    public AudioClip dashAttack;
    public AudioClip Defend;
    public AudioClip Hurt;
    public AudioClip jump;
    public AudioClip endJump;
    public AudioClip Run;
    public AudioClip Throw;
    public AudioClip ThrowTotem;
    public AudioClip Dead;

    public AudioClip TiengThoDai;
    public AudioClip TiengCuoi;

    private void Start()
    {
        musicSource.clip = backGround;
        musicSource.Play();
    }

    public void FadeOutMusic(float fadeDuration)
    {
        StartCoroutine(FadeOutCoroutine(fadeDuration));
    }

    private IEnumerator FadeOutCoroutine(float fadeDuration)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}

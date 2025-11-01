using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHatTung : MonoBehaviour
{
    [SerializeField] AudioSource SFXSource;
    public AudioClip audioHatTung;

    public void SFXHatTung()
    {
        SFXSource.PlayOneShot(audioHatTung);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpaceGate : MonoBehaviour
{
    [SerializeField] AudioSource audioSpaceGate;
    public AudioClip clipAudioSpaceGate;
    void Start()
    {
        audioSpaceGate.clip = clipAudioSpaceGate;
        audioSpaceGate.Play();
    }
}

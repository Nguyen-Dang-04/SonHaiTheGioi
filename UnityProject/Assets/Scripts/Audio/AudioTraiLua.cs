using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTraiLua : MonoBehaviour
{
    [SerializeField] AudioSource audioTraiLuaSource;
    public AudioClip audioTraiLuaClip;
 
     void Start()
    {
        audioTraiLuaSource.clip = audioTraiLuaClip;
        audioTraiLuaSource.Play();
    }
}

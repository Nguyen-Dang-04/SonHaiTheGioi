using UnityEngine;

public class AudioClick : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip click;

    public void Click()
    {
        audioSource.PlayOneShot(click);
    }
}

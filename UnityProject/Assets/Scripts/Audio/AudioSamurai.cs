using UnityEngine;

public class AudioSamurai : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void AudioAttack()
    {
        audioManager.PlaySFX(audioManager.attack);
    }

    public void AudioAttack2() 
    {
        audioManager.PlaySFX(audioManager.attack2);
    }

    public void AudioAttack3()
    {
        audioManager.PlaySFX(audioManager.attack3);
    }

    public void AudioDash()
    {
        audioManager.PlaySFX(audioManager.dash);
    }

    public void AudioDashAttack()
    {
        audioManager.PlaySFX(audioManager.dashAttack);
    }

    public void AudioDefend()
    {
        audioManager.PlaySFX(audioManager.Defend);
    }

    public void AudioHurt()
    {
        audioManager.PlaySFX(audioManager.Hurt);
    }

    public void AudioRun() 
    {
        audioManager.PlaySFX(audioManager.Run);
    }

    public void AudioThrow()
    {
        audioManager.PlaySFX(audioManager.Throw);
    }
}

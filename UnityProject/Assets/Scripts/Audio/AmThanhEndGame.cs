using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmThanhEndGame : MonoBehaviour
{
    AudioManager audioManager;
    private bool daPhatAmThanh = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D vaCham)
    {
        if (vaCham.CompareTag("Player") && !daPhatAmThanh)
        {
            audioManager.FadeOutMusic(1.5f);
            StartCoroutine(PhatAmThanh());
            daPhatAmThanh = true;
        }
    }

    private IEnumerator PhatAmThanh()
    {
        yield return new WaitForSeconds(1.5f);
        audioManager.PlaySFX(audioManager.TiengThoDai);
        yield return new WaitForSeconds(1f);
        audioManager.PlaySFX(audioManager.TiengCuoi);
    }
}

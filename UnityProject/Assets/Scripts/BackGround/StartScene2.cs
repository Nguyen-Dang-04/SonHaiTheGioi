using System.Collections;
using UnityEngine;

public class StartScene2 : MonoBehaviour
{
    [SerializeField] private GameObject sceneManager;
    [SerializeField] private GameObject playerStart;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator sceneTransitionAnimator;
    public bool CutScene = true;
    void Start()
    {
        Samurai.doiTuong.LoadPlayer();
        CoinManager.doiTuong.LoadCoin();
        StartCoroutine(OnStart());

    }

    IEnumerator OnStart()
    {
        player.SetActive(false);
        playerStart.SetActive(true);
        sceneManager.SetActive(true);
        sceneTransitionAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        playerStart.SetActive(false);
        player.SetActive(true);
        sceneManager.SetActive(false);
    }
}
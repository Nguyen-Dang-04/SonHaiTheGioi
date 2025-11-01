using UnityEngine;

public class StartScene : MonoBehaviour
{
    [SerializeField] private GameObject sceneManager;
    [SerializeField] private GameObject playerStart;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator sceneTransitionAnimator;

    void Start()
    {
        //Samurai.doiTuong.SavePlayer();
        //CoinManager.doiTuong.Savecoin();
        player.SetActive(false);
        Samurai.doiTuong.LoadPlayer();
        CoinManager.doiTuong.LoadCoin();
        playerStart.SetActive(true);
        sceneManager.SetActive(true);
        sceneTransitionAnimator.SetTrigger("Start");
    }

    public void TatVatLieu()
    {
        playerStart.SetActive(false);
        player.SetActive(true);
        sceneManager.SetActive(false);
    }
}

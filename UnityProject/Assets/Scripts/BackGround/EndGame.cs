using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject sceneManager;
    [SerializeField] private GameObject MenuEndGame;
    [SerializeField] private Animator sceneTransitionAnimator;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerEndGame;
    private Transform playerTransform;

    void Update()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D vaCham)
    {
        if (vaCham.CompareTag("Player"))
        {
            StartCoroutine(NemuEndGame());
        }
    }

    private void OnTriggerExit2D(Collider2D vaCham)
    {
        if (vaCham.CompareTag("Player"))
        {
            return;
        }
    }
    private IEnumerator NemuEndGame()
    {
        CoinManager.doiTuong.Savecoin();
        Samurai.doiTuong.SavePlayer();
        UnlockNewLevel();
        GameObject newPlayerMenu = Instantiate(playerEndGame, playerTransform.position, playerTransform.rotation);
        newPlayerMenu.transform.localScale = playerTransform.localScale;
        player.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        MenuEndGame.SetActive(true);
    }

    void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >=PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex",SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel",1) + 1);
            PlayerPrefs.Save();
        }
    }
}

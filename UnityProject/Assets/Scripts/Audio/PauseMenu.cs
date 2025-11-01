using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] private GameObject playerMenu;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject sceneManager;
    [SerializeField] private Animator sceneTransitionAnimator;
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

    public void Pause()
    {
        pauseMenu.SetActive(true);
    }

    public void Home()
    {
        GameObject newPlayerMenu = Instantiate(playerMenu, playerTransform.position, playerTransform.rotation);
        newPlayerMenu.transform.localScale = playerTransform.localScale;
        pauseMenu.SetActive(false);
        player.SetActive(false);
        Samurai.doiTuong.SavePlayer();
        sceneManager.SetActive(true);
        sceneTransitionAnimator.SetTrigger("End");
        StartCoroutine(LoadLevel());
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }

    public void Restart()
    {
        GameObject newPlayerMenu = Instantiate(playerMenu, playerTransform.position, playerTransform.rotation);
        newPlayerMenu.transform.localScale = playerTransform.localScale;
        pauseMenu.SetActive(false);
        player.SetActive(false);
        sceneManager.SetActive(true);
        sceneTransitionAnimator.SetTrigger("End");
        StartCoroutine(LoadRestart());
    }
    public void QuitGame()
    {
        Samurai.doiTuong.SavePlayer();
        Application.Quit();
    }

    private IEnumerator LoadRestart()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Home");
    }

    public void OpenDiscordInvite()
    {
        Application.OpenURL("https://discord.gg/vW3jy24EYy");
    }

    public void OpenTiktokInvite()
    {
        Application.OpenURL("https://www.tiktok.com/@ken.1104?is_from_webapp=1&sender_device=pc");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseEndGame : MonoBehaviour
{
    [SerializeField] private GameObject sceneManager;
    [SerializeField] private GameObject EndGameMenu;
    [SerializeField] private Animator sceneTransitionAnimator;
    public void Restart()
    {
        EndGameMenu.SetActive(false);
        sceneManager.SetActive(true);
        sceneTransitionAnimator.SetTrigger("End");
        StartCoroutine(LoadRestart());
    }

    public void Home()
    {
        EndGameMenu.SetActive(false);
        sceneManager.SetActive(true);
        sceneTransitionAnimator.SetTrigger("End");
        StartCoroutine(LoadHomel());
    }

    public void NextLevel()
    {
        EndGameMenu.SetActive(false);
        sceneManager.SetActive(true);
        sceneTransitionAnimator.SetTrigger("End");
        StartCoroutine(LoadLevel());
    }
    private IEnumerator LoadHomel()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Home");
    }
    private IEnumerator LoadRestart()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

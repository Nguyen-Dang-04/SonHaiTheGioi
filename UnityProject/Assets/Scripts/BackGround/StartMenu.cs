using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    private bool isHomeSceneLoaded;
    [SerializeField] private GameObject EndMenu;
    [SerializeField] private Animator sceneTransitionAnimator;

    public AudioSource audioSource;
    public AudioClip click;

    void Start()
    {
        //PlayerPrefs.DeleteKey("HomeSceneLoaded");
        //PlayerPrefs.Save();
        EndMenu.SetActive(false);
        isHomeSceneLoaded = PlayerPrefs.GetInt("HomeSceneLoaded", 0) == 1;
    }

    public void GoHomeButton()
    {
        StartCoroutine(GoHome());
    }

    private IEnumerator GoHome()
    {
        EndMenu.SetActive(true);
        sceneTransitionAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);

        if (!isHomeSceneLoaded)
        {
            PlayerPrefs.SetInt("HomeSceneLoaded", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Cutscene");
        }
        else
        {
            SceneManager.LoadScene("Home");
        }
    }

    public void GoMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Click()
    {
        audioSource.PlayOneShot(click);
    }
}

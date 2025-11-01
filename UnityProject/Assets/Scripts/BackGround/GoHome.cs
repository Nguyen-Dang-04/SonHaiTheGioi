using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoHome : MonoBehaviour
{
    [SerializeField] private GameObject sceneManager;
    [SerializeField] private Animator sceneTransitionAnimator;
 
    public void GoMenu()
    {
        StartCoroutine(StarHome());
    }

    private IEnumerator StarHome()
    {
        sceneManager.SetActive(true);
        sceneTransitionAnimator.SetTrigger("End");
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

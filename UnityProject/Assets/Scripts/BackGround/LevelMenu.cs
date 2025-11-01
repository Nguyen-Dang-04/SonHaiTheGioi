using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;

    [SerializeField] private GameObject sceneManager;
    [SerializeField] private Animator sceneTransitionAnimator;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerEndGame;
    private Transform playerTransform;

    private void Awake()
    {
        //PlayerPrefs.DeleteKey("UnlockedLevel");
        //PlayerPrefs.Save();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }   
    }

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
    public void OpenLevel(int levelId)
    {
        GameObject newPlayerMenu = Instantiate(playerEndGame, playerTransform.position, playerTransform.rotation);
        newPlayerMenu.transform.localScale = playerTransform.localScale;
        player.SetActive(false);
        Samurai.doiTuong.SavePlayer();
        sceneManager.SetActive(true);
        sceneTransitionAnimator.SetTrigger("End");
        StartCoroutine(LoadLevel(levelId));
    }

    private IEnumerator LoadLevel(int levelId)
    {
        yield return new WaitForSeconds(1.5f);
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
    }
}

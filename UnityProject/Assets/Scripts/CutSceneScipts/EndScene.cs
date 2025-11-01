using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    public GameObject endScene;
    public GameObject button;
    public Button yourButton; 
    public Animator animEndScene;

    public AudioSource audioSource;
    public AudioClip click;

    void Start()
    {
        yourButton.onClick.AddListener(() => StartCoroutine(EndSceneIE()));
    }

    IEnumerator EndSceneIE()
    {
        audioSource.PlayOneShot(click);
        button.SetActive(false);
        endScene.SetActive(true); 
        animEndScene.SetTrigger("End");
        yield return new WaitForSeconds(1.5f); 
        SceneManager.LoadSceneAsync("Home"); 
    }
}

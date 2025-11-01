using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCotTruyen : MonoBehaviour
{
    private bool isPlayerInZone;
    public GameObject NutBam;
    [SerializeField] private Animator animEndScene;
    [SerializeField] private GameObject sceneManager;

    private void Start()
    {
        if (animEndScene == null)
        {
            Debug.LogError("Animator chưa được gán trong Inspector.");
        }

        if (NutBam == null)
        {
            Debug.LogError("NutBam chưa được gán trong Inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPlayerInZone)
        {
            isPlayerInZone = true;
            NutBam.SetActive(true); 
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            NutBam.SetActive(false); 
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ChuyenScene()); 
        }
    }

    IEnumerator ChuyenScene()
    {
        NutBam.SetActive(false); 
        if (animEndScene != null)
        {
            sceneManager.SetActive(true);
            animEndScene.SetTrigger("End"); 
        }
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("Cutscene"); 
    }
}

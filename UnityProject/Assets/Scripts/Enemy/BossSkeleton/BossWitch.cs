using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class BossWitch : MonoBehaviour
{
    private skeleton2 ske;
    private Animator anim;
    public bool revive = false;
    private bool revived;
    private bool trigggerEffect;
    private bool startInsEnemy = false;
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    public CinemachineVirtualCamera cam3;

    public GameObject smoke;
    public bool stopSpawning;
    public Transform pointSpawm;

    public static BossWitch doiTuong;

    public GameObject[] effect;

    public GameObject traps;
    public GameObject ai3;

    public Samurai samurai;
    void Start()
    {
        anim = GetComponent<Animator>();
        stopSpawning = false;
        doiTuong = this;
        traps.SetActive(false);
        ai3.SetActive(false);

        GameObject myObject = GameObject.Find("Skeleton");
        if (myObject != null)
        {
            ske = myObject.GetComponent<skeleton2>();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy đối tượng với tên Skeleton.");
        }
    }

    void Update()
    {
        if (ske != null && ske.isDead && !revived)
        {
            revived = true;
            StartCoroutine(ReviveAfterDelay());
            StartCoroutine(StartInsEffectAfterDelay());
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && !startInsEnemy)
        {
            startInsEnemy = true;
            StartCoroutine(InsEnemy());
        }
    }
    IEnumerator ReviveAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        CameraManager.SwitchCamera(cam2);
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.2f);
        revive = true;
        yield return new WaitForSeconds(1.1f);
        CameraManager.SwitchCamera(cam1);
    }

    IEnumerator InsEnemy()
    {
        while (!stopSpawning)
        {
            Vector3 newPos = transform.position + new Vector3(2, 2f, 0);
            Instantiate(smoke, newPos, transform.rotation);
            yield return new WaitForSeconds(20f);
        }
    }

    IEnumerator StartInsEffectAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(InsEffect());
    }
    IEnumerator InsEffect()
    {
        while (!stopSpawning)
        {
            int random = Random.Range(0, effect.Length);
            GameObject spawn = effect[random];
            Vector3 randomPosition = GetRandomPositionOnLine();
            Instantiate(spawn, randomPosition, transform.rotation);
            yield return new WaitForSeconds(Random.Range(0f, 3f));
        }
    }

    private Vector3 GetRandomPositionOnLine()
    {
        Vector3 start = pointSpawm.position;
        Vector3 end = new Vector3(start.x + 18f, start.y, start.z);
        float randomX = Random.Range(start.x, end.x);
        return new Vector3(randomX, start.y, start.z);
    }

    private void OnDrawGizmos()
    {
        Vector3 start = pointSpawm.position;
        Vector3 end = new Vector3(start.x + 18f, start.y, start.z);
        Gizmos.color = Color.red; 
        Gizmos.DrawLine(start, end);
    }

    public IEnumerator Hide()
    {
        yield return new WaitForSeconds(2f);
        CameraManager.SwitchCamera(cam3);
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("isHide");
        yield return new WaitForSeconds(1f);
        traps.SetActive(true);
        ai3.SetActive(true);
        yield return new WaitForSeconds(1f);
        CameraManager.SwitchCamera(cam1);
    }
}

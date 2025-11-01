using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridPet : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    private Transform currentPoint;
    private bool isWaiting;
    public float speed;
    public float thoiGianDung;

    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] AudioSource audioSource;
    public AudioClip audioClipBrid;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = PointB.transform;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        if (!isWaiting)
        {
            Run();
        }
    }

    private void Run()
    {
        Vector2 direction = (currentPoint.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f)
        {
            StartCoroutine(PauseBeforeSwitching());
        }
    }

    private IEnumerator PauseBeforeSwitching()
    {
        isWaiting = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isWalk", false);
        audioSource.PlayOneShot(audioClipBrid);
        yield return new WaitForSeconds(thoiGianDung);

        if (currentPoint == PointB.transform)
        {
            Flip();
            currentPoint = PointA.transform;
        }
        else
        {
            Flip();
            currentPoint = PointB.transform;
        }

        anim.SetBool("isWalk", true);
        isWaiting = false;
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(PointB.transform.position, 0.5f);
        Gizmos.DrawLine(PointA.transform.position, PointB.transform.position);
    }
}

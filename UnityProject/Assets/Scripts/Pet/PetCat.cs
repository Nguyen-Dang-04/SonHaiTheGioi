using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetCat : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    private Transform currentPoint;
    private bool isWaiting;
    public float speed;
    private Animator anim;
    private Rigidbody2D rb;

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
        if (currentPoint == PointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
        }

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
        yield return new WaitForSeconds(5);

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

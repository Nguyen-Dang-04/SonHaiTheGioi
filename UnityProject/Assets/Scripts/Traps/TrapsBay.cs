using UnityEngine;

public class TrapsBay : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform PointA;
    public Transform PointB;
    private Transform currentPoint;
    public float tocDo;
    public float fallDelay = 0.5f;
    private bool isTriggered = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        currentPoint = PointB;
    }

    private void Update()
    {
        if (!isTriggered)
        {
            DiChuyen();
        }
    }
    private void DiChuyen()
    {
        if (PointA.position == PointB.position)
        {
            return;
        }

        Vector2 huongDiChuyen;
        if (currentPoint == PointB)
        {
            huongDiChuyen = (PointB.position - transform.position).normalized;
            rb.linearVelocity = huongDiChuyen * tocDo;
        }
        else if (currentPoint == PointA)
        {
            huongDiChuyen = (PointA.position - transform.position).normalized;
            rb.linearVelocity = huongDiChuyen * tocDo;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f)
        {
            if (currentPoint == PointB)
            {
                currentPoint = PointA;
            }
            else
            {
                currentPoint = PointB;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            if (IsPlayerAbove(collision.transform))
            {
                isTriggered = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                Invoke("Fall", fallDelay);
            }
        }
    }

    private bool IsPlayerAbove(Transform player)
    {

        return player.position.y > transform.position.y;
    }

    private void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
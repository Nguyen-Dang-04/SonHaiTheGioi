using UnityEngine;

public class areaSkeleton : MonoBehaviour
{
    [SerializeField] private skeleton2 slt2;
    [SerializeField] private Vector2 size = new Vector2(5f, 3f); 
    [SerializeField] private LayerMask playerLayer;

    private void Update()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position, size, 0f, playerLayer);
        if (hit != null)
        {
            slt2.Pursuit = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);
    }
}

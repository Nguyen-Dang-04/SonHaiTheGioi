using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private float life = 1f;
    private Samurai samurai;

    private void Start()
    {
        Destroy(gameObject, life);

        GameObject samuraiObject = GameObject.Find("Samurai");
        if (samuraiObject != null)
        {
            samurai = samuraiObject.GetComponent<Samurai>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IEnemy enemy = collision.gameObject.GetComponent<IEnemy>();
            if (enemy != null)
            {
                if (samurai != null)
                {
                    int damage = UnityEngine.Random.Range(samurai.minDamageShuriken, samurai.maxDamageShuriken + 1);
                    enemy.TakeDamage(damage);
                }

                Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("MatDat"))
        {
            Destroy(gameObject);
        }
    }
}

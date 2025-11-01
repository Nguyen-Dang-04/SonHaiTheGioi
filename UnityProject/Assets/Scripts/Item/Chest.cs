using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private float dropRateHeart;   // 40% rơi Heart
    [SerializeField] private int maxHeart;             // rơi tối đa 2 bình máu

    [SerializeField] private float dropRateMana;    // 30% rơi Mana
    [SerializeField] private int maxMana;              // rơi tối đa 2 bình mana

    [SerializeField] private float dropRateCoin;    // 80% rơi Coin
    [SerializeField] private int maxCoin;

    public GameObject Button;
    private bool isPlayerInZone = false;
    private bool opened = false;

    public AudioClip audioOpenChest;
    public AudioSource SFXsCource;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
          isPlayerInZone = true;
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E) && !opened)
        {
            Open();
        }
    }

    private void Open()
    {
        Button.SetActive(false);
        opened = true;
        SFXsCource.PlayOneShot(audioOpenChest);
        anim.SetTrigger("isCollision");
        DropItems();
        Destroy(gameObject, 30f);
    }

    void DropItems()
    {
        TryDrop("Heart", dropRateHeart, maxHeart);
        TryDrop("Mana", dropRateMana, maxMana);
        TryDrop("Coin", dropRateCoin, maxCoin);
    }

    void TryDrop(string itemName, float dropRate, int maxCount)
    {
        // Chỉ rơi nếu Random.value < tỉ lệ rơi
        if (Random.value < dropRate)
        {
            int count = Random.Range(1, maxCount + 1); // rơi từ 1 đến maxCount vật phẩm
            for (int i = 0; i < count; i++)
            {
                GameObject item = ObjectPoolManager.Instance.GetObjectFromPool(itemName);
                if (item != null)
                {
                    item.transform.position = transform.position;
                    // 🎇 (Tuỳ chọn) Thêm hiệu ứng bay lên nhẹ
                    Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(3f, 6f)), ForceMode2D.Impulse);
                    }
                }
            }
        }
    }

}

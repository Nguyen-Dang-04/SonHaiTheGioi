using UnityEngine;

public class Summon_Totem : MonoBehaviour
{
    public GameObject TotemPrefab;

    [SerializeField] private float delayBeforeSpawn = 2f; // 2 giây đứng trên đất
    [SerializeField] private float lifeTime = 10f;        // sống tối đa 10 giây
    [SerializeField] private float spawnOffsetY = 2.24f;

    private float lifeTimer = 0f;       // ⬅️ tổng thời gian tồn tại (không reset)
    private float groundedTimer = 0f;   // ⬅️ thời gian đang đứng trên đất (reset khi rời đất)

    private bool hasSpawned = false;
    private bool isCollidedWithMatDat = false;
    private bool hasPlayedAudio = false;
    private bool vaChamNuoc = false;

    AudioManager audioManager;

    private void Awake()
    {
        var audioObj = GameObject.FindGameObjectWithTag("Audio");
        audioManager = audioObj ? audioObj.GetComponent<AudioManager>() : null;
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;

        // Đếm thời gian đứng trên đất
        if (isCollidedWithMatDat && !vaChamNuoc)
        {
            groundedTimer += Time.deltaTime;

            if (groundedTimer >= delayBeforeSpawn && !hasSpawned)
            {
                Instantiate(TotemPrefab, transform.position + Vector3.up * spawnOffsetY, Quaternion.identity);
                hasSpawned = true;
                Destroy(gameObject);
                return;
            }
        }

        // Hết tuổi thọ mà chưa spawn → tự hủy
        if (lifeTimer >= lifeTime && !hasSpawned)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MatDat") && !hasPlayedAudio)
        {
            isCollidedWithMatDat = true;
            audioManager?.PlaySFX(audioManager.ThrowTotem);
            hasPlayedAudio = true;

            // Bắt đầu đếm lại “đứng trên đất”
            groundedTimer = 0f;  // ⬅️ chỉ reset timer đứng đất, KHÔNG đụng lifeTimer
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MatDat"))
        {
            isCollidedWithMatDat = false;
            hasPlayedAudio = false;   // cho phép phát lại âm khi chạm đất lần sau
            groundedTimer = 0f;       // rời đất → hủy chờ spawn
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            vaChamNuoc = true;
            Destroy(gameObject, 2f);
        }
    }
}

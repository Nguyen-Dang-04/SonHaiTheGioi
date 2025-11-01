using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Samurai samurai;
    public float slowMotionTimeScale = 0.2f;
    public float slowMotionDuration = 0.6f;
    private float originalTimeScale = 1f;

    public int thoiGianHoiSinh = 3;
    public float thoiGianHoiSinhHienTai;
    public GameObject AtiveDie;

    private bool isDying = false;
    private bool isSlowMotion = false;
    private bool isDeathSequence = false;   // 👈 mới: chống trùng

    private void Start()
    {
        Invoke(nameof(Find), 2f);
        originalTimeScale = Time.timeScale;
        thoiGianHoiSinhHienTai = thoiGianHoiSinh;
    }

    private void Update()
    {
        IsDead();

        if (isDying)
        {
            thoiGianHoiSinhHienTai -= Time.unscaledDeltaTime;
            if (thoiGianHoiSinhHienTai <= 1f)
            {
                isDying = false;
                Respawn();
            }
        }
    }

    void Find()
    {
        samurai = Object.FindFirstObjectByType<Samurai>();
    }

    private void IsDead()
    {
        if (samurai != null && samurai.isDead && !isDying && !isSlowMotion && !isDeathSequence)
        {
            Die();
        }
    }

    private void Die()
    {
        isDeathSequence = true;                 // 👈 khóa ngay lập tức
        StartCoroutine(DelayBeforeSlowMotion());
    }

    private IEnumerator DelayBeforeSlowMotion()
    {
        // ⏳ chờ 2 giây theo thời gian thực
        yield return new WaitForSecondsRealtime(2f);
        yield return StartCoroutine(SlowMotionEffect());
    }

    private IEnumerator SlowMotionEffect()
    {
        isSlowMotion = true;
        Time.timeScale = slowMotionTimeScale;
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        Time.timeScale = originalTimeScale;
        isSlowMotion = false;
        ThongBaoDie();
    }

    void ThongBaoDie()
    {
        if (AtiveDie != null) AtiveDie.SetActive(true);
        isDying = true;
        thoiGianHoiSinhHienTai = thoiGianHoiSinh;
    }

    void Respawn()
    {
        Time.timeScale = originalTimeScale;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

using UnityEngine;

public class Skill : MonoBehaviour
{
    public bool skillActive = true;
    public GameObject skillWind;
    public Transform diemEffect;
    private float tocDoEffect = 8f;
    public float manaHaoTonSkillWind = 0.05f;
    private Samurai samurai;

    private void Start()
    {
        samurai = GetComponent<Samurai>();
        skillActive = PlayerPrefs.GetInt("SkillActive", 0) == 1;
    }
    public void SkillWind()
    {
        if (!skillActive || samurai == null) return;

        // ✅ Tính 20% năng lượng hiện tại
        int manaMat = Mathf.RoundToInt(samurai.currentMana * 0.05f);

        if (samurai.currentMana < manaMat)
        {
            Debug.Log("❌ Không đủ năng lượng để tung Skill Wind!");
            return;
        }

        // 🔹 Trừ năng lượng
        samurai.Mana(manaMat);

        // 🔹 Tạo hiệu ứng gió
        GameObject effectWind = Instantiate(skillWind, diemEffect.position, diemEffect.rotation);
        Rigidbody2D rb = effectWind.GetComponent<Rigidbody2D>();

        if (samurai.dangNhinBenPhai)
        {
            rb.linearVelocity = diemEffect.right * tocDoEffect;
            effectWind.transform.localScale = new Vector2(
                Mathf.Abs(effectWind.transform.localScale.x),
                effectWind.transform.localScale.y
            );
        }
        else
        {
            rb.linearVelocity = -diemEffect.right * tocDoEffect;
            effectWind.transform.localScale = new Vector2(
                -Mathf.Abs(effectWind.transform.localScale.x),
                effectWind.transform.localScale.y
            );
        }
    }
}

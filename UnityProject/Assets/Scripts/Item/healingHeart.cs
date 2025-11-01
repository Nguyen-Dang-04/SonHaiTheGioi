using UnityEngine;
using System.Collections;

public class HealingHeart : MonoBehaviour
{
    private float amountOfHealthRestored = 0.2f;
    private float healingDuration = 2f;
    private bool isHealing = false;
    public bool coTheHoiMau = false;

    private float amountOfManaRestored = 0.2f;
    private float manaRestorationDuration = 2f;
    private bool restoringMana = false;
    public bool coTheHoiMana = false;

    private Samurai samurai;
    public static HealingHeart doiTuong;

    private void Awake()
    {
        doiTuong = this;
    }

    private void Start()
    {
        samurai = GetComponent<Samurai>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HealthPotion") && coTheHoiMau)
        {
            StartCoroutine(Healing());
        }

        if (collision.gameObject.CompareTag("ManaPotion") && coTheHoiMana)
        {
            StartCoroutine(RestoreMana());
        }
    }
    public IEnumerator Healing()
    {
        if (samurai == null || samurai.currentHealth >= samurai.maxHealth || isHealing)
        {
            yield break;
        }

        isHealing = true;
        int startHealth = samurai.currentHealth;
        int healthToRegenerate = Mathf.FloorToInt(samurai.maxHealth * amountOfHealthRestored);
        float elapsedTime = 0f;

        while (elapsedTime < healingDuration)
        {
            elapsedTime += Time.deltaTime;
            int newHealth = Mathf.FloorToInt(startHealth + healthToRegenerate * (elapsedTime / healingDuration));
            samurai.currentHealth = Mathf.Min(newHealth, samurai.maxHealth);
            yield return null;
        }
        samurai.currentHealth = Mathf.Min(startHealth + healthToRegenerate, samurai.maxHealth); 
        isHealing = false;
    }


    private IEnumerator RestoreMana()
    {
        if (samurai == null || samurai.currentMana >= samurai.maxMana || restoringMana)
        {
            yield break;
        }
        restoringMana = true;
        int startMana = samurai.currentMana;
        int manaToRegenerate = Mathf.FloorToInt(samurai.maxMana * amountOfManaRestored);
        float elapsedTime = 0f;
        while (elapsedTime < manaRestorationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / manaRestorationDuration;
            int newMana = startMana + Mathf.FloorToInt(manaToRegenerate * t);
            newMana = Mathf.Min(newMana, samurai.maxMana);
            if (newMana != samurai.currentMana)
            {
                samurai.currentMana = newMana;
            }
            yield return null;
        }
        samurai.currentMana = Mathf.Min(startMana + manaToRegenerate, samurai.maxMana);
        coTheHoiMana = false;
        restoringMana = false;
    }
}
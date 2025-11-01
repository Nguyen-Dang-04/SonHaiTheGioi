using System.Collections;
using UnityEngine;

public class Healing : MonoBehaviour
{
    public GameObject NutBam;
    public GameObject samuraiObject;
    private Samurai samurai;
    private bool healing = false;
    [SerializeField] AudioSource SFXSource;
    public AudioClip audioHealing;

    private void Start()
    {
        if (samuraiObject != null)
        {
            samurai = samuraiObject.GetComponent<Samurai>();
        }
        NutBam.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NutBam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NutBam.SetActive(false);
        }
    }

    private void Update()
    {
        if (NutBam.activeInHierarchy && Input.GetKeyDown(KeyCode.E) && !healing)
        {
            StartCoroutine(HealingCoroutine());
        }
    }

    private IEnumerator HealingCoroutine()
    {
        if (samurai == null || (samurai.currentHealth >= samurai.maxHealth &&
                                samurai.currentMana >= samurai.maxMana))
        {
            yield break;
        }
        healing = true;
        int startHealth = samurai.currentHealth;
        int healthToHeal = samurai.maxHealth - startHealth;
        int startMana = samurai.currentMana;
        int manaToRestore = samurai.maxMana - startMana;
        float healingDuration = 2.7f;
        float elapsedTime = 0f;
        if (SFXSource != null && audioHealing != null)
        {
            SFXSource.clip = audioHealing;
            SFXSource.Play();
        }
        while (elapsedTime < healingDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / healingDuration;
            int newHealth = startHealth + Mathf.FloorToInt(healthToHeal * t);
            int newMana = startMana + Mathf.FloorToInt(manaToRestore * t);
            if (newHealth != samurai.currentHealth)
            {
                samurai.currentHealth = newHealth;
            }
            if (newMana != samurai.currentMana)
            {
                samurai.currentMana = newMana;
            }
            yield return null;
        }
        samurai.currentHealth = samurai.maxHealth;
        samurai.currentMana = samurai.maxMana;
        healing = false;
    }
}
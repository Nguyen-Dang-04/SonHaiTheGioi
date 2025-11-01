[System.Serializable]
public class PlayerData
{
    public int health;
    public int currentHealth;

    public int mana;
    public int currentMana;

    public int defend;
    public int currentDef;

    public int maxDamage;
    public int minDamage;

    public int maxDamageDashAttack;
    public int minDamageDashAttack;
    public int manaDashAttack;

    public int maxDamageShuriken;
    public int minDamageShuriken;
    public int manaShuriken;

    public int damegeMagic;

    public PlayerData(Samurai samurai)
    {
        health = samurai.maxHealth;
        currentHealth = samurai.currentHealth;

        mana = samurai.maxMana;
        currentMana = samurai.currentMana;

        defend = samurai.maxDef;
        currentDef = samurai.CurrentDef;

        maxDamage = samurai.maxDamage;
        minDamage = samurai.minDamage;

        maxDamageDashAttack = samurai.maxDamageDashAttack;
        minDamageDashAttack = samurai.minDamageDashAttack;
        manaDashAttack = samurai.manaDashAttack;

        maxDamageShuriken = samurai.maxDamageShuriken;
        minDamageShuriken = samurai.minDamageShuriken;
        manaShuriken = samurai.manaThorow;

        damegeMagic = samurai.DamegeMagic;
    }
}

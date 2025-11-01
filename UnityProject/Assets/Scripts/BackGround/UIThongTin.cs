using TMPro;
using UnityEngine;
using System.Globalization;

public class UIThongTin : MonoBehaviour
{
    public GameObject samuraiObject;
    private Samurai samurai;

    public TextMeshProUGUI damage;
    public TextMeshProUGUI damegeDash;
    public TextMeshProUGUI Shuriken;
    public TextMeshProUGUI Magic;

    public TextMeshProUGUI Mau;
    public TextMeshProUGUI Mana;
    public TextMeshProUGUI Def;

    void Start()
    {
        samurai = samuraiObject.GetComponent<Samurai>();
    }

    void Update()
    {
        if (damage != null)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3 };

            damage.text = "Cận chiến: " +
                          samurai.minDamage.ToString("N0", nfi) +
                          " - " +
                          samurai.maxDamage.ToString("N0", nfi);
        }

        if (damegeDash != null)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3 };

            damegeDash.text = "Lướt đâm: " +
                          samurai.minDamageDashAttack.ToString("N0", nfi) +
                          " - " +
                          samurai.maxDamageDashAttack.ToString("N0", nfi);
        }

        if (Magic != null)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3 };

            Magic.text = "Ma thuật: " +
                          samurai.DamegeMagic.ToString("N0", nfi);
        }

        if (Shuriken != null)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3 };

            Shuriken.text = "Phi tiêu: " +
                          samurai.minDamageShuriken.ToString("N0", nfi) +
                          " - " +
                          samurai.maxDamageShuriken.ToString("N0", nfi);
        }
        if (Mau != null)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3 };

            Mau.text = "Máu: " +
                          samurai.maxHealth.ToString("N0", nfi);
        }

        if (Mana != null)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3 };

            Mana.text = "Năng lượng: " +
                          samurai.maxMana.ToString("N0", nfi);
        }

        if (Def != null)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3 };

            Def.text = "Phòng thủ: " +
                          samurai.maxDef.ToString("N0", nfi);
        }
    }
}

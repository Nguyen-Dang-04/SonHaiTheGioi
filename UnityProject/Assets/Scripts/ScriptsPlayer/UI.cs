using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject samuraiObject;
    private Samurai samurai;

    public CoinManager coinManager;

    public TextMeshProUGUI timerTextShuriken;
    public Slider shurikenSlider;

    public TextMeshProUGUI timerTexDash;
    public Slider dashSlider;

    public TextMeshProUGUI timerTextTotem;
    public Slider totemSlider;

    public TextMeshProUGUI TextDef;
    public Slider defSlider;

    public TextMeshProUGUI HealthBarValueText;
    public Slider healthSlider;
    public Gradient gradientHealth;
    public Image fillHealth;

    public TextMeshProUGUI ManaBarValueText;
    public Slider manaSlider;
    public Gradient gradientMana;
    public Image fillMana;

    public TextMeshProUGUI textCoin;

    
    void Start()
    {
        samurai = samuraiObject.GetComponent<Samurai>();
        
    }

    void Update()
    {
        timerTextShuriken.text = samurai.throwHienTai.ToString("0");
        shurikenSlider.value = samurai.throwHienTai;
        shurikenSlider.maxValue = samurai.hoiChieuThrow;

        timerTexDash.text = samurai.dashHienTai.ToString("0");
        dashSlider.value = samurai.dashHienTai;
        dashSlider.maxValue = samurai.hoiChieuDashHienTai;

        timerTextTotem.text = samurai.TotemHienTai.ToString("0");
        totemSlider.value = samurai.TotemHienTai;
        totemSlider.maxValue = samurai.hoiChieuTotem;

        TextDef.text = samurai.CurrentDef + "/" + samurai.maxDef;
        defSlider.maxValue = samurai.defRegenDelay;
        defSlider.value = samurai.DefHienTai;

        HealthBarValueText.text = samurai.currentHealth.ToString() + "/" + samurai.maxHealth.ToString();
        healthSlider.value = samurai.currentHealth;
        healthSlider.maxValue = samurai.maxHealth;
        fillHealth.color = gradientHealth.Evaluate(healthSlider.normalizedValue);

        ManaBarValueText.text = samurai.currentMana.ToString() + "/" + samurai.maxMana.ToString();
        manaSlider.value = samurai.currentMana;
        manaSlider.maxValue = samurai.maxMana;

        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberGroupSeparator = ".";
        nfi.NumberGroupSizes = new int[] { 3 };
        textCoin.text = coinManager.CoinCount.ToString("N0", nfi);
    }
}

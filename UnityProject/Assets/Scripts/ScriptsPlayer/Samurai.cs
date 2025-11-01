using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Samurai : MonoBehaviour
{
    public float tocDo;
    private float diChuyen;
    public bool dangNhinBenPhai = true;

    public float lucNhay;
    private bool dangNhay = false;

    public bool dangTanCong = false;
    public static Samurai doiTuong;

    private bool coTheDash = true;
    private bool dangDash;
    public float lucDash;
    public float thoiGianDash;
    public int hoiChieuDash;
    public float dashHienTai;
    public float hoiChieuDashHienTai;

    public int hoiChieuDashAttack;
    public int minDamageDashAttack;
    public int maxDamageDashAttack;
    public Transform dashBoxOrigin;  
    public Vector2 dashBoxSize = new Vector2(2f, 1f);
    private bool dashAtkActive = false;
    private HashSet<Collider2D> dashHitSet = new HashSet<Collider2D>();

    public Transform diemPhongShuriken;
    public GameObject shurikenPrefab;
    public float tocDoShuriken;
    public int hoiChieuThrow;
    public float throwHienTai;
    private bool coTheNiem = true;

    public int maxHealth;
    public int currentHealth;
    public bool isDead = false;

    public int maxMana;
    public int currentMana;
    private int regenInterval = 1;
    private float timerMana;
    private int manaPerTick = 1;

    public int manaDash;
    public int manaDashAttack;
    public int manaThorow;
    public int manaTotem;

    public GameObject diemTanCong;
    public float radius;
    public int minDamage;
    public int maxDamage;
    public LayerMask enemies;

    public int maxDamageShuriken;
    public int minDamageShuriken;

    public int DamegeMagic;

    private bool dangDef;
    public int maxDef;
    public float DefHienTai;
    public int CurrentDef;
    public float defRegenDelay = 5f;
    private float defRegenTimer = 0f;

    public Transform diemPhongTotem;
    public GameObject TotemPrefab;
    public float tocDoTotem;
    public float hoiChieuTotem;
    public float TotemHienTai;
    private bool coTheNiemTotem = true;

    public GameObject playerDie;

    public TMP_Text popUptext;
    public GameObject popUpSamurai;

    public Animator anim;
    private Rigidbody2D rb;
    AudioManager audioManager;

    [SerializeField] private RectTransform childUI;

    // === MỚI: Cờ khóa input khi hộp chat mở ===
    public bool TamDungDiChuyen = false;

    private void Awake()
    {
        doiTuong = this;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CooldownUI();

        // Nếu muốn vẫn cho phép bị trừ máu/mana hồi… cứ để các hàm này chạy bình thường
        Attack();     // sẽ có guard isChatting bên trong
        Health();
        HoiMana();

        // === MỚI: chặn toàn bộ input/di chuyển khi đang chat ===
        if (TamDungDiChuyen)
        {
            if (rb != null)
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y); // giữ vận tốc Y (rơi/nhảy hiện tại)
            anim.SetBool("isRun", false);
            anim.SetBool("isDefend", false);
            dangDef = false;
            // Không xử lý Run/Jump/Dash/Throw/Defend/Summon nữa
            return;
        }

        RegenDefTick();

        if (!dangTanCong && !IsPlayingAnyAttackAnimation())
        {
            Run();
            Jump();
            Dash();
            Throw();
            Defend();
            Summon();
        }
        if (dangDef && CurrentDef <= 0)
        {
            EndDefend();
        }

        if (dangDash)
        {
            return;
        }

        if (IsPlayingAnyAttackAnimation())
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1.5f;
        }
    }

    private bool IsPointerOnUI()
    {
        if (EventSystem.current == null) return false;

#if UNITY_STANDALONE || UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Kiểm tra xem UI đang được trỏ vào có nằm trong Canvas "IgnoreUI" không
            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                if (result.gameObject.layer == LayerMask.NameToLayer("IgnoreUI"))
                    return false; // bỏ qua Canvas này
            }
            return true;
        }
#else
    if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
    {
        var pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.GetTouch(0).position
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.layer == LayerMask.NameToLayer("IgnoreUI"))
                return false;
        }
        return true;
    }
#endif

        return false;
    }


    private void Run()
    {
        if (TamDungDiChuyen) return; // guard kép
        if (dangDef) return;

        diChuyen = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(diChuyen * tocDo, rb.linearVelocity.y);
        anim.SetBool("isRun", math.abs(diChuyen) > 0.1f);

        if (diChuyen > 0 && !dangNhinBenPhai) Flip();
        else if (diChuyen < 0 && dangNhinBenPhai) Flip();
    }

    private void Flip()
    {
        dangNhinBenPhai = !dangNhinBenPhai;
        Vector3 scal = transform.localScale;
        scal.x *= -1;
        transform.localScale = scal;
        FlipChildUI();
    }

    private void FlipChildUI()
    {
        if (childUI == null) return;

        Vector3 uiScale = childUI.localScale;
        uiScale.x *= -1;
        childUI.localScale = uiScale;
    }

    private void Jump()
    {
        if (TamDungDiChuyen) return;
        if (dangDef) return;

        if (Input.GetKeyDown(KeyCode.Space) && !dangNhay)
        {
            rb.AddForce(Vector2.up * lucNhay, ForceMode2D.Impulse);
            audioManager.PlaySFX(audioManager.jump);
        }
    }

    private void OnTriggerEnter2D(Collider2D vaCham)
    {
        if (vaCham.gameObject.CompareTag("MatDat"))
        {
            dangNhay = false;
            anim.SetBool("isJump", false);
            audioManager.PlaySFX(audioManager.endJump);
        }
    }

    private void OnTriggerExit2D(Collider2D vaCham)
    {
        if (vaCham.gameObject.CompareTag("MatDat"))
        {
            dangNhay = true;
            anim.SetBool("isJump", true);
        }
    }

    private void Attack()
    {
        if (TamDungDiChuyen) return;
        if (IsPointerOnUI()) return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && !dangTanCong)
        {
            anim.SetBool("isRun", false);
            anim.SetBool("isDefend", false);
            dangDef = false;
            dangTanCong = true;
        }
    }

    private void Dash()
    {
        if (TamDungDiChuyen) return;

        if (coTheDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && currentMana >= manaDash)
            {
                anim.SetTrigger("isDash");
                Mana(manaDash);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) && currentMana >= manaDashAttack)
            {
                anim.SetTrigger("isDashAttack");
                Mana(manaDashAttack);
            }
        }
    }

    void FixedUpdate()
    {
        if (!dashAtkActive) return;

        Collider2D[] hits = Physics2D.OverlapBoxAll(dashBoxOrigin.position, dashBoxSize, 0f, enemies);
        foreach (var h in hits)
        {
            if (dashHitSet.Contains(h)) continue;
            dashHitSet.Add(h);

            if (h.TryGetComponent<IEnemy>(out var enemy))
            {
                int damageRandom = UnityEngine.Random.Range(minDamageDashAttack, maxDamageDashAttack + 1);
                enemy.TakeDamage(damageRandom);
            }
        }
    }

    public void Anim_DashAttackStart()
    {
        dashAtkActive = true;
        dashHitSet.Clear();
    }

    public void Anim_DashAttackEnd()
    {
        dashAtkActive = false;
    }


    void OnDrawGizmosSelected()
    {
        if (dashBoxOrigin == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(dashBoxOrigin.position, dashBoxSize);
    }

    public void EvenDash()
    {
        StartCoroutine(HoiChieu(hoiChieuDash));
        hoiChieuDashHienTai = hoiChieuDash;
    }

    public void EvenDashAttack()
    {
        StartCoroutine(HoiChieu(hoiChieuDashAttack));
        hoiChieuDashHienTai = hoiChieuDashAttack;
    }

    public IEnumerator HoiChieu(int HoiChieu)
    {
        coTheDash = false;
        anim.SetBool("isDefend", false);
        dangDef = false;
        dangDash = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * lucDash, 0f);
        yield return new WaitForSeconds(thoiGianDash);
        rb.gravityScale = originalGravity;
        dangDash = false;
        dashHienTai = HoiChieu;
        yield return new WaitForSeconds(HoiChieu);
        coTheDash = true;
    }

    private void Throw()
    {
        if (TamDungDiChuyen) return;
        if (IsPointerOnUI()) return;

        if (Input.GetKeyDown(KeyCode.Q) && coTheNiem && currentMana >= manaThorow)
        {
            anim.SetTrigger("isThrow");
            Mana(manaThorow);
        }
    }

    public void Shuriken()
    {
        var shuriken = Instantiate(shurikenPrefab, diemPhongShuriken.position, diemPhongShuriken.rotation);
        if (dangNhinBenPhai)
            shuriken.GetComponent<Rigidbody2D>().linearVelocity = diemPhongShuriken.right * tocDoShuriken;
        else
            shuriken.GetComponent<Rigidbody2D>().linearVelocity = -diemPhongShuriken.right * tocDoShuriken;
    }

    public IEnumerator Cooldown()
    {
        coTheNiem = false;
        anim.SetBool("isDefend", false);
        dangDef = false;
        throwHienTai = hoiChieuThrow;
        yield return new WaitForSeconds(hoiChieuThrow);
        coTheNiem = true;
    }

    bool IsPlayingAnyAttackAnimation()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2") || stateInfo.IsName("Attack3")
            || stateInfo.IsName("Transition1") || stateInfo.IsName("Transition2") || stateInfo.IsName("Transition3")
            || stateInfo.IsName("Dash") || stateInfo.IsName("DashAttack") || stateInfo.IsName("Throw") || stateInfo.IsName("EndDefend")
            || stateInfo.IsName("Hurt") || stateInfo.IsName("Dead") || stateInfo.IsName("Throw_Fire_Totem");
    }

    private void CooldownUI()
    {
        if (throwHienTai > 0) throwHienTai -= Time.deltaTime;
        else if (throwHienTai <= 0) throwHienTai = 0;

        if (dashHienTai > 0) dashHienTai -= Time.deltaTime;
        else if (dashHienTai <= 0) dashHienTai = 0;

        if (TotemHienTai > 0) TotemHienTai -= Time.deltaTime;
        else if (TotemHienTai <= 0) TotemHienTai = 0;
    }

    private void Health()
    {
        if (currentHealth <= 0 && !isDead)
        {
            currentHealth = 0;
            isDead = true;
            Die();
        }
    }

    public void Die()
    {
        audioManager.PlaySFX(audioManager.Dead);
        GameObject dieInstance = Instantiate(playerDie, transform.position, Quaternion.identity);
        dieInstance.transform.localScale = transform.localScale;
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damageAmount, Vector2 attackPosition)
    {
        if (dangDash)
        {
            currentHealth -= 0;
        }
        else if (dangDef && BiTanCongPhiaTruoc(attackPosition))
        {
            CurrentDef = Mathf.Max(0, CurrentDef - 1);
            currentHealth -= 0;             
            anim.SetTrigger("isEndDefend"); 
        }
        else
        {
            popUptext.text = damageAmount.ToString();
            currentHealth -= damageAmount;
            anim.SetTrigger("isHurt");
            GameObject newObject = Instantiate(popUpSamurai, transform.position, transform.rotation);
            newObject.transform.position += new Vector3(0, 1.5f, 0);
        }
    }

    public void Mana(int anount)
    {
        currentMana -= anount;
        if (currentMana < 0) currentMana = 0;
    }

    private void HoiMana()
    {
        if (currentMana < maxMana)
        {
            timerMana += Time.deltaTime;
            if (timerMana >= regenInterval)
            {
                currentMana += manaPerTick;
                currentMana = Mathf.Min(currentMana, maxMana);
                timerMana = 0f;
            }
        }
    }

    public void TanCong()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(diemTanCong.transform.position, radius, enemies);

        foreach (Collider2D enemyGameObject in enemiesHit)
        {
            if (enemyGameObject.TryGetComponent<IEnemy>(out IEnemy enemy))
            {
                int damageRandom = UnityEngine.Random.Range(minDamage, maxDamage + 1);
                enemy.TakeDamage(damageRandom);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(diemTanCong.transform.position, radius);
    }

    private void Defend()
    {
        if (TamDungDiChuyen) return;

        if (Input.GetKey(KeyCode.S) && !dangDef && !dangNhay && CurrentDef > 0)
        {
            anim.SetBool("isDefend", true);
            anim.SetBool("isRun", false);
            rb.linearVelocity = Vector2.zero;
            dangDef = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            EndDefend();
        }
    }

    private void EndDefend()
    {
        anim.SetBool("isDefend", false);
        dangDef = false;
    }

    private void RegenDefTick()
    {
        if (CurrentDef < maxDef)
        {
            defRegenTimer += Time.deltaTime;
            DefHienTai = Mathf.Clamp(defRegenDelay - defRegenTimer, 0f, defRegenDelay); // cho UI

            if (defRegenTimer >= defRegenDelay)
            {
                CurrentDef = Mathf.Min(CurrentDef + 1, maxDef);
                defRegenTimer = 0f;
            }
        }
        else
        {
            // Đầy def thì không đếm
            defRegenTimer = 0f;
            DefHienTai = 0f;
        }
    }

    private bool BiTanCongPhiaTruoc(Vector2 viTriTanCong)
    {
        Vector2 huongEnemy = viTriTanCong - (Vector2)transform.position;
        if (dangNhinBenPhai) return huongEnemy.x > 0;
        else return huongEnemy.x < 0;
    }

    private void Summon()
    {
        if (TamDungDiChuyen) return;

        if (Input.GetKeyDown(KeyCode.R) && coTheNiemTotem && currentMana >= manaTotem)
        {
            anim.SetTrigger("isThrowTotem");
            Mana(manaTotem);
        }
    }

    public void ThrowTotem()
    {
        var Totem = Instantiate(TotemPrefab, diemPhongTotem.position, diemPhongTotem.rotation);
        if (dangNhinBenPhai)
            Totem.GetComponent<Rigidbody2D>().linearVelocity = diemPhongTotem.right * tocDoTotem;
        else
            Totem.GetComponent<Rigidbody2D>().linearVelocity = -diemPhongTotem.right * tocDoTotem;
    }

    public IEnumerator CooldownTotem()
    {
        coTheNiemTotem = false;
        anim.SetBool("isDefend", false);
        dangDef = false;
        TotemHienTai = hoiChieuTotem;
        yield return new WaitForSeconds(hoiChieuTotem);
        coTheNiemTotem = true;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        maxHealth = data.health;
        currentHealth = data.currentHealth;
        maxMana = data.mana;
        currentMana = data.currentMana;
        maxDef = data.defend;
        CurrentDef = data.currentDef;
        maxDamageShuriken = data.maxDamageShuriken;
        minDamageShuriken = data.minDamageShuriken;
        manaThorow = data.manaShuriken;
        maxDamage = data.maxDamage;
        minDamage = data.minDamage;
        maxDamageDashAttack = data.maxDamageDashAttack;
        minDamageDashAttack = data.minDamageDashAttack;
        manaDashAttack = data.manaDashAttack;
        DamegeMagic = data.damegeMagic;
    }
}

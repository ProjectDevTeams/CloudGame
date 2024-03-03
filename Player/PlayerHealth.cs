using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] AudioClip takeDamS;
    public bool isDead { get; private set; }
    [SerializeField] private float KBThrust = 10f;
    [SerializeField] private float damRecoveryTime = 1f;
    [SerializeField] GameObject Weapon;

    private Slider healthSlider;
    private bool canTakeDam = true;
    private Knockback kb;
    private Flash flash;
    const string HEALTH_SLIDER_TEXT = "HealthSlider";
    const string SCENE_TEXT = "Scene0";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    public static bool atFullHp;
    public static bool resetHealthSlider;

    protected override void Awake()
    {
        base.Awake();
        flash = GetComponent<Flash>();
        kb = GetComponent<Knockback>();
        isDead = false;
        UpdateHealthSlider();
    }

    private void Start()
    {
        isDead = false;
        atFullHp = true;
        UpdateHealthSlider();
    }

    private void Update()
    {
        if (resetHealthSlider)
        {
            resetHealthSlider = false;
            UpdateHealthSlider();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDam(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        if (EconomyManager.currentHp < EconomyManager.saveData[0])
        {
            EconomyManager.currentHp += 1;
            UpdateHealthSlider();
        }

        if (EconomyManager.currentHp == EconomyManager.saveData[0])
        {
            atFullHp = true;
        }
    }

    public void TakeDam(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDam) { return; }

        SoundManager.instance.PlaySound(takeDamS);
        kb.GetKB(hitTransform, KBThrust);
        StartCoroutine(flash.FlashRoutine());
        canTakeDam = false;
        EconomyManager.currentHp -= damageAmount;
        atFullHp = false;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (EconomyManager.currentHp <= 0)
        {
            isDead = true;
            Destroy(Weapon);
            EconomyManager.currentHp = EconomyManager.saveData[0];
            EconomyManager.saveData[2] = Mathf.CeilToInt(EconomyManager.saveData[2]*0.75f); //Lose 25% gold
            EconomyManager.Instance.UpdateGoldHave(0);
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        SceneManager.LoadScene(SCENE_TEXT);
    }

    public void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = EconomyManager.saveData[0];
        healthSlider.value = EconomyManager.currentHp;
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damRecoveryTime);
        canTakeDam = true;
    }
}

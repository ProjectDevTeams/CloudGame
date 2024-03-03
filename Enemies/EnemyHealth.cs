using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHp;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float KBThrust = 12f;
    [SerializeField] private string enemyName;
    [SerializeField] private AudioClip deathS;

    private int hp; //Current Hp
    private Knockback KB;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        KB = GetComponent<Knockback>();
    }

    private void Start()
    {
        hp = maxHp;
    }

    public void TakeDam(int _dam)
    {
        hp -= _dam;
        KB.GetKB(PlayerMovement.Instance.transform, KBThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetResetMatTime());
        DetectDeath();
    }

    public void DetectDeath()
    {
        if (hp <= 0)
        {
            SoundManager.instance.PlaySound(deathS);
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            GetComponent<PickUpSpawner>().DropItems();

            if (enemyName == "Slime")
            {
                EconomyManager.saveData[3] += 1;
            }
            if (enemyName == "Cat")
            {
                EconomyManager.saveData[4] += 1;
            }
            if (enemyName == "Boss")
            {
                EconomyManager.win = true;
            }

            EconomyManager.Instance.UpdateQuest();

            Destroy(gameObject);
        }
    }
}

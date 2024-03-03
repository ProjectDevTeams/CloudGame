using UnityEngine;

public class Samurai : MonoBehaviour
{
    [SerializeField] private GameObject[] path = new GameObject[2];
    [SerializeField] private AudioClip questS;
    public static bool passQuest;

    private void Start()
    {
        if (EconomyManager.saveData[2] != 0 || EconomyManager.saveData[3] != 0)
        {
            EconomyManager.Instance.UpdateQuest();
        }
        EconomyManager.Instance.UpdateGoldHave(0);
        ActiveWeapon.changeW = true;

        PathCheck();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            if (passQuest && EconomyManager.saveData[5] < 2)
            {
                SoundManager.instance.PlaySound(questS);
                EconomyManager.saveData[0] += 1;
                EconomyManager.saveData[1] += 1;
                EconomyManager.saveData[2] -= EconomyManager.questReq[EconomyManager.saveData[5]+2];
                EconomyManager.saveData[5] += 1;
                EconomyManager.currentHp = EconomyManager.saveData[0];
                passQuest = false;
                ActiveWeapon.changeW = true;
                CloudService.saving = true;
                EconomyManager.Instance.UpdateGoldHave(0);
                PlayerHealth.Instance.UpdateHealthSlider();
                PathCheck();
            }

            if (EconomyManager.win && EconomyManager.saveData[5] == 2)
            {
                EconomyManager.currentHp = EconomyManager.saveData[0];
                PlayerHealth.Instance.UpdateHealthSlider();
                EconomyManager.saveData[5] += 1;
                CloudService.saving = true;
            }

            EconomyManager.Instance.UpdateQuest();
        }
    }

    private void PathCheck()
    {
        path[0].SetActive(false);
        path[1].SetActive(false);

        if (EconomyManager.saveData[5] >= 1)
        {
            path[0].SetActive(true);
        }
        
        if (EconomyManager.saveData[5] >= 2)
        {
            path[1].SetActive(true);
        }
    }

    private void Update()
    {
        if (EconomyManager.win && EconomyManager.saveData[5] <= 3)
        {
            EconomyManager.win = false;
            EconomyManager.saveData[5] += 1;
            EconomyManager.currentHp = EconomyManager.saveData[0];
            PlayerHealth.Instance.UpdateHealthSlider();
        }
    }
}

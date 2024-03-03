using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EconomyManager : Singleton<EconomyManager>
{
    [SerializeField] private Slider soundSlider;
    private TMP_Text questText;
    private TMP_Text goldText;
    public static int[] saveData = new int[6]; // 0.maxHealth 1.weaponNum 2.goldHave 3.Slime 4.Cat 5.QuestNum
    public static int[] questReq = {5, 15, 10, 50}; //First 2 ele. for enemyNum, Last 2 ele. for goldReg {5, 15, 10, 50}
    private string[] enemyN = {" Slime ", " Cat ", " The forbidden one "};
    public static float soundVolume = 1f;
    public static int currentHp;
    public static bool win;
    const string QUEST_TEXT = "QuestText";
    const string GOLD_AMOUNT_TEXT = "GoldAmountText";
    const string SCENE_TEXT = "Scene0";

    public void UpdateGoldHave(int _n)
    {
        saveData[2] += _n;

        if (goldText == null)
        {
            goldText = GameObject.Find(GOLD_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        goldText.text = saveData[2].ToString("D3");
    }

    public void UpdateQuest()
    {
        if (questText == null)
        {
            questText = GameObject.Find(QUEST_TEXT).GetComponent<TMP_Text>();
        }

        if (saveData[5] < 2)
        {
            questText.text = "Defeat" + enemyN[saveData[5]] + saveData[saveData[5]+3] + "/" + questReq[saveData[5]] + ", " + questReq[saveData[5]+2] + " Golds";

            if (saveData[saveData[5]+3] < questReq[saveData[5]] || saveData[2] < questReq[saveData[5]+2])
            {
                questText.text = "<color=white>" + questText.text;
            }
            else
            {
                questText.text = "<color=green>" + questText.text;
                Samurai.passQuest = true;
            }
        }
        else
        {
            if (saveData[5] == 2)
            {
                questText.text = "Defeat" + enemyN[2];
            }
            else
            {
                questText.text = "Thank you for playing >^<";
            }
        }
    }

    public void StartGame()
    {
        for (int i=1; i<6; i++)
        {
            saveData[i] = 0;
        }

        saveData[0] = 3; //Starting maxHp
        currentHp = 3;
        SceneManager.LoadScene(SCENE_TEXT);
    }

    public void SetSoundVolume()
    {
        soundVolume = soundSlider.value;
        SoundManager.instance.SetSoundVolume();
    }
}

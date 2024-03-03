using UnityEngine;

public class BossScene : MonoBehaviour
{
    [SerializeField] private GameObject path;
    [SerializeField] private GameObject bossRoomBoader;
    [SerializeField] private GameObject bossObject;
    public static bool inBossRoom;
    

    private void Start()
    {
        path.SetActive(false);
        inBossRoom = false;
    }

    private void Update()
    {
        if (EconomyManager.win && !path.activeInHierarchy)
        {
            path.SetActive(true);
        }

        if (EconomyManager.win && bossRoomBoader.activeInHierarchy)
        {
            bossRoomBoader.SetActive(false);
            SoundManager.instance.SetBossBGM(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() && !inBossRoom)
        {
            bossRoomBoader.SetActive(true);
            inBossRoom = true;
            SoundManager.instance.SetBossBGM(true);
            bossObject.SetActive(true);
        }
    }
}

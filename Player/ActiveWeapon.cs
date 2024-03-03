using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private GameObject[] weapon = new GameObject[3];
    [SerializeField] private int[] dam = {1, 3, 10};
    public static bool changeW;

    public int currentDam;

    private void Start()
    {
        WeaponChange();
    }

    private void Update()
    {
        if (changeW)
        {
            changeW = false;
            WeaponChange();
        }
    }

    public void WeaponChange()
    {
        for (int i=0; i<3; i++)
        {
            weapon[i].SetActive(false);
        }

        weapon[EconomyManager.saveData[1]].SetActive(true);
        currentDam = dam[EconomyManager.saveData[1]];
    }
}

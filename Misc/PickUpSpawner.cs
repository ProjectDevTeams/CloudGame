using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe;

    public void DropItems()
    {
        int r = Random.Range(1, 10);

        if (r <= 3)
        {
            Instantiate(healthGlobe, transform.position, Quaternion.identity); 
        } 

        if (4 <= r && r <= 6)
        {
            int rGold = Random.Range(1, 4);
            
            for (int i = 0; i < rGold; i++)
            {
                Instantiate(goldCoin, transform.position, Quaternion.identity);
            }
        }
    }
}

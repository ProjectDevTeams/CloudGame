using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private int damPotency = 1;

    private void Start()
    {
        damPotency = GetComponentInParent<ActiveWeapon>().currentDam;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDam(damPotency);
    }
}

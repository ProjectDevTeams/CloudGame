using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool GetingKB { get; private set; } //KB = knockback

    [SerializeField] private float KBTime = .2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKB(Transform _damSource, float KBThrust)
    {
        GetingKB = true;
        Vector2 differrence = (transform.position - _damSource.position).normalized * KBThrust * rb.mass;
        rb.AddForce(differrence, ForceMode2D.Impulse);
        StartCoroutine(KBRoutine());
    }

    private IEnumerator KBRoutine()
    {
        yield return new WaitForSeconds(KBTime);
        rb.velocity = Vector2.zero;
        GetingKB = false;
    }
}

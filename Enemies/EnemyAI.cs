using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float atkRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float atkCD = 2f;
    [SerializeField] private bool stopMovingWhileAtk = false;

    private bool canAtk = true;

    private enum State {
        Roaming,
        Attacking
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private State state;
    private EnemyPathFinding enemyPathFinding;

    private void Awake()
    {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
            break;

            case State.Attacking:
                Attacking();
            break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;
        enemyPathFinding.MoveTo(roamPosition);

        if (Vector2.Distance(transform.position, PlayerMovement.Instance.transform.position) < atkRange)
        {
            state = State.Attacking;
        }

        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerMovement.Instance.transform.position) > atkRange)
        {
            state = State.Roaming;
        }

        if (atkRange != 0 && canAtk)
        {
            canAtk = false;
            (enemyType as IEnemy).Attack();
            StartCoroutine(AtkCDRoutine());
        }

        if (stopMovingWhileAtk)
        {
            enemyPathFinding.StopMoving();
        }
        else
        {
            timeRoaming += Time.deltaTime;
            enemyPathFinding.MoveTo(roamPosition);

            if (timeRoaming > roamChangeDirFloat)
            {
                roamPosition = GetRoamingPosition();
            }
        }
    }

    private IEnumerator AtkCDRoutine()
    {
        yield return new WaitForSeconds(atkCD);
        canAtk = true;
    }

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}

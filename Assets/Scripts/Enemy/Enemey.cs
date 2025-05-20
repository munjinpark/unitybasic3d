using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /* ───── 상태 ───── */
    enum State { Idle, Patrol, Detect, Attack }

    [Header("Patrol")]
    public Transform[] movePos;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float reachDistance = 0.2f;
    public float waitTime = 3f;

    [Header("Combat")]
    public Transform player;
    public float detectionRange = 5f;
    public float forgetRange = 7f;
    public float attackRange = 1.5f;

    [Header("Animation")]
    public Animator animator;

    /* ───── 내부 필드 ───── */
    State state = State.Patrol;
    Transform currMovePos;
    bool isWaiting;

    /* ───── 초기화 ───── */
    void Start()
    {
        if (movePos.Length > 0) SetNewDestination();
    }

    /* ───── 메인 루프 ───── */
    void Update()
    {
        switch (state)
        {
            case State.Idle: Idle(); break;
            case State.Patrol: Patrol(); break;
            case State.Detect: Detect(); break;
            case State.Attack: Attack(); break;
        }
    }

    /* ───── 상태별 동작 ───── */
    void Idle()
    {
        animator.SetFloat("Speed", 0f);
        if (InRange(detectionRange)) ChangeState(State.Detect);
    }

    void Patrol()
    {
        if (isWaiting || currMovePos == null)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        animator.SetFloat("Speed", moveSpeed);
        MoveTowards(currMovePos.position);

        if (Vector3.Distance(transform.position, currMovePos.position) < reachDistance)
            StartCoroutine(WaitAndResume());

        if (InRange(detectionRange)) ChangeState(State.Detect);
    }

    void Detect()
    {
        animator.SetFloat("Speed", moveSpeed);
        MoveTowards(player.position);

        if (InRange(attackRange)) ChangeState(State.Attack);
        else if (!InRange(forgetRange)) ChangeState(State.Patrol);
    }

    void Attack()
    {
        animator.SetBool("IsAttack", true);

        if (!InRange(attackRange))
        {
            animator.SetBool("IsAttack", false);
            ChangeState(State.Detect);
        }
    }

    /* ───── 보조 메서드 ───── */
    IEnumerator WaitAndResume()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        SetNewDestination();
        isWaiting = false;
    }

    bool InRange(float range)
    {
        if(player == null ) return false;

        return Vector3.Distance(transform.position, player.position) <= range;
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);
        }
        transform.position =
            Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    void SetNewDestination()
    {
        if (movePos.Length == 0) return;

        Transform next;
        do
        {
            next = movePos[Random.Range(0, movePos.Length)];
        } while (next == currMovePos && movePos.Length > 1);

        currMovePos = next;
    }

    void ChangeState(State next)
    {
        if (state == next) return;          // 중복 전환 방지
        state = next;                       // 상태 갱신
    }
}

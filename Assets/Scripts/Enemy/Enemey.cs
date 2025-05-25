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

    [Header("Detect")]
    public Transform player;
    public float detectionRange = 5f;
    public float forgetRange = 7f;
    public float attackRange = 1.5f;

    [Header("Animation")]
    public Animator animator;

    /* ───── 내부 필드 ───── */
    State state = State.Patrol;
    Transform tagetMovePos;
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
        if (isWaiting || tagetMovePos == null)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        animator.SetFloat("Speed", moveSpeed);
        MoveTowards(tagetMovePos.position);

        if (Vector3.Distance(transform.position, tagetMovePos.position) < reachDistance)
            WaitAndResume();

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
        {6
            animator.SetBool("IsAttack", false);
            ChangeState(State.Detect);
        }
    }

    /* ───── 보조 메서드 ───── */
    void WaitAndResume()
    {
        isWaiting = true;

        Invoke("SetNewDestination", waitTime);
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
            dir.y = 0f;
            Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);

            //Debug.Log($"Current dir: {dir}");

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
        } while (next == tagetMovePos && movePos.Length > 1);

        isWaiting = false;
        tagetMovePos = next;
    }

    void ChangeState(State next)
    {
        if (state == next) return;          // 중복 전환 방지
        state = next;                       // 상태 갱신
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other. gameObject);
            Die();
            Destroy(transform.parent.gameObject, 2f);
        }
    }


    void Die()
    {
        // 모든 자식 오브젝트를 가져옴
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Debug.Log(child.gameObject.name);

            // Rigidbody 추가
            Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();

            // 랜덤한 방향으로 힘 적용
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(0.5f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;

            float explosionForce = Random.Range(50f, 100f);
            rb.AddForce(randomDirection * explosionForce);

 
            rb.AddTorque(new Vector3(360, 360, 360));
        }

    }
}

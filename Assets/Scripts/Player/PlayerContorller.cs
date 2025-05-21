using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ───── 기본 설정 ─────
    [Header("Player")]
    public float moveSpeed = 1f;
    public float rotateSpeed = 100f;
    public float jumpPower = 5f;

    [Header("Bullet")]
    public float bulletMovePower = 1f;
    public Transform bulletPos;
    public GameObject bullet;

    private Rigidbody rb;
    private bool isGrounded = true;

    // ───── 애니메이션 ─────
    private Animator animator;


    // ───── 사다리 ─────
    private bool isLadder = false;


    // ──────────────────────
    void Start()
    {
        // Rigidbody 확인‧자동추가
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

        // Animator 캐시
        animator = GetComponent<Animator>();
    }

    // ──────────────────────
    void Update()
    {
        if (!animator) return;   // Animator 없으면 조기 종료

        // ───────── 이동, 회전 ─────────
        if (animator.GetBool("IsAttack") == false)
        {
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            // 회전
            transform.Rotate(0f, h * rotateSpeed * Time.deltaTime, 0f);


            // 전진·후진
            if (isLadder == false)
            {
                rb.useGravity = true;
                Vector3 move = transform.forward * v * moveSpeed * Time.deltaTime;
                rb.MovePosition(rb.position + move);
            }
            else
            {
                rb.useGravity = false;
                 Vector3 move = transform.up * v * moveSpeed * Time.deltaTime;
                rb.MovePosition(rb.position + move);
            }

                // 애니메이션 Speed 갱신 (앞·뒤 이동만 반영)
                animator.SetFloat("Speed", Mathf.Abs(v));
        }

        // ───────── 공격 입력 ─────────
        if (Input.GetMouseButton(0))
        {
            if(animator.GetBool("IsAttack") == false )
                animator.SetBool("IsAttack", true);   // Bool ON
        }

        if( Input.GetMouseButtonDown(1))
        {
            GameObject go =  Instantiate(bullet, bulletPos.position, transform.rotation);
            go.GetComponent<Rigidbody>().AddForce(transform.forward * bulletMovePower);
        }

        // ───────── 점프 입력 ─────────
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("IsJump", true);
        }
    }

    public void BulletFire()
    {
        GameObject go = Instantiate(bullet, bulletPos.position, transform.rotation);
        go.GetComponent<Rigidbody>().AddForce(transform.forward * bulletMovePower, ForceMode.VelocityChange);
    }

    // ───── 충돌 처리 ─────
    private void OnCollisionEnter(Collision collision)
    {
        // “Floor” 태그가 붙은 바닥과 접촉하면 착지로 간주
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            animator.SetBool("IsJump", false);
        }
    }

    // ───── 트리거 디버그 ─────
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger : {other.gameObject.name}");

        if( other.gameObject.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            isLadder = false;
        }
    }
}

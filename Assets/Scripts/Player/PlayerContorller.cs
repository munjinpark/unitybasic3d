using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 선언과 동시에 초기화, 인스펙터 초기화, start함수 초기화 비교
    public float moveSpeed = 1f;
    public float rotateSpeed = 100f;

    public float jumpPower = 5f;
    private Rigidbody rb;
    private bool isGrounded = true;


    // 추가코드
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //moveSpeed = 10f;
        //rotateSpeed = 50f;

        rb = GetComponent<Rigidbody>();
        // Rigidbody 없을 경우 자동 추가 가능하도록 처리
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }


        // 추가코드
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 추가코드
        bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsName("attack");

        // 이동 및 회전
        if( !isAttacking)
        {
            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");

            // 실제 값들 확인해보고 GetAxis와 GetAxisRaw 차이점 비교
            //Debug.Log(vertical);
            //Debug.Log(transform.forward);

            // 회전
            transform.Rotate(0, horizontal * rotateSpeed * Time.deltaTime, 0);

            // 전진/후진
            Vector3 movement = transform.forward * vertical * moveSpeed * Time.deltaTime;
            //transform.position += movement;
            rb.MovePosition(rb.position + movement);

            // 추가코드
            if (animator)
            {
                float speed = Mathf.Abs(vertical); // 앞뒤 움직임만 기준
                animator.SetFloat("Speed", speed);
            }
        }
        

        // 공격 (마우스 왼쪽 클릭)
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            if (animator)
                animator.SetTrigger("IsAttack");
        }


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;


            // 추가코드
            if (animator)
                animator.SetBool("IsJump", true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 바닥과 충돌하면 착지로 판단
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;


            // 추가코드
            if (animator)
                animator.SetBool("IsJump", false);
        }
    }
}

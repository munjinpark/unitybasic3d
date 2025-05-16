using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 선언과 동시에 초기화, 인스펙터 초기화, start함수 초기화 비교
    public float moveSpeed = 1f;
    public float rotateSpeed = 100f;

    // 추가코드
    public float jumpPower = 5f;
    private Rigidbody rb;
    private bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        //moveSpeed = 10f;
        //rotateSpeed = 50f;

        // 추가코드
        rb = GetComponent<Rigidbody>();
        // Rigidbody 없을 경우 자동 추가 가능하도록 처리
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 이동 및 회전
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        // 실제 값들 확인해보고 GetAxis와 GetAxisRaw 차이점 비교
        //Debug.Log(vertical);
        //Debug.Log(transform.forward);

        // 회전
        transform.Rotate(0, horizontal * rotateSpeed * Time.deltaTime, 0);

        // 전진/후진
        Vector3 movement = transform.forward * vertical * moveSpeed * Time.deltaTime;

        // 추가코드
        //transform.position += movement;
        rb.MovePosition(rb.position + movement);



        // 추가코드
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    // 추가코드
    void OnCollisionEnter(Collision collision)
    {
        // 바닥과 충돌하면 착지로 판단
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 선언과 동시에 초기화, 인스펙터 초기화, start함수 초기화 비교
    public float moveSpeed = 1f;
    public float rotateSpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        //moveSpeed = 10f;
        //rotateSpeed = 50f;
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
        transform.position += movement;
    }
}

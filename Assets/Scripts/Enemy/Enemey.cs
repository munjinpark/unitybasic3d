using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemey : MonoBehaviour
{
    public Transform[] movePos;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float reachDistance = 0.2f;
    // 추가코드
    public float waitTime = 3f;

    private Transform currMovePos;
    // 추가코드
    private bool isWaiting = false;

    void Start()
    {
        if (movePos.Length > 0)
        {
            SetNewDestination();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 추가코드
        if (currMovePos == null || isWaiting) return;

        // 목표 방향으로 부드럽게 회전
        Vector3 direction = (currMovePos.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // 현재 위치에서 목표 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, currMovePos.position, moveSpeed * Time.deltaTime);

        // 목표 위치에 거의 도착하면 새로운 목적지 선택
        if (Vector3.Distance(transform.position, currMovePos.position) < reachDistance)
        {
            //SetNewDestination();

            // 추가코드
            isWaiting = true;
            Invoke("WaitAndMove", waitTime);
        }
    }

    // 추가코드
    void  WaitAndMove()
    {
        SetNewDestination();
        isWaiting = false;
    }

    void SetNewDestination()
    {
        if (movePos.Length == 0) return;

        Transform newPos;
        do
        {
            newPos = movePos[Random.Range(0, movePos.Length)];
        } while (newPos == currMovePos && movePos.Length > 1); // 같은 위치 반복 방지

        currMovePos = newPos;
        //transform.LookAt(currMovePos);
    }
}

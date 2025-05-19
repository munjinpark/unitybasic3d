using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemey : MonoBehaviour
{
    public Transform[] movePos;
    public float moveSpeed = 3f;
    public float reachDistance = 0.2f;

    private Transform currMovePos;

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
        if (currMovePos == null) return;

        // 현재 위치에서 목표 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, currMovePos.position, moveSpeed * Time.deltaTime);

        // 목표 위치에 거의 도착하면 새로운 목적지 선택
        if (Vector3.Distance(transform.position, currMovePos.position) < reachDistance)
        {
            SetNewDestination();
        }
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
        transform.LookAt(currMovePos);
    }
}

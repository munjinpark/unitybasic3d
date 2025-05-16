using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ����� ���ÿ� �ʱ�ȭ, �ν����� �ʱ�ȭ, start�Լ� �ʱ�ȭ ��
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
        // �̵� �� ȸ��
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        // ���� ���� Ȯ���غ��� GetAxis�� GetAxisRaw ������ ��
        //Debug.Log(vertical);
        //Debug.Log(transform.forward);

        // ȸ��
        transform.Rotate(0, horizontal * rotateSpeed * Time.deltaTime, 0);

        // ����/����
        Vector3 movement = transform.forward * vertical * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }
}

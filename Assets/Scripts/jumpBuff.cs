using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpBuff : MonoBehaviour
{
    public float jumpPower = 10;
    public float rotationSpeed = 90f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}

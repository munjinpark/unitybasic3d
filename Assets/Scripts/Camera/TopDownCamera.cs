using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;
    public float height = 10f;
    public float distance = 10f;


    // Update is called once per frame
    void LateUpdate()
    {
        if (target)
        {
            transform.position = new Vector3(target.position.x, target.position.y + height, target.position.z - distance);
            transform.LookAt(target);
        }
    }
}

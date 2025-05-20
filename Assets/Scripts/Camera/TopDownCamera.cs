using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Rig Settings")]
    public float height = 10f;   // 위로 띄우는 높이
    public float distance = 10f;   // 뒤로 떨어지는 거리

    [Header("Smoothing")]
    public float posSmoothSpeed = 5f;   // 위치 보간 속도
    public float rotSmoothSpeed = 8f;   // 회전 보간 속도

    void LateUpdate()
    {
        if (!target) return;

        /* 1) 원하는 카메라 위치 : 타깃 뒤쪽(-forward) + 위쪽(height) */
        Vector3 desiredPos = target.position
                           - target.forward * distance   // 뒤로
                           + Vector3.up * height;      // 위로

        /* 2) 위치를 Lerp 로 서서히 이동 */
        transform.position = Vector3.Lerp(
            transform.position, desiredPos, posSmoothSpeed * Time.deltaTime);

        /* 3) 타깃을 바라보도록 원하는 회전 계산 */
        Quaternion desiredRot = Quaternion.LookRotation(
            target.position - transform.position, Vector3.up);

        /* 4) 회전을 Slerp 로 서서히 보간 */
        transform.rotation = Quaternion.Slerp(
            transform.rotation, desiredRot, rotSmoothSpeed * Time.deltaTime);
    }
}

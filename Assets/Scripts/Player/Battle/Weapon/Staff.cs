using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] private Transform characterTransform; // 캐릭터의 Transform
    [SerializeField] private Vector3 offset; // 캐릭터에 대한 무기의 위치 오프셋
    [SerializeField] private float rotationSpeed = 50f; // 회전 속도
    [SerializeField] private float floatSpeed = 0.5f; // 떠다니는 속도
    [SerializeField] private float floatHeight = 0.5f; // 떠다니는 높이

    private void Update()
    {
        // 무기를 가로로 돌립니다.
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // 무기를 둥둥 떠다니게 합니다.
        float newY = offset.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        Vector3 floatingOffset = new Vector3(offset.x, newY, offset.z);

        // 캐릭터의 위치와 로컬 오프셋을 이용해 무기의 새로운 위치를 설정합니다.
        transform.position = characterTransform.TransformPoint(floatingOffset);
    }
}

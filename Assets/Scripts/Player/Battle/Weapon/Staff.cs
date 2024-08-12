using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] private Transform characterTransform; // ĳ������ Transform
    [SerializeField] private Vector3 offset; // ĳ���Ϳ� ���� ������ ��ġ ������
    [SerializeField] private float rotationSpeed = 50f; // ȸ�� �ӵ�
    [SerializeField] private float floatSpeed = 0.5f; // ���ٴϴ� �ӵ�
    [SerializeField] private float floatHeight = 0.5f; // ���ٴϴ� ����

    private void Update()
    {
        // ���⸦ ���η� �����ϴ�.
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // ���⸦ �յ� ���ٴϰ� �մϴ�.
        float newY = offset.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        Vector3 floatingOffset = new Vector3(offset.x, newY, offset.z);

        // ĳ������ ��ġ�� ���� �������� �̿��� ������ ���ο� ��ġ�� �����մϴ�.
        transform.position = characterTransform.TransformPoint(floatingOffset);
    }
}

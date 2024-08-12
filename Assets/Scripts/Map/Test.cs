using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    [Header("�� UI")]
    [SerializeField] private GameObject mapUI; // Screen Space - Overlay ���� ������ Canvas

    [Header("������ ī�޶�")]
    [SerializeField] private Camera renderCamera; // �������� ����ϴ� ī�޶�

    [Header("���÷��� ī�޶�")]
    [SerializeField] private Camera displayCamera; // �������� �̹����� �����ִ� ī�޶�

    [Header("ĳ���� Transform")]
    private Transform playerTransform; // �÷��̾� ĳ������ Transform

    [Header("�� �� ���� �Ӽ�")]
    [SerializeField] private float zoomSpeed;  // �� ��/�ƿ� �ӵ�
    [SerializeField] private float minZoom;  // �ּ� �� ����
    [SerializeField] private float maxZoom;  // �ִ� �� ����

    [Header("�� ���� ���� �Ӽ�")]
    [SerializeField] private float mapWidth;  // ���� ���� ũ��
    [SerializeField] private float mapHeight;  // ���� ���� ũ��
    [SerializeField] private float boundaryPadding;  // ��� �е�

    private bool isOn = false; // �� UI�� Ȱ��ȭ�Ǿ� �ִ��� ����
    private Vector3 initialCameraPosition; // ī�޶��� �ʱ� ��ġ
    private Vector3 targetCameraPosition; // ī�޶��� ��ǥ ��ġ
    private bool isOutOfBounds = false; // ī�޶� �� ��踦 ������� ����

    void Start()
    {
        InitValue(); // �ʱⰪ ����
    }

    private void InitValue()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� ĳ������ Transform�� ������

        // Canvas�� Screen Space - Overlay ���� �����Ǿ� �ִ��� Ȯ��
        Canvas canvas = mapUI.GetComponent<Canvas>();
        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            Debug.LogWarning("mapUI Canvas�� Render Mode�� Screen Space - Overlay�� �����Ͻʽÿ�.");
        }

        mapUI.SetActive(false); // ó���� UI�� ��Ȱ��ȭ�մϴ�.
    }

    void Update()
    {
        ToggleMap(); // �� UI�� Ȱ��ȭ/��Ȱ��ȭ�� ���

        if (isOn) // �� UI�� Ȱ��ȭ�Ǿ� ������
        {
            HandleMapMovement(); // �� �̵� ó��
            HandleMapZoom(); // �� �� ó��
            ApplyElasticity(); // ź�� ȿ�� ����
        }

        if (Input.GetKeyDown(KeyCode.P)) // P Ű�� ������
        {
            Debug.Log("Render Camera Position: " + renderCamera.transform.position);
            Debug.Log("Display Camera Position: " + displayCamera.transform.position);
        }
    }

    void ToggleMap()
    {
        //if (Input.GetKeyDown(KeyCode.M)) // M Ű�� ������
        //{
        //    if (!isOn && PanelActivation.Instance.IsAnyPanelActive()) return; // �� UI�� ��Ȱ��ȭ�Ǿ� �ְ� �ٸ� �г��� Ȱ��ȭ�Ǿ� ������ ����

        //    isOn = !isOn; // Ȱ��ȭ ���¸� ���
        //    mapUI.SetActive(isOn); // UI Ȱ��ȭ/��Ȱ��ȭ
        //    PanelActivation.Instance.SetPanelActive(isOn); // �г� Ȱ��ȭ/��Ȱ��ȭ

        //    if (isOn)
        //    {
        //        CenterMapOnPlayer(); // �� UI�� Ȱ��ȭ�Ǹ� ���� �÷��̾� ĳ���� �߾����� �̵�
        //    }
        //}
    }

    void CenterMapOnPlayer()
    {
        Vector3 playerPos = playerTransform.position; // �÷��̾� ĳ������ ��ġ�� ������
        targetCameraPosition = new Vector3(playerPos.x, renderCamera.transform.position.y, playerPos.z); // ��ǥ ��ġ�� �÷��̾� ĳ������ ��ġ�� ����
        renderCamera.transform.position = targetCameraPosition; // ������ ī�޶��� ��ġ�� ��ǥ ��ġ�� �̵�
    }

    void HandleMapMovement()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) // ���콺 ���� ��ư�� ������ UI ���� ������
        {
            float h = -Input.GetAxis("Mouse X"); // ���� �̵��� ���
            float v = -Input.GetAxis("Mouse Y"); // ���� �̵��� ���
            Vector3 move = new Vector3(h, v, 0); // �̵� ���� ����

            initialCameraPosition += move; // ī�޶� ��ġ�� �̵�
            CheckBounds(); // ��� üũ
        }
    }

    void HandleMapZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // ���콺 ��ũ�� �Է��� ������
        if (scroll != 0.0f) // ��ũ�� �Է��� ������
        {
            float newSize = renderCamera.orthographicSize - scroll * zoomSpeed; // ���ο� �� ���� ���
            renderCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom); // �� ������ �ּ�/�ִ� �� ������ ����
        }
    }

    void CheckBounds()
    {
        float halfWidth = (mapWidth - boundaryPadding) / 2f; // ���� �� ���� ũ�� ���
        float halfHeight = (mapHeight - boundaryPadding) / 2f; // ���� �� ���� ũ�� ���

        if (targetCameraPosition.x < -halfWidth || targetCameraPosition.x > halfWidth || targetCameraPosition.y < -halfHeight || targetCameraPosition.y > halfHeight) // ī�޶� �� ��踦 �����
        {
            isOutOfBounds = true; // ��踦 ������� ǥ��
        }
        else
        {
            isOutOfBounds = false; // ��踦 ����� �ʾ����� ǥ��
        }
    }

    void ApplyElasticity()
    {
        if (isOutOfBounds) // ī�޶� �� ��踦 �����
        {
            float halfWidth = (mapWidth - boundaryPadding) / 2f; // ���� �� ���� ũ�� ���
            float halfHeight = (mapHeight - boundaryPadding) / 2f; // ���� �� ���� ũ�� ���

            // ���ѵ� ��ġ�� ����
            Vector3 clampedPosition = new Vector3(targetCameraPosition.x, targetCameraPosition.y, displayCamera.transform.position.z);

            // ī�޶��� x ��ǥ�� �� ���� ����
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -halfWidth, halfWidth);
            // ī�޶��� y ��ǥ�� �� ���� ����
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -halfHeight, halfHeight);

            // ���÷��� ī�޶��� ��ġ�� ���ѵ� ��ġ�� �̵�
            displayCamera.transform.position = clampedPosition;
        }
        else
        {
            // ī�޶� �� ��踦 ����� �ʾ����� ���÷��� ī�޶��� ��ġ�� ��ǥ ��ġ�� �̵�
            displayCamera.transform.position = new Vector3(targetCameraPosition.x, targetCameraPosition.y, displayCamera.transform.position.z);
        }
    }
}

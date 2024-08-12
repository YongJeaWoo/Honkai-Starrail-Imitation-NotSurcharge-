using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    [Header("맵 UI")]
    [SerializeField] private GameObject mapUI; // Screen Space - Overlay 모드로 설정된 Canvas

    [Header("랜더링 카메라")]
    [SerializeField] private Camera renderCamera; // 랜더링을 담당하는 카메라

    [Header("디스플레이 카메라")]
    [SerializeField] private Camera displayCamera; // 랜더링된 이미지를 보여주는 카메라

    [Header("캐릭터 Transform")]
    private Transform playerTransform; // 플레이어 캐릭터의 Transform

    [Header("맵 줌 관련 속성")]
    [SerializeField] private float zoomSpeed;  // 줌 인/아웃 속도
    [SerializeField] private float minZoom;  // 최소 줌 레벨
    [SerializeField] private float maxZoom;  // 최대 줌 레벨

    [Header("맵 영역 관련 속성")]
    [SerializeField] private float mapWidth;  // 맵의 가로 크기
    [SerializeField] private float mapHeight;  // 맵의 세로 크기
    [SerializeField] private float boundaryPadding;  // 경계 패딩

    private bool isOn = false; // 맵 UI가 활성화되어 있는지 여부
    private Vector3 initialCameraPosition; // 카메라의 초기 위치
    private Vector3 targetCameraPosition; // 카메라의 목표 위치
    private bool isOutOfBounds = false; // 카메라가 맵 경계를 벗어났는지 여부

    void Start()
    {
        InitValue(); // 초기값 설정
    }

    private void InitValue()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 캐릭터의 Transform을 가져옴

        // Canvas가 Screen Space - Overlay 모드로 설정되어 있는지 확인
        Canvas canvas = mapUI.GetComponent<Canvas>();
        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            Debug.LogWarning("mapUI Canvas의 Render Mode를 Screen Space - Overlay로 설정하십시오.");
        }

        mapUI.SetActive(false); // 처음에 UI를 비활성화합니다.
    }

    void Update()
    {
        ToggleMap(); // 맵 UI의 활성화/비활성화를 토글

        if (isOn) // 맵 UI가 활성화되어 있으면
        {
            HandleMapMovement(); // 맵 이동 처리
            HandleMapZoom(); // 맵 줌 처리
            ApplyElasticity(); // 탄성 효과 적용
        }

        if (Input.GetKeyDown(KeyCode.P)) // P 키를 누르면
        {
            Debug.Log("Render Camera Position: " + renderCamera.transform.position);
            Debug.Log("Display Camera Position: " + displayCamera.transform.position);
        }
    }

    void ToggleMap()
    {
        //if (Input.GetKeyDown(KeyCode.M)) // M 키를 누르면
        //{
        //    if (!isOn && PanelActivation.Instance.IsAnyPanelActive()) return; // 맵 UI가 비활성화되어 있고 다른 패널이 활성화되어 있으면 리턴

        //    isOn = !isOn; // 활성화 상태를 토글
        //    mapUI.SetActive(isOn); // UI 활성화/비활성화
        //    PanelActivation.Instance.SetPanelActive(isOn); // 패널 활성화/비활성화

        //    if (isOn)
        //    {
        //        CenterMapOnPlayer(); // 맵 UI가 활성화되면 맵을 플레이어 캐릭터 중앙으로 이동
        //    }
        //}
    }

    void CenterMapOnPlayer()
    {
        Vector3 playerPos = playerTransform.position; // 플레이어 캐릭터의 위치를 가져옴
        targetCameraPosition = new Vector3(playerPos.x, renderCamera.transform.position.y, playerPos.z); // 목표 위치를 플레이어 캐릭터의 위치로 설정
        renderCamera.transform.position = targetCameraPosition; // 랜더링 카메라의 위치를 목표 위치로 이동
    }

    void HandleMapMovement()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) // 마우스 왼쪽 버튼을 누르고 UI 위에 없으면
        {
            float h = -Input.GetAxis("Mouse X"); // 가로 이동량 계산
            float v = -Input.GetAxis("Mouse Y"); // 세로 이동량 계산
            Vector3 move = new Vector3(h, v, 0); // 이동 벡터 생성

            initialCameraPosition += move; // 카메라 위치를 이동
            CheckBounds(); // 경계 체크
        }
    }

    void HandleMapZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // 마우스 스크롤 입력을 가져옴
        if (scroll != 0.0f) // 스크롤 입력이 있으면
        {
            float newSize = renderCamera.orthographicSize - scroll * zoomSpeed; // 새로운 줌 레벨 계산
            renderCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom); // 줌 레벨을 최소/최대 줌 레벨로 제한
        }
    }

    void CheckBounds()
    {
        float halfWidth = (mapWidth - boundaryPadding) / 2f; // 맵의 반 가로 크기 계산
        float halfHeight = (mapHeight - boundaryPadding) / 2f; // 맵의 반 세로 크기 계산

        if (targetCameraPosition.x < -halfWidth || targetCameraPosition.x > halfWidth || targetCameraPosition.y < -halfHeight || targetCameraPosition.y > halfHeight) // 카메라가 맵 경계를 벗어나면
        {
            isOutOfBounds = true; // 경계를 벗어났음을 표시
        }
        else
        {
            isOutOfBounds = false; // 경계를 벗어나지 않았음을 표시
        }
    }

    void ApplyElasticity()
    {
        if (isOutOfBounds) // 카메라가 맵 경계를 벗어나면
        {
            float halfWidth = (mapWidth - boundaryPadding) / 2f; // 맵의 반 가로 크기 계산
            float halfHeight = (mapHeight - boundaryPadding) / 2f; // 맵의 반 세로 크기 계산

            // 제한된 위치를 생성
            Vector3 clampedPosition = new Vector3(targetCameraPosition.x, targetCameraPosition.y, displayCamera.transform.position.z);

            // 카메라의 x 좌표를 맵 경계로 제한
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -halfWidth, halfWidth);
            // 카메라의 y 좌표를 맵 경계로 제한
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -halfHeight, halfHeight);

            // 디스플레이 카메라의 위치를 제한된 위치로 이동
            displayCamera.transform.position = clampedPosition;
        }
        else
        {
            // 카메라가 맵 경계를 벗어나지 않았으면 디스플레이 카메라의 위치를 목표 위치로 이동
            displayCamera.transform.position = new Vector3(targetCameraPosition.x, targetCameraPosition.y, displayCamera.transform.position.z);
        }
    }
}

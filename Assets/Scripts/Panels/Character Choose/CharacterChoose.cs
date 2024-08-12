using UnityEngine;
using UnityEngine.UI;

public class CharacterChoose : MonoBehaviour
{
    [Header("플레이어 버튼 배열")]
    [SerializeField] private Button[] characterButtons;

    [Header("플레이어 버튼 배경 배열")]
    [SerializeField] private Image[] buttonBGs;

    [Header("캐릭터 랜더링 카메라 오브젝트")]
    [SerializeField] private GameObject drawCameraObject;

    [Header("파티 패널 컴포넌트")]
    [SerializeField] private PartyPanel partyPanel;

    [Header("Skybox Material")]
    [SerializeField] private Material skyboxMaterial;

    [Header("캐릭터 모델링 카메라")]
    [SerializeField] private Camera modelCamera;

    [Header("캐릭터 표현 이미지")]
    [SerializeField] private GameObject modelImage;

    [Header("모델링 캐릭터 프리팹")]
    [SerializeField] private GameObject[] modelPlayerPrefabs;

    private Material originMaterial;            // skybox 원래 Material
    private GameObject player;                  // 현재 활성화된 player
    private int buttonIndex, characterIndex;    // partyPanel에 넘겨줄 버튼과 캐릭터 Index

    #region 캐릭터 카메라 위치 변경 관련 변수
    private Vector3 previousMousePosition;
    private float rotationSpeed = 5.0f;
    private float zoomSpeed = 2.0f;
    private float minZoom = 1.0f;
    private float maxZoom = 3.0f;
    #endregion

    public int ButtonIndex { get => buttonIndex; set => buttonIndex = value; }

    private void Update()
    {
        //HandleMouseDrag();
    }

    private void OnEnable()
    {
        InitValue();

        var light = FindObjectOfType<Test_Light>();
        light.PlaceLight(true);
    }

    private void OnDisable()
    {
        RenderSettings.skybox = originMaterial;
        Destroy(player);

        var light = FindObjectOfType<Test_Light>();
        light.PlaceLight(false);
    }

    private void InitValue()
    {
        characterIndex = -1;
        ChangeButtonClick(0);
        originMaterial = RenderSettings.skybox;
        RenderSettings.skybox = skyboxMaterial;
    }

    public void ChangeButtonClick(int index)
    {
        if (characterIndex == index) return;

        characterIndex = index;
        ModelCharacterInstantiate(index);
    }

    public void CharacterSaveButtonClick()
    {
        partyPanel.ButtonImageChange(buttonIndex, characterIndex);
        gameObject.SetActive(false);
    }

    public void QuitButtonClick()
    {
        RenderSettings.skybox = originMaterial;
        gameObject.SetActive(false);
    }

    private void ModelCharacterInstantiate(int index)
    {
        if (player != null)
        {
            Destroy(player);
        }

        player = Instantiate(modelPlayerPrefabs[index], new Vector3(0f, 0f, 0f), Quaternion.identity);
        //modelCamera.transform.position = new Vector3(modelCamera.transform.position.x, player.transform.GetChild(0).transform.position.y, modelCamera.transform.position.z);
    }

    //#region 캐릭터 모델링 카메라 이동
    //private void HandleMouseDrag()
    //{
    //    var panelActivate = PopupManager.Instance.GetPanelActivation();

    //    if (panelActivate.IsAnyPanelActive() && Input.GetKeyDown(KeyCode.L)) return;

    //    if (Input.GetMouseButtonDown(0)) previousMousePosition = Input.mousePosition;

    //    if (Input.GetMouseButton(0))
    //    {
    //        Vector3 delta = Input.mousePosition - previousMousePosition;
    //        float rotationY = delta.x * rotationSpeed * Time.deltaTime;

    //        modelCamera.transform.RotateAround(player.transform.position, Vector3.up, rotationY);

    //        previousMousePosition = Input.mousePosition;
    //    }

    //    HandleMouseScroll();
    //}

    //private void HandleMouseScroll()
    //{
    //    float scroll = Input.GetAxis("Mouse ScrollWheel");
    //    if (scroll != 0.0f)
    //    {
    //        float newSize = modelCamera.orthographicSize - scroll * zoomSpeed;
    //        modelCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    //    }
    //}
    //#endregion
}
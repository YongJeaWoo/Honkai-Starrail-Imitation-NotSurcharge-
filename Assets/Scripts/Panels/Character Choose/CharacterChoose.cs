using UnityEngine;
using UnityEngine.UI;

public class CharacterChoose : MonoBehaviour
{
    [Header("�÷��̾� ��ư �迭")]
    [SerializeField] private Button[] characterButtons;

    [Header("�÷��̾� ��ư ��� �迭")]
    [SerializeField] private Image[] buttonBGs;

    [Header("ĳ���� ������ ī�޶� ������Ʈ")]
    [SerializeField] private GameObject drawCameraObject;

    [Header("��Ƽ �г� ������Ʈ")]
    [SerializeField] private PartyPanel partyPanel;

    [Header("Skybox Material")]
    [SerializeField] private Material skyboxMaterial;

    [Header("ĳ���� �𵨸� ī�޶�")]
    [SerializeField] private Camera modelCamera;

    [Header("ĳ���� ǥ�� �̹���")]
    [SerializeField] private GameObject modelImage;

    [Header("�𵨸� ĳ���� ������")]
    [SerializeField] private GameObject[] modelPlayerPrefabs;

    private Material originMaterial;            // skybox ���� Material
    private GameObject player;                  // ���� Ȱ��ȭ�� player
    private int buttonIndex, characterIndex;    // partyPanel�� �Ѱ��� ��ư�� ĳ���� Index

    #region ĳ���� ī�޶� ��ġ ���� ���� ����
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

    //#region ĳ���� �𵨸� ī�޶� �̵�
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
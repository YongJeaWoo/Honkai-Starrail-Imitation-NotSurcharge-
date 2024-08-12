using TMPro;
using UnityEngine;

public class CharacterStatPanel : MonoBehaviour
{
    [Header("���ݷ� Text")]
    [SerializeField] private TextMeshProUGUI damageText;

    [Header("HP Text")]
    [SerializeField] private TextMeshProUGUI HpText;

    [Header("�� ĳ���� ������")]
    [SerializeField] private GameObject[] modelPlayerPrefabs;

    [Header("�� ������ ī�޶� ������Ʈ")]
    [SerializeField] private GameObject modelCamera;

    [Header("������ Skybox Material")]
    [SerializeField] private Material skyboxMaterial;

    private CharacterStateSOJ currentSOJ;

    private GameObject modelPlayer;
    private int currentCharacterIndex;
    private Material originMaterial;

    private void OnEnable()
    {
        InitValue();
    }

    private void InitValue()
    {
        var light = FindObjectOfType<Test_Light>();
        light.PlaceLight(true);

        currentCharacterIndex = -1;
        SetStatUI(0);
        CharacterButtonClick(0);
        originMaterial = RenderSettings.skybox;
        RenderSettings.skybox = skyboxMaterial;
    }

    private void OnDisable()
    {
        RenderSettings.skybox = originMaterial;
        Destroy(modelPlayer);

        var light = FindObjectOfType<Test_Light>();
        light.PlaceLight(false);
    }

    public void CharacterButtonClick(int index)
    {
        SetStatUI(index);
        ModelCharacterInstantiate(index);
    }
    
    // ����â UI ����
    private void SetStatUI(int index)
    {
        // TODO : ĳ���� ������ ���� �� �ҷ������� Ȯ���ؾ� ��
        DataPlayer dataPlayer = modelPlayerPrefabs[index].GetComponent<DataPlayer>();

        if (dataPlayer != null)
        {
            currentSOJ = dataPlayer.GetSOJData();

            if (currentSOJ != null)
            {
                damageText.text = "Damage : " + currentSOJ.Damage.ToString();
                HpText.text = "HP : " + currentSOJ.Maxhp.ToString();
            }
        }
    }

    // ǥ���� ĳ���� ����
    private void ModelCharacterInstantiate(int index)
    {
        if (currentCharacterIndex == index) return;

        if(modelPlayer != null)
        {
            Destroy(modelPlayer);
        }

        modelPlayer = Instantiate(modelPlayerPrefabs[index], new Vector3(0f, 0f, 0f), Quaternion.identity);
        modelCamera.transform.position = new Vector3(modelCamera.transform.position.x, modelPlayer.transform.GetChild(0).transform.position.y, modelCamera.transform.position.z);
        currentCharacterIndex = index;
    }
}

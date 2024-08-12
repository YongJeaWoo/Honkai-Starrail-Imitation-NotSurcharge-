using TMPro;
using UnityEngine;

public class CharacterStatPanel : MonoBehaviour
{
    [Header("공격력 Text")]
    [SerializeField] private TextMeshProUGUI damageText;

    [Header("HP Text")]
    [SerializeField] private TextMeshProUGUI HpText;

    [Header("모델 캐릭터 프리팹")]
    [SerializeField] private GameObject[] modelPlayerPrefabs;

    [Header("모델 렌더링 카메라 오브젝트")]
    [SerializeField] private GameObject modelCamera;

    [Header("변경할 Skybox Material")]
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
    
    // 스탯창 UI 설정
    private void SetStatUI(int index)
    {
        // TODO : 캐릭터 데이터 정보 잘 불러오는지 확인해야 함
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

    // 표현할 캐릭터 생성
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

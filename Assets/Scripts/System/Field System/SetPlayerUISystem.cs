using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerUISystem : MonoBehaviour
{
    [Header("플레이어 스킬 UI")]
    [SerializeField] private GameObject attackUIArea;
    [Header("플레이어 정보 UI")]
    [SerializeField] private Image infoImage;
    [Header("캐릭터 UI")]
    [SerializeField] private FieldCharacterArea[] characterAreas;

    private SetPlayerSystem setPlayerSystem;

    public FieldCharacterArea[] CharacterAreas { get => characterAreas; set => characterAreas = value; }

    private void Awake()
    {
        SetSOJ();
    }

    public void UISet()
    {
        for (int i = 0; i < CharacterAreas.Length; i++)
        {
            int temp = setPlayerSystem.PartyIndex[i];
            CharacterAreas[i].SetCharacterUI(setPlayerSystem.FieldPlayers[temp]);
        }
    }

    public void SetSOJ()
    {
        setPlayerSystem = GetComponent<SetPlayerSystem>();
        setPlayerSystem.PartyIndex = PlayerDataManager.Instance.GetPlayerBattler();

        CharacterStateSOJ[] sojs = PlayerDataManager.Instance.GetPlayerSOJData();

        for(int i=0;i<CharacterAreas.Length;i++)
        {
            CharacterAreas[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < setPlayerSystem.PartyIndex.Count; i++)
        {
            CharacterAreas[i].gameObject.SetActive(true);
            CharacterAreas[i].Soj = sojs[setPlayerSystem.PartyIndex[i]];
        }
    }

    public void SetSkillAttackUIPlayer(GameObject player)
    {
        var playerSkillUI = player.GetComponentInChildren<CharacterButtonInfo>();
        playerSkillUI.AttackUIArea = attackUIArea;
    }

    public void SetInfoUIPlayer(GameObject player)
    {
        var fieldObj = player.GetComponentInChildren<FieldComponent>();
        var detective = fieldObj.GetComponentInChildren<Detective>();

        detective.SetInfoImage(infoImage);
    }

    public void SetSkillButton(GameObject player)
    {
        var pState = player.GetComponentInChildren<FieldState>();
        var soj = pState.GetStateInfo();

        var skillParent = attackUIArea.transform.GetChild(1).GetChild(0);
        var basicAttackParent = attackUIArea.transform.GetChild(2).GetChild(0);
        var skillTextParent = attackUIArea.transform.GetChild(1).GetChild(1);

        var skillButtonIcon = skillParent.GetChild(0);
        var basicAttackButtonIcon = basicAttackParent.GetChild(0);

        var skillImage = skillButtonIcon.GetComponent<Image>();
        var basicAttackImage = basicAttackButtonIcon.GetComponent<Image>();

        var skillText = skillTextParent.GetChild(0).GetComponent<TextMeshProUGUI>();

        skillText.text = soj.GetSkillText();
        skillImage.sprite = soj.GetSkillIcon();
        basicAttackImage.sprite = soj.GetBasicAttackIcon();
    }

    public Image GetInfoImage() => infoImage;
}


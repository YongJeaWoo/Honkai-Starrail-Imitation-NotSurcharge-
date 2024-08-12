using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyPanel : MonoBehaviour
{
    [Header("사용 가능 파티 모음")]
    [SerializeField] private Button[] collectsParty;

    [Header("선택한 파티 숫자")]
    [SerializeField] private TextMeshProUGUI partyText;

    [Header("파티 캐릭터 버튼 배열")]
    [SerializeField] private Button[] charBtns;

    [Header("플레이어 세팅 시스템")]
    [SerializeField] private SetPlayerSystem changeSystem;

    [Header("플레이어 UI 세팅 시스템")]
    [SerializeField] private SetPlayerUISystem UISystem;

    [Header("캐릭터 선택 컨트롤러 컴포넌트")]
    [SerializeField] private CharacterChooseController controller;

    [SerializeField] private Sprite originSprite;

    private readonly string panelText = $"Player Info Panel";
    private readonly string notSelectText = $"캐릭터를 선택하지 않았습니다.";

    private bool isPartyChange = false;

    private int currentPartyIndex;

    private List<List<int>> partyPlayerIndexes;

    public bool IsPartyChange { get => isPartyChange; set => isPartyChange = value; }
    public SetPlayerSystem ChangeSystem { get => changeSystem; set => changeSystem = value; }
    public Button[] CharBtns { get => charBtns; set => charBtns = value; }

    public void Start()
    {
        SetPartyInitialize();
    }

    private void OnEnable()
    {
        ClickCollectButton(controller.OriginPartyIndex);
    }

    private void SetPartyInitialize()
    {
        partyText.text = $"파티 01";

        partyPlayerIndexes = new List<List<int>>();

        for (int i = 0; i < collectsParty.Length; i++)
        {
            partyPlayerIndexes.Add(new List<int>());
            for (int j = 0; j < CharBtns.Length; j++)
            {
                partyPlayerIndexes[i].Add(-1);
            }
        }

        currentPartyIndex = 0;

        var button = collectsParty[currentPartyIndex].GetComponent<PartyButton>();
        button.SelectButton();

        LoadParty(0);
    }

    public void ClickCollectButton(int index)
    {
        if (currentPartyIndex == index) return;

        currentPartyIndex = index;
        partyText.text = $"파티 {index + 1:D2}";

        var maxParty = collectsParty.Length;

        var button = collectsParty[currentPartyIndex].GetComponent<PartyButton>();
        button.SelectButton();

        for (int i = 0; i < maxParty; i++)
        {
            if (i == currentPartyIndex) continue;

            var otherButtons = collectsParty[i].GetComponent<PartyButton>();
            otherButtons.DeselectButton();
        }

        LoadParty(index);
    }

    public void ButtonImageChange(int buttonIndex, int characterIndex)
    {
        if (partyPlayerIndexes[currentPartyIndex][buttonIndex] == characterIndex)
        {
            ResetButton(buttonIndex);
            return;
        }

        PreventDuplication(characterIndex);

        FieldPlayerState characterFS = changeSystem.FieldPlayers[characterIndex].GetComponentInChildren<FieldState>().StateSOJ;
        CharBtns[buttonIndex].gameObject.transform.GetChild(0).GetComponent<Image>().sprite = characterFS.GetCharacterIcon();
        CharBtns[buttonIndex].GetComponent<ChooseButton>().CharacterIndex = characterIndex;

        partyPlayerIndexes[currentPartyIndex][buttonIndex] = characterIndex;
    }

    public void PartySaveButtonClick()
    {
        if (!AllCharactersSelected())
        {
            var info = PopupManager.Instance.InstantPopUp(panelText);
            var infoPanel = info.GetComponentInChildren<AlramPanel>();

            infoPanel.SetAlramText(notSelectText);
        }
        else
        {
            List<int> filterList = new List<int>();

            for (int i = 0; i < partyPlayerIndexes[currentPartyIndex].Count; i++)
            {
                int characterindex = partyPlayerIndexes[currentPartyIndex][i];
                if (characterindex != -1)
                {
                    filterList.Add(characterindex);
                }
            }
            PlayerDataManager.Instance.SetPlayerBattler(filterList);
            ChangeSystem.PartyIndex = PlayerDataManager.Instance.GetPlayerBattler();

            ChangeSystem.Change(ChangeSystem.PartyIndex[0]);

            SetUI();

            IsPartyChange = true;

            controller.OriginPartyIndex = currentPartyIndex;
            
            controller.ExitButtonClick();
        }
    }

    public void PartyCancelButtonClick()
    {
        controller.ExitButtonClick();
    }

    private bool AllCharactersSelected()
    {
        for (int i = 0; i < CharBtns.Length; i++)
        {
            if (CharBtns[i].GetComponent<ChooseButton>().CharacterIndex != -1)
            {
                return true;
            }
        }

        return false;
    }

    private void SetUI()
    {
        UISystem.SetSOJ();

        for (int i = 0; i < UISystem.CharacterAreas.Length; i++)
        {
            UISystem.CharacterAreas[i].SetCharacterUI();
        }
    }

    private void LoadParty(int index)
    {
        for (int i = 0; i < CharBtns.Length; i++)
        {
            int characterIndex = partyPlayerIndexes[index][i];

            if (characterIndex == -1)
            {
                ResetButton(i);
            }
            else
            {
                FieldPlayerState characterFS = ChangeSystem.FieldPlayers[characterIndex].GetComponentInChildren<FieldState>().StateSOJ;
                CharBtns[i].gameObject.transform.GetChild(0).GetComponent<Image>().sprite = characterFS.GetCharacterIcon();
                CharBtns[i].GetComponent<ChooseButton>().CharacterIndex = characterIndex;
            }
        }
    }

    #region 캐릭터 중복 방지
    public void PreventDuplication(int index)
    {
        // 배열을 검사하여 캐릭터 인덱스를 이미 가지고 있는 버튼이 있는지 확인
        for (int i = 0; i < CharBtns.Length; i++)
        {
            ChooseButton btn = CharBtns[i].GetComponent<ChooseButton>();

            if (btn.CharacterIndex == index)
            {
                ResetButton(i);
            }
        }
    }

    public void ResetButton(int index)
    {
        CharBtns[index].GetComponent<ChooseButton>().CharacterIndex = -1;
        CharBtns[index].gameObject.transform.GetChild(0).GetComponent<Image>().sprite = originSprite;
        partyPlayerIndexes[currentPartyIndex][index] = -1;
    }
    #endregion
}

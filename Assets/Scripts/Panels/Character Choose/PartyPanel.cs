using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyPanel : MonoBehaviour
{
    [Header("��� ���� ��Ƽ ����")]
    [SerializeField] private Button[] collectsParty;

    [Header("������ ��Ƽ ����")]
    [SerializeField] private TextMeshProUGUI partyText;

    [Header("��Ƽ ĳ���� ��ư �迭")]
    [SerializeField] private Button[] charBtns;

    [Header("�÷��̾� ���� �ý���")]
    [SerializeField] private SetPlayerSystem changeSystem;

    [Header("�÷��̾� UI ���� �ý���")]
    [SerializeField] private SetPlayerUISystem UISystem;

    [Header("ĳ���� ���� ��Ʈ�ѷ� ������Ʈ")]
    [SerializeField] private CharacterChooseController controller;

    [SerializeField] private Sprite originSprite;

    private readonly string panelText = $"Player Info Panel";
    private readonly string notSelectText = $"ĳ���͸� �������� �ʾҽ��ϴ�.";

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
        partyText.text = $"��Ƽ 01";

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
        partyText.text = $"��Ƽ {index + 1:D2}";

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

    #region ĳ���� �ߺ� ����
    public void PreventDuplication(int index)
    {
        // �迭�� �˻��Ͽ� ĳ���� �ε����� �̹� ������ �ִ� ��ư�� �ִ��� Ȯ��
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

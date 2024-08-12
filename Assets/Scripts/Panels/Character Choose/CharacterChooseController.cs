using UnityEngine;

public class CharacterChooseController : MonoBehaviour
{
    [Header("��Ƽ ���� �г�")]
    [SerializeField] private PartyPanel partyPanel;

    [Header("ĳ���� ���� �г�")]
    [SerializeField] private CharacterChoose choosePanel;

    private bool isOn = false;
    private int originPartyIndex = 0;

    public int OriginPartyIndex { get => originPartyIndex; set => originPartyIndex = value; }

    private void Update()
    {
        TogglePartyKey();
    }

    private void TogglePartyKey()
    {
        if (Input.GetKeyDown(KeyCode.L) && !partyPanel.gameObject.activeSelf)
        {
            var panelActivate = PopupManager.Instance.GetPanelActivation();

            if (!isOn && panelActivate.IsAnyPanelActive()) return;

            isOn = !isOn;
            partyPanel.gameObject.SetActive(isOn);
            panelActivate.SetPanelActive(isOn);
        }
    }

    public void PartyChangeButtonClick(int partyIndex)
    {
        choosePanel.gameObject.SetActive(true);
        choosePanel.ButtonIndex = partyIndex;
    }

    public void ExitButtonClick()
    {
        var panelActivate = PopupManager.Instance.GetPanelActivation();

        isOn = false;
        partyPanel.gameObject.SetActive(isOn);
        panelActivate.SetPanelActive(isOn);
    }

    public PartyPanel GetOncePartyPanel() => partyPanel;
}
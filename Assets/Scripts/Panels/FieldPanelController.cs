using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPanelController : MonoBehaviour
{
    [Header("캐릭터 스탯 패널")]
    [SerializeField] private GameObject CharacterStatPanel;

    [Header("캐릭터 스탯 키")]
    [SerializeField] private KeyCode characterStatKey;

    private bool isOn = false;

    // Update is called once per frame
    private void Update()
    {
        ToggleKey();
    }

    private void ToggleKey()
    {
        if (Input.GetKeyDown(characterStatKey)) OpenStatPanel();
    }

    public void OpenStatPanel()
    {
        var panelActivate = PopupManager.Instance.GetPanelActivation();

        if (!isOn && panelActivate.IsAnyPanelActive()) return;

        isOn = !isOn;
        CharacterStatPanel.SetActive(isOn);
        panelActivate.SetPanelActive(isOn);
    }
}

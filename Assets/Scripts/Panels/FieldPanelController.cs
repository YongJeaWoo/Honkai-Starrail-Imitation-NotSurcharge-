using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPanelController : MonoBehaviour
{
    [Header("ĳ���� ���� �г�")]
    [SerializeField] private GameObject CharacterStatPanel;

    [Header("ĳ���� ���� Ű")]
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

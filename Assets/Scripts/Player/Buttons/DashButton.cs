using UnityEngine;

public class DashButton : PlayerUseButton
{
    [Header("�޸��� Ȱ��ȭ �̹���")]
    [SerializeField] private GameObject run_Ac;

    public override void UseEffectButton()
    {
        run_Ac.SetActive(isKey);
    }

    public bool IsRunCheck(bool isValue) => isKey = isValue;
}

using UnityEngine;

public class DashButton : PlayerUseButton
{
    [Header("달리기 활성화 이미지")]
    [SerializeField] private GameObject run_Ac;

    public override void UseEffectButton()
    {
        run_Ac.SetActive(isKey);
    }

    public bool IsRunCheck(bool isValue) => isKey = isValue;
}

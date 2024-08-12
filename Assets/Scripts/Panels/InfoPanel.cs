using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [Header("게임 패널 애니메이터 텍스트")]
    [SerializeField] protected string isOpen;

    protected bool isOn;

    protected PanelActivate panelActivation;
    protected Animator animator;

    protected virtual void Awake()
    {
        panelActivation = PopupManager.Instance.GetPanelActivation();
        animator = GetComponentInChildren<Animator>();
    }
}

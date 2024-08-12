using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [Header("���� �г� �ִϸ����� �ؽ�Ʈ")]
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

using UnityEngine;

public abstract class PlayerUseButton : MonoBehaviour
{
    [Header("��ư Ÿ��")]
    [SerializeField] private E_ButtonType buttonType;

    protected bool isKey;

    public abstract void UseEffectButton();
}

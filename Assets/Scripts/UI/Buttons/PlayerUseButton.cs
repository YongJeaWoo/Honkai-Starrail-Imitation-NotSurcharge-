using UnityEngine;

public abstract class PlayerUseButton : MonoBehaviour
{
    [Header("버튼 타입")]
    [SerializeField] private E_ButtonType buttonType;

    protected bool isKey;

    public abstract void UseEffectButton();
}

using UnityEngine;

public class FieldState : MonoBehaviour
{
    [Header("�ش� ĳ���� SOJ ������")]
    [SerializeField] private FieldPlayerState stateSOJ;

    [Header("���� ������")]
    public static int gauge;

    public FieldPlayerState StateSOJ { get => stateSOJ; }

    private void Start()
    {
        InitValues();
    }

    private void InitValues()
    {
        gauge = 3;
    }

    public FieldPlayerState GetStateInfo() => StateSOJ;
}

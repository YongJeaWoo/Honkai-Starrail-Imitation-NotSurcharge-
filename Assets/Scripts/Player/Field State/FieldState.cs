using UnityEngine;

public class FieldState : MonoBehaviour
{
    [Header("해당 캐릭터 SOJ 데이터")]
    [SerializeField] private FieldPlayerState stateSOJ;

    [Header("공용 게이지")]
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

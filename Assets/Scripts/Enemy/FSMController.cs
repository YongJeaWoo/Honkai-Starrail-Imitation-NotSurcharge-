using System;
using UnityEngine;

public abstract class FSMController<TEnum, TState, TController> : MonoBehaviour
    where TEnum : Enum
    where TState : GetState<TEnum, TState, TController>
    where TController : FSMController<TEnum, TState, TController>
{
    protected TState currentState;
    public TState CurrentState { get => currentState; set => currentState = value; }

    [Header("���� ����")]
    [SerializeField] protected TState[] myStates;
    public TState[] MyStates { get => myStates; set => myStates = value; }

    protected TEnum currentEnum;
    public TEnum CurrentEnum { get => currentEnum; set => currentEnum = value; }
    

    protected GameObject player;
    protected SetPlayerSystem playerSystem;

    [Header("�÷��̾� ���� ����")]
    [SerializeField] private float detectDistance;
    [Header("�÷��̾� ���� ����")]
    [SerializeField] private float attackDistance;
    [Header("���� ��� �̸�")]
    [SerializeField] protected string targetName;
    public string TargetName { get => targetName; set => targetName = value; }
    public float DetectDistance { get => detectDistance; set => detectDistance = value; }
    public float AttackDistance { get => attackDistance; set => attackDistance = value; }

    #region Enemy State Changing

    protected virtual void OnEnable()
    {
        playerSystem = FindObjectOfType<SetPlayerSystem>();

        player = playerSystem.GetPlayer();
    }

    protected virtual void Update()
    {
        CurrentState?.UpdateState();
    }

    public void TransitionToState(TEnum state)
    {
        CurrentState?.ExitState();
        CurrentState = MyStates[Convert.ToInt32(state)];
        CurrentState.EnterState(state);
    }
    #endregion

    #region GetValues
    public float GetPlayerDistance() 
        => Vector3.Distance(transform.position, player.transform.position);
    public GameObject GetPlayer() => player;
    public TState GetCurrentState() => CurrentState;
    public TState[] GetCurrentStates() => MyStates;
    public TEnum GetCurrentEnum() => currentEnum;

    #endregion
}

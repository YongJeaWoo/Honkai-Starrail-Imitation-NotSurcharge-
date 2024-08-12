using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleFSMController : BattleBehaviourComponent
{
    // ���� ����
    [SerializeField] private EnemyStateSOJ state;
    [SerializeField] protected AudioClip[] hitSound;
    [SerializeField] protected AudioClip deathSound;

    // �ڱ� �������� 
    private bool isMyTurn;

    private bool isHit;

    public bool IsMyTurn { get => isMyTurn; set => isMyTurn = value; }
    public bool IsHit { get => isHit; set => isHit = value; }

    public BattleSystem GetBattleSystem() => battleSystem;

    private void Start()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    // �ɷ�ġ ����
    public void InitValues()
    {
        MaxHp = state.Maxhp;
        CurrentHp = MaxHp;
        damage = state.Damage;
        ActionPoint = state.Acting;
        isAlive = true;
    }

    // ���� ��������
    public void EndTurn()
    {
        ActionPoint = state.Acting;
        IsMyTurn = false;
        EventActionToTurnOver();
    }

    public override void StartTurn()
    {
        
    }

    // �߻� �޼ҵ�� ����
    public abstract void Hit(float dmg);

    public abstract void SetRandomAttackTarget();

    public EnemyStateSOJ GetSOJData() => state;
}

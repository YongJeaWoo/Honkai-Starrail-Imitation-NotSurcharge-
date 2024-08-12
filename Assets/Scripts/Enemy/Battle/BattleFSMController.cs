using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleFSMController : BattleBehaviourComponent
{
    // 몬스터 스텟
    [SerializeField] private EnemyStateSOJ state;
    [SerializeField] protected AudioClip[] hitSound;
    [SerializeField] protected AudioClip deathSound;

    // 자기 차례인지 
    private bool isMyTurn;

    private bool isHit;

    public bool IsMyTurn { get => isMyTurn; set => isMyTurn = value; }
    public bool IsHit { get => isHit; set => isHit = value; }

    public BattleSystem GetBattleSystem() => battleSystem;

    private void Start()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    // 능력치 설정
    public void InitValues()
    {
        MaxHp = state.Maxhp;
        CurrentHp = MaxHp;
        damage = state.Damage;
        ActionPoint = state.Acting;
        isAlive = true;
    }

    // 턴이 끝났는지
    public void EndTurn()
    {
        ActionPoint = state.Acting;
        IsMyTurn = false;
        EventActionToTurnOver();
    }

    public override void StartTurn()
    {
        
    }

    // 추상 메소드로 정의
    public abstract void Hit(float dmg);

    public abstract void SetRandomAttackTarget();

    public EnemyStateSOJ GetSOJData() => state;
}

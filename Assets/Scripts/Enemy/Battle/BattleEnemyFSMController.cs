using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyFSMController : BattleFSMController
{
    // 몬스터의 현재 동작 중인 상태 컴포넌트
    [SerializeField] private BattleEnemyState currentState;

    // 몬스터의 모든 상태 컴포넌트들
    [SerializeField] private BattleEnemyState[] EnemyStatas;

    private Animator animator;

    // 상태 전환 메소드
    public void TransactionToState(E_BattleEnemyState state)
    {
        currentState?.ExitState(); // 이전 상태 정리
        currentState = EnemyStatas[(int)state]; // 상태 전환 처리
        currentState.EnterState(state); // 세로운 상태 전이
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        // 대기 상태로 시작
        TransactionToState(E_BattleEnemyState.Idle);
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    // 공격
    public override void StartTurn()
    {
        IsMyTurn = true;

        TransactionToState(E_BattleEnemyState.Attack);
    }

    // 플레이어에게 공격을 받음
    public override void Hit(float dmg)
    {
        // 현재 상태가 이미 사망한 상태면 피격 처리하지 않음
        if (currentState == EnemyStatas[(int)E_BattleEnemyState.Die]) return;

        health.HpDown(dmg);

        // 피격 상태로 전환
        TransactionToState(E_BattleEnemyState.Hit);

        // 랜덤한 히트 사운드를 재생합니다.
        if (hitSound.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSound.Length);
            AudioManager.instance.EffectPlay(hitSound[randomIndex]);
        }

        if (health.GetCurrentHp() <= 0)
        {
            TransactionToState(E_BattleEnemyState.Die);

            AudioManager.instance.EffectPlay(deathSound);
        }
    }

    // 랜덤 적 선택
    public override void SetRandomAttackTarget()
    {
        var enemyBattleSystem = battleSystem.GetEnemySystem();
        var targetLists = enemyBattleSystem.GetSelectTargetList();

        var aliveTargets = new List<BattleBehaviourComponent>();

        if (targetLists.Count > 0)
        {
            foreach (var target in targetLists)
            {
                var component = target.GetComponent<BattleBehaviourComponent>();

                if (component.IsAlive)
                {
                    aliveTargets.Add(target);
                }
            }

            if (aliveTargets.Count > 0)
            {
                int randomIndex = Random.Range(0, aliveTargets.Count);
                attackTarget = aliveTargets[randomIndex];
            }
            else
            {
                attackTarget = null;
            }
        }
    }
}

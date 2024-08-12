using UnityEngine;

public class BattleEnemyAttackState : BattleEnemyState
{
    // 기본 공격
    protected void BasicAttack(BattleBehaviourComponent target)
    {
        Debug.Log(target + "을(를) 때렷습니다");

        // 선택된 타겟이 BattleCharacterState 타입인지 확인 후 Hit 메서드 호출
        if (target is BattleCharacterState)
        {
            (target as BattleCharacterState).Hit(controller.Damage);
        }
    }

    public override void EnterState(E_BattleEnemyState state)
    {
        Debug.Log(name + "의 턴입니다.");

        animator.SetInteger("state", (int)state);

        controller.SetRandomAttackTarget();

        BasicAttack(controller.GetAttackTarget());

        Debug.Log(name + "의 턴이 끝났습니다");
    }

    public override void UpdateState()
    {
        // 죽엇는지
        if (controller.health.GetCurrentHp() <= 0)
        {
            // 죽음 상태로 전환
            controller.TransactionToState(E_BattleEnemyState.Die);
            return;
        }
    }

    public override void ExitState()
    {

    }
}

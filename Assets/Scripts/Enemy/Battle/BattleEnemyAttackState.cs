using UnityEngine;

public class BattleEnemyAttackState : BattleEnemyState
{
    // �⺻ ����
    protected void BasicAttack(BattleBehaviourComponent target)
    {
        // ���õ� Ÿ���� BattleCharacterState Ÿ������ Ȯ�� �� Hit �޼��� ȣ��
        if (target is BattleCharacterState)
        {
            (target as BattleCharacterState).Hit(controller.Damage);
        }
    }

    public override void EnterState(E_BattleEnemyState state)
    {
        animator.SetInteger("state", (int)state);

        controller.SetRandomAttackTarget();

        BasicAttack(controller.GetAttackTarget());
    }

    public override void UpdateState()
    {
        // �׾�����
        if (controller.health.GetCurrentHp() <= 0)
        {
            // ���� ���·� ��ȯ
            controller.TransactionToState(E_BattleEnemyState.Die);
            return;
        }
    }

    public override void ExitState()
    {

    }
}

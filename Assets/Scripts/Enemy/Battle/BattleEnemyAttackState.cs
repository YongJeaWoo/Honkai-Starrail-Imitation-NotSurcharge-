using UnityEngine;

public class BattleEnemyAttackState : BattleEnemyState
{
    // �⺻ ����
    protected void BasicAttack(BattleBehaviourComponent target)
    {
        Debug.Log(target + "��(��) ���ǽ��ϴ�");

        // ���õ� Ÿ���� BattleCharacterState Ÿ������ Ȯ�� �� Hit �޼��� ȣ��
        if (target is BattleCharacterState)
        {
            (target as BattleCharacterState).Hit(controller.Damage);
        }
    }

    public override void EnterState(E_BattleEnemyState state)
    {
        Debug.Log(name + "�� ���Դϴ�.");

        animator.SetInteger("state", (int)state);

        controller.SetRandomAttackTarget();

        BasicAttack(controller.GetAttackTarget());

        Debug.Log(name + "�� ���� �������ϴ�");
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

public class BattleEnemyIdleState : BattleEnemyState
{
    // ��� ���� ����(����) ó�� (���� �ʱ�ȭ)
    public override void EnterState(E_BattleEnemyState state)
    {
        animator.SetInteger("state", (int)state);
    }

    // ��� ���� ��� ���� ó�� (���� ����)
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

    // ��� ���� ����(�ٸ����·� ����) ���� ó��(���� ����)
    public override void ExitState()
    {

    }
}

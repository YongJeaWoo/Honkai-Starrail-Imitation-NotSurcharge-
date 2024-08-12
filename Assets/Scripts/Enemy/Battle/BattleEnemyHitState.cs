using UnityEngine;

public class BattleEnemyHitState : BattleEnemyState
{
    // �ǰ� ��ƼŬ
    [SerializeField] protected ParticleSystem hitParticle;

    public override void EnterState(E_BattleEnemyState state)
    {
        // ��Ʈ ��� ����
        animator.SetInteger("state", (int)state);

        animator.SetTrigger("Hit");

        hitParticle.Play();
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

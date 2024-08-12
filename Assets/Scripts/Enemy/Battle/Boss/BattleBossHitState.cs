using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBossHitState : BattleBossState
{
    // �ǰ� ��ƼŬ
    [SerializeField] protected ParticleSystem hitParticle;

    public override void EnterState(E_BattleBossState state)
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
            controller.TransactionToState(E_BattleBossState.Die);
            return;
        }
    }

    public override void ExitState()
    {

    }
}

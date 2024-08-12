using UnityEngine;

public class BattleEnemyHitState : BattleEnemyState
{
    // 피격 파티클
    [SerializeField] protected ParticleSystem hitParticle;

    public override void EnterState(E_BattleEnemyState state)
    {
        // 히트 모션 실행
        animator.SetInteger("state", (int)state);

        animator.SetTrigger("Hit");

        hitParticle.Play();
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

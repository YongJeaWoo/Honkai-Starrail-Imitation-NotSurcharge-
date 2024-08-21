using UnityEngine;

public class BossAttackState : BossState
{
    [Header("공격 대상 레이어")]
    [SerializeField] private LayerMask targetLayer;
    [Header("공격 타겟 위치")]
    [SerializeField] private Transform attackTransform;
    [Header("공격 범위")]
    [SerializeField] private float attackRadius;
    [Header("공격 범위 각도")]
    [SerializeField] private float hitAngle;
    [Header("공격 탄 프리팹")]
    [SerializeField] private GameObject bulletPrefab;

    public override void EnterState(E_BossState state)
    {
        animator.SetInteger("state", (int)state);
        agent.isStopped = true;
    }

    public override void UpdateState()
    {
        if (fsm.GetPlayer().GetComponentInChildren<CloakingSkill>() != null)
        {
            if (fsm.GetPlayer().GetComponentInChildren<CloakingSkill>().GetIsCloaking())
            {
                fsm.TransitionToState(E_BossState.Giveup);
                return;
            }
        }

        transform.LookAt(fsm.GetPlayer().transform);

        if (fsm.GetPlayerDistance() <= fsm.DetectDistance && fsm.GetPlayerDistance() > fsm.AttackDistance)
        {
            if (fsm.GetPlayer().GetComponentInChildren<CloakingSkill>() != null)
            {
                if (fsm.GetPlayer().GetComponentInChildren<CloakingSkill>().GetIsCloaking())
                {
                    fsm.TransitionToState(E_BossState.Giveup);
                    return;
                }
            }

            fsm.TransitionToState(E_BossState.Detect);
            return;
        }
    }
    public override void ExitState()
    {
        agent.isStopped = false;
    }

    public void MonsterAttackEvent()
    {
        Instantiate(bulletPrefab, attackTransform.position, attackTransform.rotation, fsm.MyZoneCheck().transform.Find("Bullet Collect"));
    }
}

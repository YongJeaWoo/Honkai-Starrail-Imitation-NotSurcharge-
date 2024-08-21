using UnityEngine;

public class BossDetectState : BossState
{
    [Header("추적 속도")]
    [SerializeField] private float detectSpeed;

    public override void EnterState(E_BossState state)
    {
        agent.speed = detectSpeed;
        animator.SetInteger("state", (int)state);
    }

    public override void UpdateState()
    {
        agent.isStopped = false;
        agent.SetDestination(fsm.GetPlayer().transform.position);

        if (fsm.GetPlayerDistance() <= fsm.AttackDistance)
        {
            if (fsm.GetPlayer().GetComponentInChildren<CloakingSkill>() != null)
            {
                if (fsm.GetPlayer().GetComponentInChildren<CloakingSkill>().GetIsCloaking())
                {
                    fsm.TransitionToState(E_BossState.Giveup);
                    return;
                }
            }

            fsm.TransitionToState(E_BossState.Attack);
            return;
        }

        if (fsm.GetPlayerDistance() >= fsm.DetectDistance)
        {
            if (fsm.GetPlayer().GetComponentInChildren<CloakingSkill>() != null)
            {
                if (fsm.GetPlayer().GetComponentInChildren<CloakingSkill>().GetIsCloaking())
                {
                    fsm.TransitionToState(E_BossState.Giveup);
                    return;
                }
            }

            fsm.TransitionToState(E_BossState.Giveup);
            return;
        }
    }
    public override void ExitState()
    {

    }
}

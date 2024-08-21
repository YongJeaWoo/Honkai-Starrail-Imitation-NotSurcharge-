using UnityEngine;

public class DetectState : EnemyState
{
    [Header("추적 속도")]
    [SerializeField] private float detectSpeed;

    public override void EnterState(E_EnemyState state)
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
                    fsm.TransitionToState(E_EnemyState.Giveup);
                    return;
                }
            }

            fsm.TransitionToState(E_EnemyState.Attack);
            return;
        }

        if(fsm.GetPlayerDistance() >= fsm.DetectDistance)
        {
            if (fsm.GetPlayer().GetComponentInChildren<CloakingSkill>() != null)
            {
                if (fsm.GetPlayer().GetComponentInChildren<CloakingSkill>().GetIsCloaking())
                {
                    fsm.TransitionToState(E_EnemyState.Giveup);
                    return;
                }
            }

            fsm.TransitionToState(E_EnemyState.Giveup);
            return;
        }
    }
    public override void ExitState()
    {
        
    }
}

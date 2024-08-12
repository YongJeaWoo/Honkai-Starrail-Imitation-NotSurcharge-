using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : EnemyState
{
    public override void EnterState(E_EnemyState state)
    {
        agent.isStopped = true;
        animator.SetInteger("state", (int)state);
    }

    public override void UpdateState()
    {
        if (fsm.GetPlayer().GetComponentInChildren<CloakingSkill>() != null)
        {
            if (fsm.GetPlayer().GetComponentInChildren<CloakingSkill>().GetIsCloaking()) return;        
        }

        if (fsm.GetPlayerDistance() <= fsm.DetectDistance)
        {
            fsm.TransitionToState(E_EnemyState.Detect);
            return;
        }
    }
    public override void ExitState() { }
}

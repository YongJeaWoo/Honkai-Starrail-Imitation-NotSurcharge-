using System;
using UnityEngine;

public class HitState : EnemyState
{
    public override void EnterState(E_EnemyState state)
    {
        agent.isStopped = true;
        animator.SetInteger("state", (int)state);
        animator.speed = 0.1f;
    }

    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        agent.isStopped = false;
        animator.speed = 1f;
    }
}

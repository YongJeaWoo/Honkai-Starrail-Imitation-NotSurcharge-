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
        var player = fsm.GetPlayer();

        // �÷��̾� ������Ʈ�� null���� Ȯ��
        if (player == null) return;

        var cloakingSkill = player.GetComponentInChildren<CloakingSkill>();

        // CloakingSkill ������Ʈ�� �ִ��� Ȯ��
        if (cloakingSkill != null && cloakingSkill.GetIsCloaking()) return;

        if (fsm.GetPlayerDistance() <= fsm.DetectDistance)
        {
            fsm.TransitionToState(E_EnemyState.Detect);
            return;
        }
    }
    public override void ExitState() { }
}

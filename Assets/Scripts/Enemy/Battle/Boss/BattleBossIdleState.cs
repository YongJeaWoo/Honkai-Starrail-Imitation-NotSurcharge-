using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBossIdleState : BattleBossState
{
    // 대기 상태 시작(진입) 처리 (상태 초기화)
    public override void EnterState(E_BattleBossState state)
    {
        animator.SetInteger("state", (int)state);
    }

    // 대기 상태 기능 동작 처리 (상태 실행)
    public override void UpdateState()
    {
        // 죽엇는지
        if (controller.health.GetCurrentHp() <= 0)
        {
            // 죽음 상태로 전환
            controller.TransactionToState(E_BattleBossState.Die);
            return;
        }
    }

    // 대기 상태 종료(다른상태로 전이) 동작 처리(상태 정리)
    public override void ExitState()
    {

    }
}

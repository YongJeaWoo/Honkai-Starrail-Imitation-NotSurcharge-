using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBossDeathState : BattleBossState
{
    public override void EnterState(E_BattleBossState state)
    {
        Debug.Log("몬스터가 죽었습니다.");

        animator.SetInteger("state", (int)state);

        controller.IsAlive = false;
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
}

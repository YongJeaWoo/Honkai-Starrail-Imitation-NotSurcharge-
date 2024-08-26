using UnityEngine;

public class BattleEnemyDeathState : BattleEnemyState
{
    public override void EnterState(E_BattleEnemyState state)
    {
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

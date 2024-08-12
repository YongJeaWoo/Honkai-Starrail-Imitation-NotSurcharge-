using UnityEngine;

public class BattleEnemyDeathState : BattleEnemyState
{
    public override void EnterState(E_BattleEnemyState state)
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

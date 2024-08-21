public class BossIdleState : BossState
{
    public override void EnterState(E_BossState state)
    {
        agent.isStopped = true;
        animator.SetInteger("state", (int)state);
    }

    public override void UpdateState()
    {
        var player = fsm.GetPlayer();

        if (player == null) return;

        var cloakingSkill = player.GetComponentInChildren<CloakingSkill>();

        if (cloakingSkill != null && cloakingSkill.GetIsCloaking()) return;

        if (fsm.GetPlayerDistance() <= fsm.DetectDistance)
        {
            fsm.TransitionToState(E_BossState.Detect);
            return;
        }
    }
    public override void ExitState() { }
}

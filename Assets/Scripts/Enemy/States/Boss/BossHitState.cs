public class BossHitState : BossState
{
    public override void EnterState(E_BossState state)
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

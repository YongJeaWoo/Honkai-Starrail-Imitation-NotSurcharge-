using UnityEngine;

public class BossGiveupState : BossState
{
    [Header("복귀 속도")]
    [SerializeField] private float giveupSpeed;
    [Header("원래 위치")]
    private Vector3 originPos;
    [Header("원래 위치와의 거리")]
    private float originDis;

    private void Start()
    {
        originPos = transform.position;
        originDis = 0f;
    }

    public override void EnterState(E_BossState state)
    {
        agent.speed = giveupSpeed;
        animator.SetInteger("state", (int)state);
    }

    public override void UpdateState()
    {
        if (fsm.GetPlayerDistance() <= fsm.DetectDistance)
        {
            fsm.TransitionToState(E_BossState.Detect);
            return;
        }

        GoToOriginPosition();

        if (originPos != null)
        {
            originDis = Vector3.Distance(transform.position, originPos);

            if (originDis <= 1f)
            {
                fsm.TransitionToState(E_BossState.Idle);
            }
        }
    }
    public override void ExitState()
    {
        agent.isStopped = true;
    }

    private void GoToOriginPosition()
    {
        agent.SetDestination(originPos);
    }
}

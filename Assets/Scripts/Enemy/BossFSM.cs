using UnityEngine;

public enum E_BossState
{
    Idle,
    Giveup,
    Detect,
    Attack,
    Hit
}

public class BossFSM : FSMController<E_BossState, BossState, BossFSM>, IHit
{
    private EnemyZone myZone;
    protected void Start()
    {
        MyZoneCheck();
        TransitionToState(E_BossState.Idle);
    }

    public EnemyZone MyZoneCheck()
    {
        return myZone = transform.parent.parent.GetComponent<EnemyZone>();
    }

    public void Hit()
    {
        TransitionToState(E_BossState.Hit);
        BattleEntryManager.Instance.SetZone(myZone);
        BattleEntryManager.Instance.BrokenEntryBattle(E_TurnBase.Player);
    }
}

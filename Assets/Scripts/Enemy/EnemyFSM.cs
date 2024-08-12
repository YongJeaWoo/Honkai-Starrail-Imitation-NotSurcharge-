public enum E_EnemyState
{
    Idle,
    Giveup,
    Detect,
    Attack,
    Hit
}

public class EnemyFSM : FSMController<E_EnemyState, EnemyState, EnemyFSM>, IHit
{
    private EnemyZone myZone;

    protected void Start()
    {
        MyZoneCheck();
        TransitionToState(E_EnemyState.Idle);
    }

    public EnemyZone MyZoneCheck()
    {
        return myZone = transform.parent.parent.GetComponent<EnemyZone>();
    }

    public void Hit()
    {
        TransitionToState(E_EnemyState.Hit);
        BattleEntryManager.Instance.SetZone(myZone);
        BattleEntryManager.Instance.FadeoutEntryBattle(E_TurnBase.Player);
    }
    
    public EnemyZone GetMyZone() => myZone;
}

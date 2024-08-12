using UnityEngine;

public abstract class TurnBattlerObject : MonoBehaviour
{
    protected abstract void StartTurn();
    protected abstract void TurnOver();

    protected TurnBattlerObject attackTarget;

    protected bool isAlive;
    protected bool isTurnOver;

    protected float actionPoints;

    protected bool GetTurnOver() => isTurnOver;
    protected float DecreaseActionPoint() => actionPoints = 0f;
    public TurnBattlerObject GetAttackTarget() => attackTarget;
}

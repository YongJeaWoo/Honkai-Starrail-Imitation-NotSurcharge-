using UnityEngine;

public class BasicAttackButton : MonoBehaviour
{
    public delegate void BasicCallAttack();

    public static BasicCallAttack basicCallAttack;

    private void Start()
    {
        CallMethod();
    }

    public void CallMethod()
    {
        var battleSystem = FindObjectOfType<BattleSystem>();
        var playerSystem = battleSystem.GetPlayerSystem();
        playerSystem.onBasicAttackButton += BaseAttackMethod;
    }

    public void BaseAttackMethod()
    {
        basicCallAttack?.Invoke();
    }
}

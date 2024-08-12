using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateAttackButton : MonoBehaviour
{
    public delegate void UltimateCallAttack();

    public static UltimateCallAttack ultimateCallAttack;

    public void Start()
    {
        CallMethod();
    }

    public void CallMethod()
    {
        var battleSystem = FindObjectOfType<BattleSystem>();
        var playerSystem = battleSystem.GetPlayerSystem();
        playerSystem.onUltimateAttackButton += UltimateAttackMethod;
    }

    public void UltimateAttackMethod()
    {
        ultimateCallAttack.Invoke();
    }
}

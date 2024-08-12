using System.Collections.Generic;
using UnityEngine;

public class CycleUIController : MonoBehaviour
{
    public void SetBattler(BattleSystem battleSystem, List<BattleBehaviourComponent> getBattler)
    {
        float tempAcp;

        foreach (var battler in getBattler)
        {
            var component = battler.GetComponent<BattleBehaviourComponent>();
            tempAcp = component.ActionPoint;
        }

        getBattler.Sort((x, y) => x.ActionPoint.CompareTo(y.ActionPoint));
        Debug.Log(getBattler);
    }
}

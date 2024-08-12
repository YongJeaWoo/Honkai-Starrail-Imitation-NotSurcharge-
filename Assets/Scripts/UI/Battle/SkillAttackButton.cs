using UnityEngine;
using UnityEngine.UI;

public class SkillAttackButton : MonoBehaviour
{
    public delegate void SkillCallAttack();

    public static SkillCallAttack skillCallAttack;

    private PlayerBattleSystem playerBattleSystem;

    private void Start()
    {
        CallMethod();
        playerBattleSystem = FindAnyObjectByType<PlayerBattleSystem>();
    }

    public void CallMethod()
    {
        var battleSystem = FindObjectOfType<BattleSystem>();
        var playerSystem = battleSystem.GetPlayerSystem();
        playerSystem.onSkillAttackButton += SkillAttackMethod;
    }

    public void SkillAttackMethod()
    {
        if (playerBattleSystem.GetActionPoint() <= 0)
        {
            Debug.Log("스킬을 사용할 수 없습니다.");
            return;
        }
        skillCallAttack?.Invoke();
    }
}

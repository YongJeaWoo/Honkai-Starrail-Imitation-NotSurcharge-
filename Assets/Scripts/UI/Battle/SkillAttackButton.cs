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
            Debug.Log("��ų�� ����� �� �����ϴ�.");
            return;
        }
        skillCallAttack?.Invoke();
    }
}

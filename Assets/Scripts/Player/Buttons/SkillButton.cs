using UnityEngine;
using UnityEngine.UI;

public class SkillButton : PlayerUseButton
{
    [Header("스킬 게이지 이미지")]
    [SerializeField] private Image[] skill_Gages;
    [Header("스킬 게이지 색상")]
    [SerializeField] private Color32 gageColor;

    private Color32 originColor = new Color32(255, 255, 255, 255);

    public override void UseEffectButton()
    {
        if (FieldState.gauge < 0) return;

        skill_Gages[FieldState.gauge--].color = originColor;
    }

    public void HealItemUse()
    {
        if (FieldState.gauge >= 3) return;

        skill_Gages[++FieldState.gauge].color = gageColor;
    }
}

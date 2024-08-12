using UnityEngine;

public class AnimationEventComponent : MonoBehaviour
{
    private FastRunEffect effect;
    private CharacterAttack attack;

    private void Awake()
    {
        effect = GetComponentInChildren<FastRunEffect>();
        attack = GetComponentInChildren<CharacterAttack>();
    }

    public void ShowTrailAniEvent()
    {
        effect.ShowTrailRenderEvent();
    }

    public void HideTrailAniEvent()
    {
        effect.HideTrailRenderEvent();
    }

    public void ShowAttackTrailAniEvent()
    {
        attack.ShowAttackTrailEvent();
    }

    public void HideAttackTrailAniEvent()
    {
        attack.HideAttackTrailEvent();
    }

    public void AttackHitAniEvent()
    {
        attack.AttackHitAnimationEvent();
    }
}

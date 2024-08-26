using UnityEngine;

public abstract class FieldSkill : MonoBehaviour
{
    protected readonly string skillAnimationText = "Skill";
    [SerializeField] protected AudioClip skillSound;

    public abstract void UseFieldSkill();

    protected void PlaySoundEvent()
    {
        AudioManager.instance.EffectPlay(skillSound);
    }
}

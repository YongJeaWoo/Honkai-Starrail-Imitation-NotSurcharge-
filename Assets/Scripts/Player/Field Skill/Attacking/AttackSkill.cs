public class AttackSkill : FieldSkill
{
    private CharacterAttack attack;
    
    private void Awake()
    {
        attack = GetComponent<CharacterAttack>();
    }

    public override void UseFieldSkill()
    {
        var animator = attack.GetAnimator();
        animator.SetTrigger(skillAnimationText);
    }
}

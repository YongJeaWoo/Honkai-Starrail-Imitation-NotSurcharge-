using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingSkill : FieldSkill
{
    [Header("생성할 이펙트")]
    [SerializeField] private GameObject healParticle;

    public override void UseFieldSkill()
    {
        BaseHealth health = transform.parent.GetComponent<BaseHealth>();
        Animator animator = GetComponentInParent<Animator>();

        animator.SetTrigger(skillAnimationText);
        Instantiate(healParticle, transform.position, Quaternion.identity);


        float currentHp = health.GetCurrentHp();
        float maxHp = health.GetMaxHp();
        float plusHp = currentHp * 0.2f;

        if (currentHp + plusHp <= maxHp)
        {
            health.HpUp(plusHp);
        }
        else
        {
            health.SetCurrentHp(maxHp);
        }
    }
}

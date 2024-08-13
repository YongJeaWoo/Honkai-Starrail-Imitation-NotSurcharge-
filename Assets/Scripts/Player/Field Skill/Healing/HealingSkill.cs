using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingSkill : FieldSkill
{
    [Header("������ ����Ʈ")]
    [SerializeField] private GameObject healParticle;

    public override void UseFieldSkill()
    {
        //Instantiate(healParticle, transform.position, Quaternion.identity);
        BaseHealth health = transform.parent.GetComponent<BaseHealth>();
        
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

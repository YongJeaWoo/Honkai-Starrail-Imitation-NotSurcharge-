using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapphiSkill : FieldSkill
{
    [Header("»ý¼ºÇÒ ÀÌÆåÆ®")]
    [SerializeField] private GameObject attackParticle;

    [Header("ÆÄ±« ÀÌÆåÆ®")]
    [SerializeField] private GameObject destroyParticle;

    [Header("¸ó½ºÅÍ¸¦ Á×ÀÏ ¹Ý°æ")]
    [SerializeField] private float radius;

    public override void UseFieldSkill()
    {
        //Instantiate(attackParticle, transform.position, Quaternion.identity);

        Collider[] Enemy = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider obj in Enemy)
        {
            if (obj.CompareTag("Enemy"))
            {
                NormalEnemyZone enemyZone = obj.transform.parent.parent.GetComponent<NormalEnemyZone>();

                enemyZone.CharacterSkillCall();
                Destroy(obj.gameObject);
                //GameObject destroy = Instantiate(destroyParticle, obj.transform.position, Quaternion.identity);
                //destroy.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}

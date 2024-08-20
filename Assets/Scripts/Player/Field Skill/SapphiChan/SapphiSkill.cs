using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapphiSkill : FieldSkill
{
    [Header("ÆÄ±« ÀÌÆåÆ®")]
    [SerializeField] private GameObject destroyParticle;

    [Header("¸ó½ºÅÍ¸¦ Á×ÀÏ ¹Ý°æ")]
    [SerializeField] private float radius;

    private NormalEnemyZone enemyZone;

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

    public void FieldSkillEvent()
    {
        //Instantiate(attackParticle, transform.position, Quaternion.identity);

        Collider[] Enemy = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider obj in Enemy)
        {
            if (obj.CompareTag("Enemy"))
            {
                enemyZone = obj.transform.parent.parent.GetComponent<NormalEnemyZone>();

                var enemyList = enemyZone.GetFieldEnemies();
                foreach (var enemy in enemyList)
                {
                    var fsm = enemy.GetComponent<EnemyFSM>();
                    var anim = enemy.GetComponent<Animator>();

                    fsm.TransitionToState(E_EnemyState.Hit);
                    anim.speed = 0.3f;
                    StartCoroutine(DestroyCoroutine(obj.gameObject));
                }
            }
        }
    }

    private IEnumerator DestroyCoroutine(GameObject enemy)
    {
        yield return new WaitForSeconds(1.3f);
        //GameObject destroy = Instantiate(destroyParticle, obj.transform.position, Quaternion.identity);
        //destroy.GetComponent<ParticleSystem>().Play();

        enemyZone.CallRespawn();
        enemyZone.CharacterSkillCall();
        Destroy(enemy.gameObject);

        yield return null;
    }
}

using UnityEngine;

public class MeleeAttack : CharacterAttack
{
    [Header("공격 대상 레이어")]
    [SerializeField] private LayerMask targetLayer;
    [Header("공격 범위")]
    [SerializeField] private float attackRadius;
    [Header("공격 범위 각도")]
    [SerializeField] private float hitAngle;

    private void Awake()
    {
        GetComponents();
    }

    private void Update()
    {
        Attack();
    }

    public override void AttackHitAnimationEvent()
    {
        AudioManager.instance.EffectPlay(attackSound);
        Collider[] hits = Physics.OverlapSphere(attackTransform.position, attackRadius, targetLayer);
        Collider closestHit = null;
        float closestDistasnce = float.MaxValue;

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = hit.transform.position - transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget < closestDistasnce)
            {
                closestDistasnce = distanceToTarget;
                closestHit = hit;
            }
        }

        if (closestHit != null)
        {
            Vector3 directionToTarget = closestHit.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget < hitAngle)
            {
                PlayerDataManager.Instance.SetPlayerTransform(gameObject.transform.parent.position, gameObject.transform.parent.rotation);
                closestHit.GetComponent<IHit>().Hit();
            }
        }

        HideAttackTrailEvent();
    }

    public override void ShowAttackTrailEvent()
    {
        attackTrail.enabled = true;
    }

    public override void HideAttackTrailEvent()
    {
        attackTrail.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, attackRadius);
    }
}

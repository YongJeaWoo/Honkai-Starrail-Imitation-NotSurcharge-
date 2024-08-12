using UnityEngine;

public class RangedAttack : CharacterAttack
{
    [Header("���� ������")]
    [SerializeField] private GameObject attackPrefab;
    [Header("ȿ�� ������")]
    [SerializeField] private GameObject attackSystem;

    private void Awake()
    {
        GetComponents();
    }

    private void Update()
    {
        Attack();
    }

    public override void ShowAttackTrailEvent()
    {
        
    }

    public override void HideAttackTrailEvent()
    {
        
    }

    public override void AttackHitAnimationEvent()
    {
        Instantiate(attackPrefab, attackTransform.position, attackTransform.rotation);

        AudioManager.instance.EffectPlay(attackSound);
        AudioManager.instance.EffectPlay(attackSFXSound);
        //Instantiate(attackSystem, go.transform.position, attackTransform.rotation);
    }
}

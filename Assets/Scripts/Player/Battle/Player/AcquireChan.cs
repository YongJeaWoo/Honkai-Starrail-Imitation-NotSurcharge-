using UnityEngine;

public class AcquireChan : BattleCharacterState, IActionButton
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballPos;
    [SerializeField] private GameObject ultimateBallPrefab;
    [SerializeField] private Transform airBallPos;
    
    
    public override void StartTurn()
    {
        base.StartTurn();

        if (isUltimateReady)
        {
            uiSystem.UseUltimate(PlayerUltimateSprite);
        }

        BasicAttackButton.basicCallAttack += BasicAttack;
        SkillAttackButton.skillCallAttack += SkillAttack;
        UltimateAttackButton.ultimateCallAttack += UltimateAttack;
    }

    // �⺻ ����
    public override void BasicAttack()
    {
        base.BasicAttack();

        selectedTarget = SelectTarget();

        if (selectedTarget == null)
        {
            return; // Ÿ���� ���õ��� �ʾ����� ��ȯ
        }

        FaceTarget(selectedTarget.transform.position);

        Animator.SetTrigger("Battle Attack");

        ChargeUltimateGauge(0.2f);

        battleSystem.GetPlayerSystem().PlayerTurnEnd();
    }

    // ��ų ����
    public override void SkillAttack()
    {
        base.SkillAttack();

        selectedTarget = SelectTarget();
        if (selectedTarget == null)
        {
            return; // Ÿ���� ���õ��� �ʾ����� ��ȯ
        }

        FaceTarget(selectedTarget.transform.position);

        ChargeUltimateGauge(0.3f);

        Animator.SetTrigger("Battle Skill");

        battleSystem.GetPlayerSystem().PlayerTurnEnd();
    }

    // �ʻ�� ����
    public override void UltimateAttack()
    {
        // �ʻ�� ����
        if (isUltimateReady == true)
        {
            base.UltimateAttack();

            allSelectedTarget = SelectAllTarget();

            if (allSelectedTarget == null)
            {
                return; // Ÿ���� ���õ��� �ʾ����� ��ȯ
            }

            Animator.SetTrigger("Battle Ultimate");

            SetUltimateReady(false);

            // �ñر� ��� �� UI ������Ʈ
            uiSystem.ReturnUltimateButton(playerIndex);
        }
    }

    // �� ���� 
    public override void EndTurn()
    {
        base.EndTurn();

        BasicAttackButton.basicCallAttack -= BasicAttack;
        SkillAttackButton.skillCallAttack -= SkillAttack;
        UltimateAttackButton.ultimateCallAttack -= UltimateAttack;
    }

    // ���� ���� �ִϸ��̼� �̺�Ʈ
    public override void ApplyDamage()
    {
        if (selectedTarget == null)
        {
            return; // Ÿ���� ���õ��� �ʾ����� ��ȯ
        }

        // ���õ� Ÿ���� BattleEnemyFSMController Ÿ������ Ȯ�� �� Hit �޼��� ȣ��
        if (selectedTarget is BattleFSMController)
        {
            (selectedTarget as BattleFSMController).Hit(Damage);
        }
    }

    public void BallEvent()
    {
        GameObject ball = Instantiate(ballPrefab, ballPos.position, Quaternion.identity);
        ball.GetComponent<Ball>().SetTarget(selectedTarget);
        AudioManager.instance.EffectPlay(attackSFXSound);
    }

    public override void AllApplyDamage()
    {
        if (allSelectedTarget == null || allSelectedTarget.Count == 0)
        {
            return;
        }

        foreach (var target in allSelectedTarget)
        {
            // ���õ� Ÿ���� BattleEnemyFSMController Ÿ������ Ȯ�� �� Hit �޼��� ȣ��
            if (target is BattleFSMController enemy)
            {
                enemy.Hit(Damage);
            }
            else
            {
                Debug.LogWarning($"{target.name}��(��) ��ȿ�� Ÿ�� Ÿ���� �ƴմϴ�.");
            }

            GameObject ball = Instantiate(ultimateBallPrefab, airBallPos.position, Quaternion.identity);
            ball.GetComponent<Ball>().SetTarget(target);
            AudioManager.instance.EffectPlay(ultimateSFXSound);
        }
    }
}

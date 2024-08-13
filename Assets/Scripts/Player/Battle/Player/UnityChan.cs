using System.Collections;
using UnityEngine;

public class UnityChan : BattleCharacterState, IActionButton
{
    // �ñر� ����Ʈ ��ƼŬ
    [SerializeField] private GameObject BurstingEffectPrefabs;

    [SerializeField] private Transform UltimatePos;

    public override void StartTurn()
    {
        base.StartTurn();

        Debug.Log(name + "�� ���Դϴ�.");

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
            Debug.Log("Ÿ���� ���õ��� �ʾҽ��ϴ�.");
            return; // Ÿ���� ���õ��� �ʾ����� ��ȯ
        }

        // ����1 ����
        Debug.Log("�⺻ ���� �ߵ�");

        // ĳ���͸� ���� ������ �̵�
        Vector3 targetPosition = selectedTarget.transform.position;
        targetPosition.y -= 0.4f;

        StartCoroutine(MoveToTarget(targetPosition, "Battle Attack", 1f));

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
            Debug.Log("Ÿ���� ���õ��� �ʾҽ��ϴ�.");
            return; // Ÿ���� ���õ��� �ʾ����� ��ȯ
        }

        // ����2 ����
        Debug.Log("(����Ʈ)��ų �ߵ�");

        // ĳ���͸� ���� ������ �̵�
        Vector3 targetPosition = selectedTarget.transform.position;
        targetPosition.y -= 0.4f;
        StartCoroutine(MoveToTarget(targetPosition, "Battle Skill", 5f));

        ChargeUltimateGauge(0.3f);

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
                Debug.Log("Ÿ���� ���õ��� �ʾҽ��ϴ�.");
                return; // Ÿ���� ���õ��� �ʾ����� ��ȯ
            }

            var enemiesHalf = allSelectedTarget.Count / 2;

            for (int i = 0; i < enemiesHalf; i++)
            {
                var target = allSelectedTarget[i];
                FaceTarget(target.transform.position);
            }

            Animator.SetTrigger("Battle Ultimate");

            SetUltimateReady(false);

            // �ñر� ��� �� UI ������Ʈ
            uiSystem.ReturnUltimateButton(playerIndex);
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        BasicAttackButton.basicCallAttack -= BasicAttack;
        SkillAttackButton.skillCallAttack -= SkillAttack;
        UltimateAttackButton.ultimateCallAttack -= UltimateAttack;
    }

    IEnumerator MoveToTarget(Vector3 targetPosition, string attackTrigger, float stopDistance)
    {
        FaceTarget(targetPosition);

        CharacterController controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError("CharacterController ������Ʈ�� �����ϴ�!");
            yield break;
        }

        Vector3 adjustedTargetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        animator.SetFloat("Battle Move", 1f);
        float currentDistance = Vector3.Distance(transform.position, adjustedTargetPosition);
        while (currentDistance > stopDistance)
        {
            Vector3 direction = (adjustedTargetPosition - transform.position).normalized;
            Vector3 move = direction * moveSpeed * Time.deltaTime;

            controller.Move(move);

            yield return null;

            currentDistance = Vector3.Distance(transform.position, adjustedTargetPosition);
        }

        Animator.SetTrigger(attackTrigger);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(stateInfo.length); // �ִϸ��̼��� ���� �ð��� ��� (�ʿ信 ���� ����)
    }

    // ���� ���� �ִϸ��̼� �̺�Ʈ
    public override void ApplyDamage()
    {
        if (selectedTarget == null)
        {
            Debug.Log("Ÿ���� ���õ��� �ʾҽ��ϴ�.");
            return; // Ÿ���� ���õ��� �ʾ����� ��ȯ
        }

        // ���õ� Ÿ���� BattleEnemyFSMController Ÿ������ Ȯ�� �� Hit �޼��� ȣ��
        if (selectedTarget is BattleFSMController)
        {
            (selectedTarget as BattleFSMController).Hit(Damage);
        }
    }

    public override void AllApplyDamage()
    {
        if (allSelectedTarget == null || allSelectedTarget.Count == 0)
        {
            Debug.Log("Ÿ���� ���õ��� �ʾҽ��ϴ�.");
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
        }

        Instantiate(BurstingEffectPrefabs, UltimatePos.position, Quaternion.Euler(90f, 0f, 0f));
        AudioManager.instance.EffectPlay(ultimateSFXSound);
    }
}

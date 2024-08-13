using System.Collections;
using UnityEngine;

public class UnityChan : BattleCharacterState, IActionButton
{
    // 궁극기 이팩트 파티클
    [SerializeField] private GameObject BurstingEffectPrefabs;

    [SerializeField] private Transform UltimatePos;

    public override void StartTurn()
    {
        base.StartTurn();

        Debug.Log(name + "의 턴입니다.");

        if (isUltimateReady)
        {
            uiSystem.UseUltimate(PlayerUltimateSprite);
        }

        BasicAttackButton.basicCallAttack += BasicAttack;
        SkillAttackButton.skillCallAttack += SkillAttack;
        UltimateAttackButton.ultimateCallAttack += UltimateAttack;
    }

    // 기본 공격
    public override void BasicAttack()
    {
        base.BasicAttack();

        selectedTarget = SelectTarget();
        if (selectedTarget == null)
        {
            Debug.Log("타겟이 선택되지 않았습니다.");
            return; // 타겟이 선택되지 않았으면 반환
        }

        // 공격1 로직
        Debug.Log("기본 공격 발동");

        // 캐릭터를 몬스터 앞으로 이동
        Vector3 targetPosition = selectedTarget.transform.position;
        targetPosition.y -= 0.4f;

        StartCoroutine(MoveToTarget(targetPosition, "Battle Attack", 1f));

        ChargeUltimateGauge(0.2f);

        battleSystem.GetPlayerSystem().PlayerTurnEnd();
    }

    // 스킬 공격
    public override void SkillAttack()
    {
        base.SkillAttack();

        selectedTarget = SelectTarget();

        if (selectedTarget == null)
        {
            Debug.Log("타겟이 선택되지 않았습니다.");
            return; // 타겟이 선택되지 않았으면 반환
        }

        // 공격2 로직
        Debug.Log("(서포트)스킬 발동");

        // 캐릭터를 몬스터 앞으로 이동
        Vector3 targetPosition = selectedTarget.transform.position;
        targetPosition.y -= 0.4f;
        StartCoroutine(MoveToTarget(targetPosition, "Battle Skill", 5f));

        ChargeUltimateGauge(0.3f);

        battleSystem.GetPlayerSystem().PlayerTurnEnd();
    }

    // 필살기 공격
    public override void UltimateAttack()
    {
        // 필살기 로직
        if (isUltimateReady == true)
        {
            base.UltimateAttack();

            allSelectedTarget = SelectAllTarget();

            if (allSelectedTarget == null)
            {
                Debug.Log("타겟이 선택되지 않았습니다.");
                return; // 타겟이 선택되지 않았으면 반환
            }

            var enemiesHalf = allSelectedTarget.Count / 2;

            for (int i = 0; i < enemiesHalf; i++)
            {
                var target = allSelectedTarget[i];
                FaceTarget(target.transform.position);
            }

            Animator.SetTrigger("Battle Ultimate");

            SetUltimateReady(false);

            // 궁극기 사용 후 UI 업데이트
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
            Debug.LogError("CharacterController 컴포넌트가 없습니다!");
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

        yield return new WaitForSeconds(stateInfo.length); // 애니메이션이 끝날 시간을 대기 (필요에 따라 조정)
    }

    // 공격 판정 애니메이션 이벤트
    public override void ApplyDamage()
    {
        if (selectedTarget == null)
        {
            Debug.Log("타겟이 선택되지 않았습니다.");
            return; // 타겟이 선택되지 않았으면 반환
        }

        // 선택된 타겟이 BattleEnemyFSMController 타입인지 확인 후 Hit 메서드 호출
        if (selectedTarget is BattleFSMController)
        {
            (selectedTarget as BattleFSMController).Hit(Damage);
        }
    }

    public override void AllApplyDamage()
    {
        if (allSelectedTarget == null || allSelectedTarget.Count == 0)
        {
            Debug.Log("타겟이 선택되지 않았습니다.");
            return;
        }

        foreach (var target in allSelectedTarget)
        {
            // 선택된 타겟이 BattleEnemyFSMController 타입인지 확인 후 Hit 메서드 호출
            if (target is BattleFSMController enemy)
            {
                enemy.Hit(Damage);
            }
            else
            {
                Debug.LogWarning($"{target.name}은(는) 유효한 타겟 타입이 아닙니다.");
            }
        }

        Instantiate(BurstingEffectPrefabs, UltimatePos.position, Quaternion.Euler(90f, 0f, 0f));
        AudioManager.instance.EffectPlay(ultimateSFXSound);
    }
}

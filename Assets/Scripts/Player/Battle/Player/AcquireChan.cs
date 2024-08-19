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

    // 기본 공격
    public override void BasicAttack()
    {
        base.BasicAttack();

        selectedTarget = SelectTarget();

        if (selectedTarget == null)
        {
            return; // 타겟이 선택되지 않았으면 반환
        }

        FaceTarget(selectedTarget.transform.position);

        Animator.SetTrigger("Battle Attack");

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
            return; // 타겟이 선택되지 않았으면 반환
        }

        FaceTarget(selectedTarget.transform.position);

        ChargeUltimateGauge(0.3f);

        Animator.SetTrigger("Battle Skill");

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
                return; // 타겟이 선택되지 않았으면 반환
            }

            Animator.SetTrigger("Battle Ultimate");

            SetUltimateReady(false);

            // 궁극기 사용 후 UI 업데이트
            uiSystem.ReturnUltimateButton(playerIndex);
        }
    }

    // 턴 끝남 
    public override void EndTurn()
    {
        base.EndTurn();

        BasicAttackButton.basicCallAttack -= BasicAttack;
        SkillAttackButton.skillCallAttack -= SkillAttack;
        UltimateAttackButton.ultimateCallAttack -= UltimateAttack;
    }

    // 공격 판정 애니메이션 이벤트
    public override void ApplyDamage()
    {
        if (selectedTarget == null)
        {
            return; // 타겟이 선택되지 않았으면 반환
        }

        // 선택된 타겟이 BattleEnemyFSMController 타입인지 확인 후 Hit 메서드 호출
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
            // 선택된 타겟이 BattleEnemyFSMController 타입인지 확인 후 Hit 메서드 호출
            if (target is BattleFSMController enemy)
            {
                enemy.Hit(Damage);
            }
            else
            {
                Debug.LogWarning($"{target.name}은(는) 유효한 타겟 타입이 아닙니다.");
            }

            GameObject ball = Instantiate(ultimateBallPrefab, airBallPos.position, Quaternion.identity);
            ball.GetComponent<Ball>().SetTarget(target);
            AudioManager.instance.EffectPlay(ultimateSFXSound);
        }
    }
}

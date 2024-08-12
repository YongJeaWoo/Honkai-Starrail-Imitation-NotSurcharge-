using System;
using System.Collections;
using UnityEngine;

public class Mryotaisu : BattleCharacterState, IActionButton
{
    [SerializeField] private ParticleSystem healingPrefab;
    [SerializeField] private ParticleSystem allHealingPrefab;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballPos;

    private bool isSelectingSupportSkill = false;

    public override void StartTurn()
    {
        base.StartTurn();

        Debug.Log(name + "�� ���Դϴ�.");

        if (isUltimateReady)
        {
            var targetSystem = battleSystem.GetComponentInChildren<TargetSelectSystem>();
            Camera.main.transform.position = targetSystem.TargetCamPos.position;
            Camera.main.transform.rotation = targetSystem.TargetCamPos.rotation;
            uiSystem.UseUltimate(PlayerUltimateSprite);
        }

        BasicAttackButton.basicCallAttack += BasicAttack;
        SkillAttackButton.skillCallAttack += SkillAttack;
        UltimateAttackButton.ultimateCallAttack += UltimateAttack;
    }

    private void StartSupportSkillSelection()
    {
        // ī�޶� �� ��ġ ����
        var targetSystem = battleSystem.GetComponentInChildren<TargetSelectSystem>();
        targetSystem.OriginCameraPos = Camera.main.transform.position;
        targetSystem.OriginCameraRot = Camera.main.transform.rotation;

        // ī�޶� �̵� ���� �߰�
        Camera.main.transform.position = targetSystem.TargetCamPos.position;
        Camera.main.transform.rotation = targetSystem.TargetCamPos.rotation;

        targetSystem.IsSelectingSupportTarget = true;
        targetSystem.SelectedIndex = 0;
        targetSystem.UpdateSupportTarget();

        isSelectingSupportSkill = true;
    }

    private void ExitSupportSkillMode()
    {
        var uiSystem = battleSystem.GetComponentInChildren<TargetUISystem>();
        var targetSystem = battleSystem.GetComponentInChildren<TargetSelectSystem>();

        uiSystem.PlayerAllCycleUI(false);
        targetSystem.IsSelectingSupportTarget = false;
        targetSystem.SelectedIndex = 0;
        isSelectingSupportSkill = false;
    }

    private void ExecuteSupportSkill()
    {
        selectedTarget = SelectPlayerTarget();

        if (selectedTarget == null)
        {
            Debug.Log("Ÿ���� ���õ��� �ʾҽ��ϴ�.");
            return;
        }

        Debug.Log("����Ʈ ��ų �ߵ�");

        // ����Ʈ ��ų (��)
        var maxHp = health.GetMaxHp();
        float healAmount = maxHp * 0.2f;

        selectedTarget.health.HpUp(healAmount);

        Animator.SetTrigger("Battle Skill");

        FaceTarget(selectedTarget.transform.position);

        // ��ƼŬ ���
        Vector3 particlePosition = selectedTarget.transform.position;
        ParticleSystem supportParticle = Instantiate(healingPrefab, particlePosition, healingPrefab.transform.rotation);
        AudioManager.instance.EffectPlay(skillSFXSound);

        battleSystem.GetPlayerSystem().PlayerTurnEnd();

        ChargeUltimateGauge(0.3f);

        int playerIndex = selectedTarget.GetComponent<BattleCharacterState>().GatPlayerIndex();

        StartCoroutine(ParticleSystemPlayAndDestroy(supportParticle));
    }

    // �⺻ ����
    public override void BasicAttack()
    {
        base.BasicAttack();

        if (isSelectingSupportSkill)
        {
            ExitSupportSkillMode();

            var uiSystem = battleSystem.GetComponentInChildren<TargetUISystem>();
            var targetSystem = battleSystem.GetComponentInChildren<TargetSelectSystem>();

            uiSystem.EnemyAllCycleUI(true);
            targetSystem.UpdateTarget();
            Camera.main.transform.position = targetSystem.OriginCameraPos;
            Camera.main.transform.rotation = targetSystem.OriginCameraRot;
            return;
        }

        selectedTarget = SelectTarget();

        if (selectedTarget == null)
        {
            Debug.Log("Ÿ���� ���õ��� �ʾҽ��ϴ�.");
            return; // Ÿ���� ���õ��� �ʾ����� ��ȯ
        }

        FaceTarget(selectedTarget.transform.position);

        // ����1 ����
        Debug.Log("�⺻ ���� �ߵ�");

        Animator.SetTrigger("Battle Attack");

        ChargeUltimateGauge(0.2f);

        battleSystem.GetPlayerSystem().PlayerTurnEnd();
    }

    // ��ų ����
    public override void SkillAttack()
    {
        if (!isSelectingSupportSkill)
        {
            StartSupportSkillSelection();
        }
        else
        {
            base.SkillAttack();
            ExecuteSupportSkill();
        }
    }

    // �ʻ�� ����
    public override void UltimateAttack()
    {
        // �ʻ�� ����
        if (isUltimateReady == true)
        {
            base.UltimateAttack();

            allSelectedTarget = SelectAllPlayerTarget();

            if (allSelectedTarget == null || allSelectedTarget.Count == 0)
            {
                Debug.Log("Ÿ���� ���õ��� �ʾҽ��ϴ�.");
                return; // Ÿ���� ���õ��� �ʾ����� ��ȯ
            }

            Debug.Log("�ʻ�� ��ų �ߵ�");

            Animator.SetTrigger("Battle Ultimate");

            // ��� �÷��̾�� �� ����
            var maxHp = health.GetMaxHp();
            float healAmount = maxHp * 0.2f;

            foreach (var target in allSelectedTarget)
            {
                target.health.HpUp(healAmount);
            }

            Vector3 particlePosition = new Vector3(-1.5f, 0, -2f);
            ParticleSystem supportParticle = Instantiate(allHealingPrefab, particlePosition, allHealingPrefab.transform.rotation);
            AudioManager.instance.EffectPlay(ultimateSFXSound);

            SetUltimateReady(false);

            battleSystem.GetPlayerSystem().PlayerTurnEnd();

            // �ñر� ��� �� UI ������Ʈ
            uiSystem.ReturnUltimateButton(playerIndex);

            StartCoroutine(ParticleSystemPlayAndDestroy(supportParticle));
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        BasicAttackButton.basicCallAttack -= BasicAttack;
        SkillAttackButton.skillCallAttack -= SkillAttack;
        UltimateAttackButton.ultimateCallAttack -= UltimateAttack;

        // ����Ʈ ��ų ��� �� �⺻ ���·� ���ư���
        ExitSupportSkillMode();
    }

    IEnumerator ParticleSystemPlayAndDestroy(ParticleSystem particleSystem)
    {
        particleSystem.Play();
        yield return new WaitForSeconds(particleSystem.main.duration);
        Destroy(particleSystem.gameObject);
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

    public void BallEvent()
    {
        GameObject ball = Instantiate(ballPrefab, ballPos.position, Quaternion.identity);
        ball.GetComponent<Ball>().SetTarget(selectedTarget);
        AudioManager.instance.EffectPlay(attackSFXSound);
    }
}

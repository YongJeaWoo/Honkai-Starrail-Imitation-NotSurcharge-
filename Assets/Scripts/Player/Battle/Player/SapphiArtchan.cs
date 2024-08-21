using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.ProBuilder;

public class SapphiArtchan : BattleCharacterState, IActionButton
{
    [SerializeField] private ParticleSystem suportPrefab;

    [SerializeField] private GameObject w_Rifle;
    [SerializeField] private GameObject a_Rifle;

    [SerializeField] private GameObject w_Gun;
    [SerializeField] private GameObject a_Gun;

    [SerializeField] private PlayableDirector UltimateTimeLine;

    private bool isSelectingSupportSkill = false;

    [SerializeField] private GameObject projectilePrefab; // �Ѿ� ������
    [SerializeField] private Transform firePoint; // �Ѿ� �߻� ��ġ

    [SerializeField] private GameObject UltimateBulletPrefab;
    [SerializeField] private GameObject UltimateExplosionPrefab;

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
            return;
        }

        // ����Ʈ ��ų (���� ���ܿ���)
        selectedTarget.ActionPoint = 0f;

        Animator.SetTrigger("Battle Skill");

        FaceTarget(selectedTarget.transform.position);

        // ��ƼŬ ���
        Vector3 particlePosition = selectedTarget.transform.position + new Vector3(0f, 4f, 0f); // ĳ���� �Ӹ� ��
        ParticleSystem supportParticle = Instantiate(suportPrefab, particlePosition, suportPrefab.transform.rotation);
        AudioManager.instance.EffectPlay(skillSFXSound);

        battleSystem.GetPlayerSystem().PlayerTurnEnd();

        ChargeUltimateGauge(0.3f);

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
            return; // Ÿ���� ���õ��� �ʾ����� ��ȯ
        }

        FaceTarget(selectedTarget.transform.position);

        Animator.SetTrigger("Battle Attack");

        AudioManager.instance.EffectPlay(attackSound);

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

            allSelectedTarget = SelectAllTarget();

            if (selectedTarget == null)
            {
                return; // Ÿ���� ���õ��� �ʾ����� ��ȯ
            }

            UltimateTimeLine.Play();

            SetUltimateReady(false);

            battleSystem.GetPlayerSystem().PlayerTurnEnd();

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
    }

    public void BulletEnevt()
    {
        GameObject ball = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        ball.GetComponent<Ball>().SetTarget(selectedTarget);
        AudioManager.instance.EffectPlay(attackSFXSound);
    }

    public void FireAndExplodeBullet()
    {
        // �Ѿ� ���� �� �߻�
        GameObject projectile = Instantiate(UltimateBulletPrefab, firePoint.position, firePoint.rotation);

        // �Ѿ��� Ÿ���� ���� �߻�ǵ��� ����
        Vector3 direction = projectile.transform.forward;

        // ���� ���͸� ����ȭ
        direction.Normalize();

        projectile.GetComponent<Rigidbody>().velocity = direction * 30f;

        // �Ѿ��� 0.5�� �Ŀ� �ı��ǰ� ���� ����Ʈ�� �����ϵ��� ��
        StartCoroutine(DestroyProjectileAfterDelay(projectile, 0.3f));

        AudioManager.instance.EffectPlay(ultimateSFXSound);
    }

    private IEnumerator DestroyProjectileAfterDelay(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);

        // �Ѿ� �ı�
        Vector3 explosionPosition = projectile.transform.position;
        Destroy(projectile);

        // ���� ����Ʈ ����
        GameObject explosion = Instantiate(UltimateExplosionPrefab, explosionPosition, Quaternion.identity);

        // ���� ����Ʈ�� ���� �ð� �Ŀ� �ı�
        Destroy(explosion, 2f);
    }


    public void WeaponChange()
    {
        w_Rifle.SetActive(false);
        a_Rifle.SetActive(true);
    }

    public void WeaponReturn()
    {
        w_Rifle.SetActive(true);
        a_Rifle.SetActive(false);
    }

    public void GunWeaponChange()
    {
        w_Gun.SetActive(false);
        a_Gun.SetActive(true);
    }

    public void GunWeaponReturn()
    {
        w_Gun.SetActive(true);
        a_Gun.SetActive(false);
    }


}

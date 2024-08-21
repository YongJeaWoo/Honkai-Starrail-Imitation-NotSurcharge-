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

    [SerializeField] private GameObject projectilePrefab; // 총알 프리팹
    [SerializeField] private Transform firePoint; // 총알 발사 위치

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
        // 카메라 본 위치 저장
        var targetSystem = battleSystem.GetComponentInChildren<TargetSelectSystem>();
        targetSystem.OriginCameraPos = Camera.main.transform.position;
        targetSystem.OriginCameraRot = Camera.main.transform.rotation;

        // 카메라 이동 로직 추가
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

        // 서포트 스킬 (순서 땡겨오기)
        selectedTarget.ActionPoint = 0f;

        Animator.SetTrigger("Battle Skill");

        FaceTarget(selectedTarget.transform.position);

        // 파티클 재생
        Vector3 particlePosition = selectedTarget.transform.position + new Vector3(0f, 4f, 0f); // 캐릭터 머리 위
        ParticleSystem supportParticle = Instantiate(suportPrefab, particlePosition, suportPrefab.transform.rotation);
        AudioManager.instance.EffectPlay(skillSFXSound);

        battleSystem.GetPlayerSystem().PlayerTurnEnd();

        ChargeUltimateGauge(0.3f);

        StartCoroutine(ParticleSystemPlayAndDestroy(supportParticle));
    }

    // 기본 공격
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
            return; // 타겟이 선택되지 않았으면 반환
        }

        FaceTarget(selectedTarget.transform.position);

        Animator.SetTrigger("Battle Attack");

        AudioManager.instance.EffectPlay(attackSound);

        ChargeUltimateGauge(0.2f);

        battleSystem.GetPlayerSystem().PlayerTurnEnd();
    }

    // 스킬 공격
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

    // 필살기 공격
    public override void UltimateAttack()
    {
        // 필살기 로직
        if (isUltimateReady == true)
        {
            base.UltimateAttack();

            allSelectedTarget = SelectAllTarget();

            if (selectedTarget == null)
            {
                return; // 타겟이 선택되지 않았으면 반환
            }

            UltimateTimeLine.Play();

            SetUltimateReady(false);

            battleSystem.GetPlayerSystem().PlayerTurnEnd();

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

        // 서포트 스킬 사용 후 기본 상태로 돌아가기
        ExitSupportSkillMode();
    }

    IEnumerator ParticleSystemPlayAndDestroy(ParticleSystem particleSystem)
    {
        particleSystem.Play();
        yield return new WaitForSeconds(particleSystem.main.duration);
        Destroy(particleSystem.gameObject);
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
        // 총알 생성 및 발사
        GameObject projectile = Instantiate(UltimateBulletPrefab, firePoint.position, firePoint.rotation);

        // 총알이 타겟을 향해 발사되도록 설정
        Vector3 direction = projectile.transform.forward;

        // 방향 벡터를 정규화
        direction.Normalize();

        projectile.GetComponent<Rigidbody>().velocity = direction * 30f;

        // 총알이 0.5초 후에 파괴되고 폭발 이펙트를 생성하도록 함
        StartCoroutine(DestroyProjectileAfterDelay(projectile, 0.3f));

        AudioManager.instance.EffectPlay(ultimateSFXSound);
    }

    private IEnumerator DestroyProjectileAfterDelay(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 총알 파괴
        Vector3 explosionPosition = projectile.transform.position;
        Destroy(projectile);

        // 폭발 이펙트 생성
        GameObject explosion = Instantiate(UltimateExplosionPrefab, explosionPosition, Quaternion.identity);

        // 폭발 이펙트를 일정 시간 후에 파괴
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

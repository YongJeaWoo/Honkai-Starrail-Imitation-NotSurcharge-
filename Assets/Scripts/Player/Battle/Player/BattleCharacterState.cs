using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 능력치 관리
public class BattleCharacterState : BattleBehaviourComponent
{
    // 캐릭터 스텟 참조
    [SerializeField] protected CharacterStateSOJ state;

    // 애니메이터 컴포넌트
    [SerializeField] protected Animator animator;

    // 카메라 위치
    [SerializeField] protected Transform lookPos;

    // Apply Root Motion으로 인해 원래 자리로 돌아가기 위한 변수
    [SerializeField] protected float moveSpeed;
    protected Vector3 originalPosition;
    protected Quaternion originalRotation;

    // 캐릭터 타입
    [SerializeField] private E_CharacterType characterType;

    protected BattleBehaviourComponent selectedTarget;
    protected List<BattleBehaviourComponent> allSelectedTarget;

    // 궁 사용 가능 여부
    protected bool isUltimateReady;

    protected UiSystem uiSystem;
    protected int playerIndex;

    [SerializeField] private AudioClip turnSound;
    [SerializeField] private AudioClip[] hitSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip skillSound;
    [SerializeField] private AudioClip ultimateSound;
    [SerializeField] private AudioClip clearSound;
    [SerializeField] protected AudioClip attackSFXSound;
    [SerializeField] protected AudioClip skillSFXSound;
    [SerializeField] protected AudioClip ultimateSFXSound;

    public Animator Animator { get => animator; set => animator = value; }

    // 능력치 설정
    public void InitValues()
    {
        MaxHp = state.Maxhp;
        CurrentHp = MaxHp;
        damage = state.Damage;
        ActionPoint = state.Acting;
        UltimateGauge = state.UltimateGauge;
        IsAlive = true;
        isUltimateReady = state.IsUltimateAttack;
        Sprite = state.PlayerIcon;
        PlayerUltimateSprite = state.PlayerFullBody;

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        Animator.SetBool("Battle Start", true);
    }

    public void SetUiSystem(int index)
    {
        uiSystem = battleSystem.GetUiSystem();
        playerIndex = index;
    }

    public void ChargeUltimateGauge(float amount)
    {
        UltimateGauge += amount;
        uiSystem.RefreshUltimateGauge(playerIndex, amount);
    }

    public void SetUltimateReady(bool isReady)
    {
        isUltimateReady = isReady;
    }

    // 타겟 선택 메서드 (UI 연동)
    public BattleBehaviourComponent SelectTarget()
    {
        var targetSystem = battleSystem.GetComponentInChildren<TargetSelectSystem>();
        int selectedIndex = targetSystem.GetCurrentSelectedIndex();

        var enemyList = battleSystem.GetEnemySystem().GetEnemyList();
        if (enemyList == null || enemyList.Count == 0)
        {
            return null;
        }

        // 살아있는 첫 번째 몬스터를 찾습니다.
        for (int i = selectedIndex; i < enemyList.Count; i++)
        {
            if (enemyList[i].IsAlive)
            {
                return enemyList[i];
            }
        }

        // 모든 몬스터가 죽었다면 null을 반환합니다.
        return null;
    }

    public List<BattleBehaviourComponent> SelectAllTarget()
    {
        var enemyList = battleSystem.GetEnemySystem().GetEnemyList();
        if (enemyList == null || enemyList.Count == 0)
        {
            return null;
        }

        // 살아있는 모든 적을 리스트에 추가
        List<BattleBehaviourComponent> validTargets = new List<BattleBehaviourComponent>();
        foreach (var enemy in enemyList)
        {
            if (enemy.IsAlive)
            {
                validTargets.Add(enemy);
            }
        }

        if (validTargets.Count == 0)
        {
            return null;
        }

        return validTargets;
    }

    // 타겟 선택 메서드 (UI 연동)
    public BattleBehaviourComponent SelectPlayerTarget()
    {
        var targetSystem = battleSystem.GetComponentInChildren<TargetSelectSystem>();
        int selectedIndex = targetSystem.GetCurrentSelectedIndex();

        var playerList = battleSystem.GetPlayerSystem().GetPlayerList();
        if (playerList == null || playerList.Count == 0 || selectedIndex < 0 || selectedIndex >= playerList.Count)
        {
            return null;
        }

        BattleBehaviourComponent selectedTarget = playerList[selectedIndex];
        return selectedTarget;
    }

    public List<BattleBehaviourComponent> SelectAllPlayerTarget()
    {
        var playerList = battleSystem.GetPlayerSystem().GetPlayerList();
        if (playerList == null || playerList.Count == 0)
        {
            return null;
        }

        // 살아있는 모든 적을 리스트에 추가
        List<BattleBehaviourComponent> validTargets = new List<BattleBehaviourComponent>();

        foreach (var player in playerList)
        {
            if (player.IsAlive)
            {
                validTargets.Add(player);
            }
        }

        return validTargets;
    }

    public virtual void EndTurn()
    {
        // 공격이 끝나면 원래 위치와 회전으로 돌아감
        StartCoroutine(MoveBackToOriginalPosition(originalPosition, originalRotation));
    }

    public void Hit(float dmg)
    {
        Animator.applyRootMotion = false;

        // 맞는 애니메이션
        Animator.SetTrigger("Battle Hit");

        // 랜덤한 히트 사운드를 재생합니다.
        if (hitSound.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSound.Length);
            AudioManager.instance.EffectPlay(hitSound[randomIndex]);
        }

        health.HpDown(dmg);

        if (health.GetCurrentHp() <= 0f)
        {
            Die();
        }

        Vector3 enemiesMidpoint = battleSystem.GetEnemySystem().GetEnemiesMidPoint();

        battleSystem.MoveCameraToPosition(lookPos.position, enemiesMidpoint);
    }

    public void HitAll(float dmg)
    {
        Animator.applyRootMotion = false;

        // 맞는 애니메이션
        Animator.SetTrigger("Battle Hit");

        // 랜덤한 히트 사운드를 재생합니다.
        if (hitSound.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSound.Length);
            AudioManager.instance.EffectPlay(hitSound[randomIndex]);
        }

        health.HpDown(dmg);

        if (health.GetCurrentHp() <= 0f)
        {
            Die();
        }

        // 전체적인 카메라 시점 설정
        SetOverallCameraView();
    }

    private void SetOverallCameraView()
    {
        // GetAllTargetPos() 메소드로부터 위치와 방향 정보를 가져옵니다.
        Transform targetTransform = battleSystem.GetPlayerSystem().GetAllTargetPos();

        // 카메라를 플레이어들이 모두 보이는 위치로 이동하고 바라보는 방향을 설정
        Vector3 cameraPosition = new Vector3(-1.5f, 5, -5); // 직접 정의한 카메라 위치
        Vector3 lookAtPosition = targetTransform.position; // GetAllTargetPos()로 받은 위치를 바라보도록 설정

        battleSystem.MoveCameraToPosition(cameraPosition, lookAtPosition);
    }

    private void Die()
    {
        Animator.applyRootMotion = false;
        AudioManager.instance.EffectPlay(deathSound);
        // 죽는 애니메이션
        Animator.SetTrigger("Death");
        IsAlive = false;
    }

    public override void StartTurn()
    {
        Vector3 enemiesMidpoint = battleSystem.GetEnemySystem().GetEnemiesMidPoint();

        battleSystem.MoveCameraToPosition(lookPos.position, enemiesMidpoint);
        Animator.applyRootMotion = true;

        AudioManager.instance.EffectPlay(turnSound);
    }

    // 타겟을 바라보는 메소드
    public void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }

    protected IEnumerator MoveBackToOriginalPosition(Vector3 originalPosition, Quaternion originalRotation)
    {
        CharacterController controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            yield break;
        }

        animator.SetFloat("Battle Move", 1f);

        // 원래 위치로 이동
        while (Vector3.Distance(transform.position, originalPosition) > 0.5f)
        {
            Vector3 direction = (originalPosition - transform.position).normalized;
            Vector3 move = direction * moveSpeed * Time.deltaTime;

            controller.Move(move);
            
            yield return null;
        }

        // 원래 회전으로 돌아감
        while (Quaternion.Angle(transform.rotation, originalRotation) > 0.5f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, Time.deltaTime * moveSpeed * 10); // 회전 속도는 위치 이동 속도의 10배로 설정
            yield return null;
        }

        animator.SetFloat("Battle Move", 0f);

        ActionPoint = state.Acting;
        EventActionToTurnOver();
    }

    public virtual void ApplyDamage()
    {

    }

    public virtual void AllApplyDamage()
    {

    }

    public virtual void BasicAttack()
    {
        uiSystem.ControllActingPoint(false);
        AudioManager.instance.EffectPlay(attackSound);
    }

    public virtual void SkillAttack()
    {
        battleSystem.GetPlayerSystem().SetActionPoint(true);
        uiSystem.ControllActingPoint(true);
        AudioManager.instance.EffectPlay(skillSound);
    }

    public virtual void UltimateAttack()
    {
        AudioManager.instance.EffectPlay(ultimateSound);
    }

    public bool IsUltimateReady() => isUltimateReady;
    public int GatPlayerIndex() => playerIndex;

    
    public E_CharacterType CharacterType() => characterType;

    public CharacterStateSOJ GetSOJData() => state;

    #region Take Damage State

    public void AddState(float value, float state)
    {
        state += value;
    }

    public void MultiState(float value, float state)
    {
        state *= value;
    }

    public void RemoveState(float value, float state)
    {
        state -= value;
    }

    public void DivitionState(float value, float state)
    {
        state /= value;
    }

    #endregion
}
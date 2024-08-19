using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ɷ�ġ ����
public class BattleCharacterState : BattleBehaviourComponent
{
    // ĳ���� ���� ����
    [SerializeField] protected CharacterStateSOJ state;

    // �ִϸ����� ������Ʈ
    [SerializeField] protected Animator animator;

    // ī�޶� ��ġ
    [SerializeField] protected Transform lookPos;

    // Apply Root Motion���� ���� ���� �ڸ��� ���ư��� ���� ����
    [SerializeField] protected float moveSpeed;
    protected Vector3 originalPosition;
    protected Quaternion originalRotation;

    // ĳ���� Ÿ��
    [SerializeField] private E_CharacterType characterType;

    protected BattleBehaviourComponent selectedTarget;
    protected List<BattleBehaviourComponent> allSelectedTarget;

    // �� ��� ���� ����
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

    // �ɷ�ġ ����
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

    // Ÿ�� ���� �޼��� (UI ����)
    public BattleBehaviourComponent SelectTarget()
    {
        var targetSystem = battleSystem.GetComponentInChildren<TargetSelectSystem>();
        int selectedIndex = targetSystem.GetCurrentSelectedIndex();

        var enemyList = battleSystem.GetEnemySystem().GetEnemyList();
        if (enemyList == null || enemyList.Count == 0)
        {
            return null;
        }

        // ����ִ� ù ��° ���͸� ã���ϴ�.
        for (int i = selectedIndex; i < enemyList.Count; i++)
        {
            if (enemyList[i].IsAlive)
            {
                return enemyList[i];
            }
        }

        // ��� ���Ͱ� �׾��ٸ� null�� ��ȯ�մϴ�.
        return null;
    }

    public List<BattleBehaviourComponent> SelectAllTarget()
    {
        var enemyList = battleSystem.GetEnemySystem().GetEnemyList();
        if (enemyList == null || enemyList.Count == 0)
        {
            return null;
        }

        // ����ִ� ��� ���� ����Ʈ�� �߰�
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

    // Ÿ�� ���� �޼��� (UI ����)
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

        // ����ִ� ��� ���� ����Ʈ�� �߰�
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
        // ������ ������ ���� ��ġ�� ȸ������ ���ư�
        StartCoroutine(MoveBackToOriginalPosition(originalPosition, originalRotation));
    }

    public void Hit(float dmg)
    {
        Animator.applyRootMotion = false;

        // �´� �ִϸ��̼�
        Animator.SetTrigger("Battle Hit");

        // ������ ��Ʈ ���带 ����մϴ�.
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

        // �´� �ִϸ��̼�
        Animator.SetTrigger("Battle Hit");

        // ������ ��Ʈ ���带 ����մϴ�.
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

        // ��ü���� ī�޶� ���� ����
        SetOverallCameraView();
    }

    private void SetOverallCameraView()
    {
        // GetAllTargetPos() �޼ҵ�κ��� ��ġ�� ���� ������ �����ɴϴ�.
        Transform targetTransform = battleSystem.GetPlayerSystem().GetAllTargetPos();

        // ī�޶� �÷��̾���� ��� ���̴� ��ġ�� �̵��ϰ� �ٶ󺸴� ������ ����
        Vector3 cameraPosition = new Vector3(-1.5f, 5, -5); // ���� ������ ī�޶� ��ġ
        Vector3 lookAtPosition = targetTransform.position; // GetAllTargetPos()�� ���� ��ġ�� �ٶ󺸵��� ����

        battleSystem.MoveCameraToPosition(cameraPosition, lookAtPosition);
    }

    private void Die()
    {
        Animator.applyRootMotion = false;
        AudioManager.instance.EffectPlay(deathSound);
        // �״� �ִϸ��̼�
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

    // Ÿ���� �ٶ󺸴� �޼ҵ�
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

        // ���� ��ġ�� �̵�
        while (Vector3.Distance(transform.position, originalPosition) > 0.5f)
        {
            Vector3 direction = (originalPosition - transform.position).normalized;
            Vector3 move = direction * moveSpeed * Time.deltaTime;

            controller.Move(move);
            
            yield return null;
        }

        // ���� ȸ������ ���ư�
        while (Quaternion.Angle(transform.rotation, originalRotation) > 0.5f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, Time.deltaTime * moveSpeed * 10); // ȸ�� �ӵ��� ��ġ �̵� �ӵ��� 10��� ����
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
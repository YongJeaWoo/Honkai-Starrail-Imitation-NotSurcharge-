using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBossFSMController : BattleFSMController
{
    // ������ ���� ���� ���� ���� ������Ʈ
    [SerializeField] private BattleBossState currentState;

    // ������ ��� ���� ������Ʈ��
    [SerializeField] private BattleBossState[] EnemyStatas;

    public int attackCount;

    private Animator animator;
    

    // ���� ��ȯ �޼ҵ�
    public void TransactionToState(E_BattleBossState state)
    {
        currentState?.ExitState(); // ���� ���� ����
        currentState = EnemyStatas[(int)state]; // ���� ��ȯ ó��
        currentState.EnterState(state); // ���ο� ���� ����
    }

    void Start()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
        // ��� ���·� ����
        TransactionToState(E_BattleBossState.Idle);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    // ����
    public override void StartTurn()
    {
        IsMyTurn = true;
        IsHit = false;
        TransactionToState(E_BattleBossState.Attack);
    }

    // �÷��̾�� ������ ����
    public override void Hit(float dmg)
    {
        // ���� ���°� �̹� ����� ���¸� �ǰ� ó������ ����
        if (currentState == EnemyStatas[(int)E_BattleBossState.Die]) return;

        health.HpDown(dmg);

        // �ǰ� ���·� ��ȯ
        TransactionToState(E_BattleBossState.Hit);

        // ������ ��Ʈ ���带 ����մϴ�.
        if (hitSound.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSound.Length);
            AudioManager.instance.EffectPlay(hitSound[randomIndex]);
        }

        Debug.Log(currentHp);

        if (health.GetCurrentHp() <= 0)
        {
            TransactionToState(E_BattleBossState.Die);

            AudioManager.instance.EffectPlay(deathSound);
        }
    }

    // ���� �� ����
    public override void SetRandomAttackTarget()
    {
        var enemyBattleSystem = battleSystem.GetEnemySystem();
        var targetLists = enemyBattleSystem.GetSelectTargetList();

        var aliveTargets = new List<BattleBehaviourComponent>();

        if (targetLists.Count > 0)
        {
            foreach (var target in targetLists)
            {
                var component = target.GetComponent<BattleBehaviourComponent>();

                if (component.IsAlive)
                {
                    aliveTargets.Add(target);
                }
            }

            if (aliveTargets.Count > 0)
            {
                int randomIndex = Random.Range(0, aliveTargets.Count);
                attackTarget = aliveTargets[randomIndex];
            }
            else
            {
                attackTarget = null;
            }
        }
    }
}

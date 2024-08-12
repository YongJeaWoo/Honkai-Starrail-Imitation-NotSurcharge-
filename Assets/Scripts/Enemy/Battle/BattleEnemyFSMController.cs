using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyFSMController : BattleFSMController
{
    // ������ ���� ���� ���� ���� ������Ʈ
    [SerializeField] private BattleEnemyState currentState;

    // ������ ��� ���� ������Ʈ��
    [SerializeField] private BattleEnemyState[] EnemyStatas;

    private Animator animator;

    // ���� ��ȯ �޼ҵ�
    public void TransactionToState(E_BattleEnemyState state)
    {
        currentState?.ExitState(); // ���� ���� ����
        currentState = EnemyStatas[(int)state]; // ���� ��ȯ ó��
        currentState.EnterState(state); // ���ο� ���� ����
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        // ��� ���·� ����
        TransactionToState(E_BattleEnemyState.Idle);
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    // ����
    public override void StartTurn()
    {
        IsMyTurn = true;

        TransactionToState(E_BattleEnemyState.Attack);
    }

    // �÷��̾�� ������ ����
    public override void Hit(float dmg)
    {
        // ���� ���°� �̹� ����� ���¸� �ǰ� ó������ ����
        if (currentState == EnemyStatas[(int)E_BattleEnemyState.Die]) return;

        health.HpDown(dmg);

        // �ǰ� ���·� ��ȯ
        TransactionToState(E_BattleEnemyState.Hit);

        // ������ ��Ʈ ���带 ����մϴ�.
        if (hitSound.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSound.Length);
            AudioManager.instance.EffectPlay(hitSound[randomIndex]);
        }

        if (health.GetCurrentHp() <= 0)
        {
            TransactionToState(E_BattleEnemyState.Die);

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

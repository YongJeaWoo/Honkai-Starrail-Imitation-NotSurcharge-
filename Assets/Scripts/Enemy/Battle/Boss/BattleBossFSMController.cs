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

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform buttlePos;

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

        if (attackCount < 2)
        {
            TransactionToState(E_BattleBossState.Attack);
        }
        else
        {
            attackCount = 0;

            EnemyBattleSystem enemyBattleSystem = GetBattleSystem().GetEnemySystem();
            bool canSummon = enemyBattleSystem.HasEmptySlots();

            if (canSummon)
            {
                // ü�¿� ���� ���� ���� ��ȯ
                if (health.GetCurrentHp() <= (health.GetMaxHp() / 2))
                {
                    TransactionToState(E_BattleBossState.Skill1);
                }
                else
                {
                    int randomSkill = Random.Range(0, 2);

                    Debug.Log(randomSkill);

                    if (randomSkill == 0)
                    {
                        TransactionToState(E_BattleBossState.Skill2);
                    }
                    else
                    {
                        TransactionToState(E_BattleBossState.Skill3);
                    }
                }

            }
        }
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

    public GameObject GatButtleObj() => bulletPrefab;
    public GameObject GatExplosionObj() => explosionPrefab;
    public Transform GetButtlePos() => buttlePos;
}

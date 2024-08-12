using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBossSkill1State : BattleBossState
{
    [SerializeField] private GameObject[] summonEnemyPrefabs;
    [SerializeField] private ParticleSystem summonEffect;

    public void SummonEnemy()
    {
        EnemyBattleSystem enemyBattleSystem = controller.GetBattleSystem().GetEnemySystem();
        BattleSystem battleSystem = controller.GetBattleSystem();
        TargetUISystem targetUISystem = battleSystem.GetComponentInChildren<TargetUISystem>(); 

        int emptySlots = enemyBattleSystem.GetEnemyPos().Length - enemyBattleSystem.GetEnemyList().Count;

        if (emptySlots > 0)
        {
            for (int i = 0; i < emptySlots; i++)
            {
                int spawnIndex = -1;
                for (int j = 0; j < enemyBattleSystem.GetEnemyPos().Length; j++)
                {
                    if (enemyBattleSystem.GetEnemyPos()[j].childCount == 0)
                    {
                        spawnIndex = j;
                        break;
                    }
                }

                if (spawnIndex != -1)
                {
                    // ��ȯ ����Ʈ ���� �� ���
                    Vector3 spawnPosition = enemyBattleSystem.GetEnemyPos()[spawnIndex].position;
                    ParticleSystem effect = Instantiate(summonEffect, spawnPosition, Quaternion.identity);
                    effect.Play();

                    // ����Ʈ�� ���� �� ����
                    StartCoroutine(DestroyEffectAfterDuration(effect));

                    // ���� ��ȯ
                    int randomEnemyIndex = Random.Range(0, summonEnemyPrefabs.Length);
                    GameObject enemy = Instantiate(summonEnemyPrefabs[randomEnemyIndex], spawnPosition, Quaternion.identity);
                    BattleEnemyFSMController enemyController = enemy.GetComponent<BattleEnemyFSMController>();
                    if (enemyController != null)
                    {
                        enemyController.InitValues();
                        enemy.transform.SetParent(enemyBattleSystem.GetEnemyPos()[spawnIndex]);

                        // `turnBattlers`�� �߰�
                        battleSystem.GetTurnList().Add(enemyController);
                        enemyBattleSystem.GetEnemyList().Add(enemyController);

                        // 180�� ȸ�� (�ڸ� ���� �ִ� ���)
                        enemy.transform.Rotate(0f, 180f, 0f);

                        // ��ȯ �� UI ������Ʈ
                        targetUISystem.InitializeEnemyUI(enemyBattleSystem.GetEnemyList().Count);

                        Debug.Log(enemyBattleSystem.GetEnemyList().Count);

                        Debug.Log("��ȯ �Ϸ�");
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }

    private IEnumerator DestroyEffectAfterDuration(ParticleSystem effect)
    {
        // ����Ʈ ��� �ð���ŭ ���
        yield return new WaitForSeconds(effect.main.duration);

        // ����Ʈ ��ü ����
        Destroy(effect.gameObject);
    }

    // ��� ���� ����(����) ó�� (���� �ʱ�ȭ)
    public override void EnterState(E_BattleBossState state)
    {
        animator.SetInteger("state", (int)state);
    }

    // ��� ���� ��� ���� ó�� (���� ����)
    public override void UpdateState()
    {
        // �׾�����
        if (controller.health.GetCurrentHp() <= 0)
        {
            // ���� ���·� ��ȯ
            controller.TransactionToState(E_BattleBossState.Die);
            return;
        }
    }

    // ��� ���� ����(�ٸ����·� ����) ���� ó��(���� ����)
    public override void ExitState()
    {

    }
}

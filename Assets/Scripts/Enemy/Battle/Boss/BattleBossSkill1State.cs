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
                    // 소환 이펙트 생성 및 재생
                    Vector3 spawnPosition = enemyBattleSystem.GetEnemyPos()[spawnIndex].position;
                    ParticleSystem effect = Instantiate(summonEffect, spawnPosition, Quaternion.identity);
                    effect.Play();

                    // 이펙트가 끝난 후 제거
                    StartCoroutine(DestroyEffectAfterDuration(effect));

                    // 몬스터 소환
                    int randomEnemyIndex = Random.Range(0, summonEnemyPrefabs.Length);
                    GameObject enemy = Instantiate(summonEnemyPrefabs[randomEnemyIndex], spawnPosition, Quaternion.identity);
                    BattleEnemyFSMController enemyController = enemy.GetComponent<BattleEnemyFSMController>();
                    if (enemyController != null)
                    {
                        enemyController.InitValues();
                        enemy.transform.SetParent(enemyBattleSystem.GetEnemyPos()[spawnIndex]);

                        // `turnBattlers`에 추가
                        battleSystem.GetTurnList().Add(enemyController);
                        enemyBattleSystem.GetEnemyList().Add(enemyController);

                        // 180도 회전 (뒤를 보고 있는 경우)
                        enemy.transform.Rotate(0f, 180f, 0f);

                        // 소환 후 UI 업데이트
                        targetUISystem.InitializeEnemyUI(enemyBattleSystem.GetEnemyList().Count);

                        Debug.Log(enemyBattleSystem.GetEnemyList().Count);

                        Debug.Log("소환 완료");
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
        // 이펙트 재생 시간만큼 대기
        yield return new WaitForSeconds(effect.main.duration);

        // 이펙트 객체 제거
        Destroy(effect.gameObject);
    }

    // 대기 상태 시작(진입) 처리 (상태 초기화)
    public override void EnterState(E_BattleBossState state)
    {
        animator.SetInteger("state", (int)state);
    }

    // 대기 상태 기능 동작 처리 (상태 실행)
    public override void UpdateState()
    {
        // 죽엇는지
        if (controller.health.GetCurrentHp() <= 0)
        {
            // 죽음 상태로 전환
            controller.TransactionToState(E_BattleBossState.Die);
            return;
        }
    }

    // 대기 상태 종료(다른상태로 전이) 동작 처리(상태 정리)
    public override void ExitState()
    {

    }
}

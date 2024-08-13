using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBossSkill2State : BattleBossState
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform buttlePos;

    public void AllTarget()
    {
        EnemyBattleSystem enemyBattleSystem = controller.GetBattleSystem().GetEnemySystem();
        List<BattleBehaviourComponent> playerList = enemyBattleSystem.GetSelectTargetList();

        foreach (var player in playerList)
        {
            // 플레이어의 위치로 총알을 발사
            GameObject bullet = Instantiate(bulletPrefab, buttlePos.position, Quaternion.identity);
            StartCoroutine(MoveBullet(bullet, player));
        }
    }

    private IEnumerator MoveBullet(GameObject bullet, BattleBehaviourComponent target)
    {
        // 목표 위치 보정
        Vector3 targetPosition = target.transform.position + new Vector3(0, 1f, 0); // 목표 위치의 Y축 보정

        // 미사일이 목표로 이동
        while (bullet != null && target != null && Vector3.Distance(bullet.transform.position, targetPosition) > 0.1f)
        {
            bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, targetPosition, Time.deltaTime * 30);
            yield return null;
        }

        // 히트 프리펩 생성
        if (bullet != null)
        {
            // 폭발 효과 생성
            GameObject explosion = Instantiate(explosionPrefab, bullet.transform.position, Quaternion.identity);
            Destroy(bullet);

            // 폭발 프리팹 삭제를 위한 대기
            Destroy(explosion, 1f); // 1초 후 폭발 효과 삭제

            // 타겟에 도달했을 때의 로직 (예: 타겟에 데미지 주기 등)
            if (target != null && target is BattleCharacterState player)
            {
                player.HitAll(controller.Damage);
            }
        }
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

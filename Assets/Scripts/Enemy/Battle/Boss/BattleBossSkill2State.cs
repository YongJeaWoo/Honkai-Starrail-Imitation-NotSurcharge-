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
            // �÷��̾��� ��ġ�� �Ѿ��� �߻�
            GameObject bullet = Instantiate(bulletPrefab, buttlePos.position, Quaternion.identity);
            StartCoroutine(MoveBullet(bullet, player));
        }
    }

    private IEnumerator MoveBullet(GameObject bullet, BattleBehaviourComponent target)
    {
        // ��ǥ ��ġ ����
        Vector3 targetPosition = target.transform.position + new Vector3(0, 1f, 0); // ��ǥ ��ġ�� Y�� ����

        // �̻����� ��ǥ�� �̵�
        while (bullet != null && target != null && Vector3.Distance(bullet.transform.position, targetPosition) > 0.1f)
        {
            bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, targetPosition, Time.deltaTime * 30);
            yield return null;
        }

        // ��Ʈ ������ ����
        if (bullet != null)
        {
            // ���� ȿ�� ����
            GameObject explosion = Instantiate(explosionPrefab, bullet.transform.position, Quaternion.identity);
            Destroy(bullet);

            // ���� ������ ������ ���� ���
            Destroy(explosion, 1f); // 1�� �� ���� ȿ�� ����

            // Ÿ�ٿ� �������� ���� ���� (��: Ÿ�ٿ� ������ �ֱ� ��)
            if (target != null && target is BattleCharacterState player)
            {
                player.HitAll(controller.Damage);
            }
        }
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

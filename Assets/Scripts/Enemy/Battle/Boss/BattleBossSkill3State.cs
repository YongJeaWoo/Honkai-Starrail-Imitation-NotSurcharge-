using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBossSkill3State : BattleBossState
{
    [SerializeField] private GameObject buttleObj;
    [SerializeField] private GameObject explosionObj;

    // �⺻ ����
    protected void StrongAttack()
    {
        controller.SetRandomAttackTarget();

        // Ÿ���� ���������� ����
        BattleBehaviourComponent target = controller.GetAttackTarget();

        // Ÿ���� �����ϴ��� Ȯ��
        if (target == null)
        {
            Debug.LogError("Ÿ���� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �Ѿ� ����
        GameObject bullet = Instantiate(buttleObj, controller.GetButtlePos().position, Quaternion.identity);

        // �Ѿ��� Ÿ������ �̵���Ű�� �ڷ�ƾ ����
        StartCoroutine(MoveBullet(bullet, target));
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
            GameObject explosion = Instantiate(explosionObj, bullet.transform.position, Quaternion.identity);
            Destroy(bullet);

            // ���� ������ ������ ���� ���
            Destroy(explosion, 1f); // 1�� �� ���� ȿ�� ����

            // Ÿ�ٿ� �������� ���� ���� (��: Ÿ�ٿ� ������ �ֱ� ��)
            if (target != null && target is BattleCharacterState player)
            {
                player.Hit(controller.Damage * 5);
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

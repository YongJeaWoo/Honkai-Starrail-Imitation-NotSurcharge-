using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBossAttackState : BattleBossState
{
    // �⺻ ����
    protected void BasicAttack(BattleBehaviourComponent target)
    {
        Debug.Log(target + "��(��) ���ǽ��ϴ�");

        // ���õ� Ÿ���� BattleCharacterState Ÿ������ Ȯ�� �� Hit �޼��� ȣ��
        if (target is BattleCharacterState)
        {
            (target as BattleCharacterState).Hit(controller.Damage);
        }
    }

    public override void EnterState(E_BattleBossState state)
    {
        if (controller.attackCount < 2)
        {
            animator.SetInteger("state", (int)state);

            controller.SetRandomAttackTarget();

            BasicAttack(controller.GetAttackTarget());

            controller.attackCount++;
        }
        else
        {
            controller.attackCount = 0;

            EnemyBattleSystem enemyBattleSystem = controller.GetBattleSystem().GetEnemySystem();
            bool canSummon = enemyBattleSystem.HasEmptySlots();

            if (canSummon)
            {
                int randomSkill = Random.Range(0, 2);

                if (randomSkill == 0)
                {
                    controller.TransactionToState(E_BattleBossState.Skill1);
                }
                else
                {
                    controller.TransactionToState(E_BattleBossState.Skill2);
                }
            }
            else
            {
                controller.TransactionToState(E_BattleBossState.Skill2);
            }
        }
    }

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

    public override void ExitState()
    {

    }
}


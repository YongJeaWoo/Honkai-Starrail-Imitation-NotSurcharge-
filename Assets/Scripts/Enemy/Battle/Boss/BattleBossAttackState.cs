using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBossAttackState : BattleBossState
{
    // 기본 공격
    protected void BasicAttack(BattleBehaviourComponent target)
    {
        Debug.Log(target + "을(를) 때렷습니다");

        // 선택된 타겟이 BattleCharacterState 타입인지 확인 후 Hit 메서드 호출
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
        // 죽엇는지
        if (controller.health.GetCurrentHp() <= 0)
        {
            // 죽음 상태로 전환
            controller.TransactionToState(E_BattleBossState.Die);
            return;
        }


    }

    public override void ExitState()
    {

    }
}


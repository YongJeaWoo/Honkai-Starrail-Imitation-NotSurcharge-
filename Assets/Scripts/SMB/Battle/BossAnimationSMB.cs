using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationSMB : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var fsmController = animator.GetComponent<BattleFSMController>();

        // »ì¾Ñ´ÂÁö
        if (fsmController.health.GetCurrentHp() > 0)
        {
            ((BattleBossFSMController)fsmController).TransactionToState(E_BattleBossState.Idle);
        }

        fsmController.EndTurn();
    }
}

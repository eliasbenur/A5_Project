using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard_SearchAction_Behaviour : State_Machine_Controller
{
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Get_IAController(animator);

        resetVars(animator);

        //[Min, max)
        int randomTmp = Random.Range(1, 2);
        switch (randomTmp)
        {
            case 1:
                animator.SetBool("patrollingZone", true);
                animator.GetParameter(0);
                break;
            default:
                break;
        }
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    /* Reset the variables of the Animation */ 
    void resetVars(Animator animator_)
    {
        animator_.SetBool("patrollingZone", false);
    }
}

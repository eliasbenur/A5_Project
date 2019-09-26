using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Machine_Controller : StateMachineBehaviour
{
    public AI_Controller characterController;

    public AI_Controller Get_IAController(Animator animator)
    {
        if (characterController == null)
        {
            characterController = animator.GetComponent<AI_Controller>();
        }
        return characterController;
    }
}

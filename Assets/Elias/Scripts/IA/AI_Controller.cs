using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    public enum IA_TYPE
    {
        Guard = 1
    }
    public IA_TYPE ia_type;

    // Start is called before the first frame update
    void Start()
    {
        Animator animator;
        if (GetComponent<Animator>() == null)
        {
            animator = gameObject.AddComponent<Animator>();
        }
        else
        {
            animator = gameObject.GetComponent<Animator>();
        }

        switch (ia_type)
        {
            case IA_TYPE.Guard:
                animator.runtimeAnimatorController = Resources.Load("IA_States/GuardStateMachineCOntroller") as RuntimeAnimatorController;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

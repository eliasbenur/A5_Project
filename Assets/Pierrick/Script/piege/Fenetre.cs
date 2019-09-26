using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Fenetre : Obj
{
    public override void ActiveEvent()
    {
        base.ActiveEvent();
        Vector3 dir = transform.position - voleur.transform.position;
        float angle = Vector3.SignedAngle(dir, transform.up, transform.up);
        Debug.Log(angle);
        Vector3 d = Vector3.zero;
        if (angle < 90) d = Vector3.up;
        else d = -Vector3.up;
        voleur.transform.position = transform.position + d; ;
    }
}
